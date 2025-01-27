﻿using Newtonsoft.Json;
using TRGE.Coord;
using TRGE.Core;
using TRLevelControl.Model;
using TRRandomizerCore.Helpers;
using TRRandomizerCore.Processors;
using TRRandomizerCore.Randomizers;
using TRRandomizerCore.Textures;

namespace TRRandomizerCore.Editors;

public class TR2RandoEditor : TR2LevelEditor, ISettingsProvider
{
    public RandomizerSettings Settings { get; private set; }

    public TR2RandoEditor(TRDirectoryIOArgs args, TREdition edition)
        : base(args, edition) { }

    protected override void ApplyConfig(Config config)
    {
        Settings = new RandomizerSettings
        {
            ExcludableEnemies = JsonConvert.DeserializeObject<Dictionary<short, string>>(File.ReadAllText(@"Resources\TR2\Restrictions\excludable_enemies.json"))
        };
        Settings.ApplyConfig(config);
    }

    protected override void StoreConfig(Config config)
    {
        Settings.StoreConfig(config);
    }

    protected override int GetSaveTarget(int numLevels)
    {
        int target = base.GetSaveTarget(numLevels);

        if (Settings.RandomizeGameStrings || Settings.ReassignPuzzleItems)
        {
            target++;
        }

        if (Settings.RandomizeNightMode)
        {
            target += numLevels;
            if (!Settings.RandomizeTextures)
            {
                // Texture randomizer will run if night mode is on to ensure skyboxes and such like match
                target += numLevels;
            }
        }

        if (Settings.RandomizeSecrets)
        {
            target += numLevels;
        }

        if (Settings.RandomizeAudio)
        {
            target += numLevels;
        }

        if (Settings.RandomizeItems)
        {
            // Standard/key item rando followed by unarmed logic after enemy rando
            target += numLevels * 2;
            if (Settings.RandomizeItemSprites)
            {
                target += numLevels;
            }
            if (Settings.IncludeKeyItems)
            {
                target += numLevels;
            }
        }

        if (Settings.RandomizeStartPosition)
        {
            target += numLevels;
        }

        if (Settings.DeduplicateTextures)
        {
            // *2 because of multithreaded approach
            target += numLevels * 2;
        }

        if (Settings.ReassignPuzzleItems)
        {
            // For TR2ModelAdjuster
            target += numLevels;
        }

        if (Settings.RandomizeEnemies)
        {
            // 3 for multithreading cross-level work
            target += Settings.CrossLevelEnemies ? numLevels * 3 : numLevels;
            // And again for eliminating unused enemies
            target += numLevels;
        }

        if (Settings.RandomizeTextures)
        {
            // *3 because of multithreaded approach
            target += numLevels * 3;
        }

        if (Settings.RandomizeOutfits)
        {
            // *2 because of multithreaded approach
            target += numLevels * 2;
        }

        // Environment randomizer always runs
        target += numLevels * 2;

        return target;
    }

    protected override void SaveImpl(AbstractTRScriptEditor scriptEditor, TRSaveMonitor monitor)
    {
        List<TR2ScriptedLevel> levels = new(
            scriptEditor.EnabledScriptedLevels.Cast<TR2ScriptedLevel>().ToList()
        );

        if (scriptEditor.GymAvailable)
        {
            levels.Add(scriptEditor.AssaultLevel as TR2ScriptedLevel);
        }

        // Each processor will have a reference to the script editor, so can
        // make on-the-fly changes as required.
        TR23ScriptEditor tr23ScriptEditor = scriptEditor as TR23ScriptEditor;
        string backupDirectory = _io.BackupDirectory.FullName;
        string wipDirectory = _io.WIPOutputDirectory.FullName;

        if (Settings.DevelopmentMode)
        {
            (tr23ScriptEditor.Script as TR23Script).LevelSelectEnabled = true;
            scriptEditor.SaveScript();
        }

        ItemFactory<TR2Entity> itemFactory = new()
        {
            DefaultItem = new() { Intensity1 = -1, Intensity2 = -1 }
        };
        TR2TextureMonitorBroker textureMonitor = new();

        TR2ItemRandomizer itemRandomizer = new()
        {
            ScriptEditor = tr23ScriptEditor,
            Levels = levels,
            BasePath = wipDirectory,
            BackupPath = backupDirectory,
            SaveMonitor = monitor,
            Settings = Settings,
            TextureMonitor = textureMonitor,
            ItemFactory = itemFactory,
        };

        TR2EnvironmentRandomizer environmentRandomizer = new()
        {
            ScriptEditor = tr23ScriptEditor,
            Levels = levels,
            BasePath = wipDirectory,
            BackupPath = backupDirectory,
            SaveMonitor = monitor,
            Settings = Settings,
            TextureMonitor = textureMonitor
        };
        environmentRandomizer.AllocateMirroredLevels(Settings.EnvironmentSeed);

        // Texture monitoring is needed between enemy and texture randomization
        // to track where imported enemies are placed.
        using (textureMonitor)
        {
            if (!monitor.IsCancelled && Settings.RandomizeEnemies)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Adjusting enemy entities");
                new TR2EnemyAdjuster
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    ItemFactory = itemFactory,
                }.AdjustEnemies();
            }

            if (!monitor.IsCancelled && (Settings.RandomizeGameStrings || Settings.ReassignPuzzleItems))
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Adjusting game strings");
                new TR2GameStringRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.GameStringsSeed);
            }

            if (!monitor.IsCancelled && Settings.DeduplicateTextures)
            {
                // This is needed to make as much space as possible available for cross-level enemies.
                // We do this if we are implementing cross-level enemies OR if randomizing textures,
                // as the texture mapping is optimised for levels that have been deduplicated.
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Deduplicating textures");
                new TR2TextureDeduplicator
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor
                }.Deduplicate();
            }

            if (!monitor.IsCancelled && Settings.RandomizeSecrets)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing secrets");
                new TR2SecretRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    Mirrorer = environmentRandomizer,
                    ItemFactory = itemFactory,
                }.Randomize(Settings.SecretSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeItems)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing standard items");
                itemRandomizer.Randomize(Settings.ItemSeed);
            }

            if (!monitor.IsCancelled && Settings.ReassignPuzzleItems)
            {
                // P2 items are converted to P3 in case the dragon is present as the dagger type is hardcoded.
                // Must take place before enemy randomization. OG P2 key items must be zoned based on being P3.
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Adjusting level models");
                new TR2ModelAdjuster
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor
                }.AdjustModels();
            }

            if (!monitor.IsCancelled && Settings.RandomizeEnemies)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing enemies");
                new TR2EnemyRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.EnemySeed);
            }

            // Randomize ammo/weapon in unarmed levels post enemy randomization
            if (!monitor.IsCancelled && Settings.RandomizeItems)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing unarmed level items");
                itemRandomizer.RandomizeAmmo();
            }

            if (!monitor.IsCancelled && Settings.RandomizeStartPosition)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing start positions");
                new TR2StartPositionRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.StartPositionSeed);
            }

            if (!monitor.IsCancelled)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, Settings.RandomizeEnvironment ? "Randomizing environment" : "Applying default environment packs");
                environmentRandomizer.Randomize(Settings.EnvironmentSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeItems && Settings.IncludeKeyItems)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing key items");
                itemRandomizer.RandomizeKeyItems();
            }

            if (!monitor.IsCancelled)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Finalizing environment changes");
                environmentRandomizer.FinalizeEnvironment();
            }

            if (!monitor.IsCancelled && Settings.RandomizeAudio)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing audio tracks");
                new TR2AudioRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.AudioSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeOutfits)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing outfits");
                new TR2OutfitRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.OutfitSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeNightMode)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing night mode");
                new TR2NightModeRandomizer
                {
                    ScriptEditor = tr23ScriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.NightModeSeed);
            }

            if (!monitor.IsCancelled)
            {
                if (Settings.RandomizeTextures)
                {
                    monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing textures");
                    new TR2TextureRandomizer
                    {
                        ScriptEditor = tr23ScriptEditor,
                        Levels = levels,
                        BasePath = wipDirectory,
                        BackupPath = backupDirectory,
                        SaveMonitor = monitor,
                        Settings = Settings,
                        TextureMonitor = textureMonitor
                    }.Randomize(Settings.TextureSeed);
                }
                else if (Settings.RandomizeNightMode)
                {
                    monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing night mode textures");
                    new TR2TextureRandomizer
                    {
                        ScriptEditor = tr23ScriptEditor,
                        Levels = levels,
                        BasePath = wipDirectory,
                        BackupPath = backupDirectory,
                        SaveMonitor = monitor,
                        Settings = Settings,
                        TextureMonitor = textureMonitor
                    }.Randomize(Settings.NightModeSeed);
                }
            }

            if (!monitor.IsCancelled && Settings.RandomizeItems && Settings.RandomizeItemSprites)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing Sprites");
                itemRandomizer.RandomizeLevelsSprites();
            }
        }
    }
}
