﻿using Newtonsoft.Json;
using System.Drawing;
using TRGE.Coord;
using TRGE.Core;
using TRLevelControl.Model;
using TRRandomizerCore.Helpers;
using TRRandomizerCore.Processors;
using TRRandomizerCore.Randomizers;
using TRRandomizerCore.Textures;
using TRTexture16Importer.Helpers;

namespace TRRandomizerCore.Editors;

public class TR1RandoEditor : TR1LevelEditor, ISettingsProvider
{
    public RandomizerSettings Settings { get; private set; }

    public TR1RandoEditor(TRDirectoryIOArgs args, TREdition edition)
        : base(args, edition) { }

    protected override void ApplyConfig(Config config)
    {
        Settings = new RandomizerSettings()
        {
            ExcludableEnemies = JsonConvert.DeserializeObject<Dictionary<short, string>>(File.ReadAllText(@"Resources\TR1\Restrictions\excludable_enemies.json"))
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

        // String rando always runs
        target++;

        // Injection checks
        target += numLevels;

        if (Settings.RandomizeStartingHealth)
        {
            target += numLevels;
        }

        if (Settings.RandomizeSecrets)
        {
            // *3 for multithreaded work
            target += numLevels * 3;
        }

        if (Settings.RandomizeEnemies)
        {
            // *3 for multithreaded work
            target += Settings.CrossLevelEnemies ? numLevels * 3 : numLevels;
        }

        if (Settings.RandomizeItems)
        {
            target += numLevels;
            if (Settings.IncludeKeyItems)
            {
                target += numLevels;
            }
        }

        if (Settings.RandomizeSecretRewardsPhysical)
        {
            target += numLevels;
        }

        if (Settings.RandomizeAudio)
        {
            target += numLevels;
        }

        if (Settings.RandomizeOutfits)
        {
            // *2 because of multithreaded approach
            target += numLevels * 2;
        }

        if (Settings.RandomizeTextures)
        {
            // *3 for multithreaded work
            target += numLevels * 3;
        }

        if (Settings.RandomizeStartPosition)
        {
            target += numLevels;
        }

        if (Settings.RandomizeNightMode)
        {
            target += numLevels;
        }

        // Environment randomizer always runs
        target += numLevels * 2;

        return target;
    }

    protected override void SaveImpl(AbstractTRScriptEditor scriptEditor, TRSaveMonitor monitor)
    {
        List<TR1ScriptedLevel> levels = new(
            scriptEditor.EnabledScriptedLevels.Cast<TR1ScriptedLevel>().ToList()
        );

        if (scriptEditor.GymAvailable)
        {
            levels.Add(scriptEditor.AssaultLevel as TR1ScriptedLevel);
        }

        string backupDirectory = _io.BackupDirectory.FullName;
        string wipDirectory = _io.WIPOutputDirectory.FullName;

        TR1ScriptEditor scriptEd = scriptEditor as TR1ScriptEditor;
        if (Settings.DevelopmentMode)
        {
            scriptEd.EnableCheats = true;
            scriptEd.EnableConsole = true;
            scriptEditor.SaveScript();
        }

        ItemFactory<TR1Entity> itemFactory = new(@"Resources\TR1\Items\repurposable_items.json");
        TR1TextureMonitorBroker textureMonitor = new();

        TR1ItemRandomizer itemRandomizer = new()
        {
            ScriptEditor = scriptEditor,
            Levels = levels,
            BasePath = wipDirectory,
            BackupPath = backupDirectory,
            SaveMonitor = monitor,
            Settings = Settings,
            ItemFactory = itemFactory
        };

        TR1EnvironmentRandomizer environmentRandomizer = new()
        {
            ScriptEditor = scriptEditor,
            Levels = levels,
            BasePath = wipDirectory,
            BackupPath = backupDirectory,
            SaveMonitor = monitor,
            Settings = Settings,
            TextureMonitor = textureMonitor
        };
        environmentRandomizer.AllocateMirroredLevels(Settings.EnvironmentSeed);

        using (textureMonitor)
        {
            if (!monitor.IsCancelled)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing text");
                new TR1GameStringRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.GameStringsSeed);
            }

            if (!monitor.IsCancelled)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Validating data injections");
                new TR1InjectionProcessor
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                }.Run();
            }

            if (!monitor.IsCancelled && Settings.RandomizeStartingHealth)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing health");
                new TR1HealthRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.HealthSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeSecrets)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing secrets");
                new TR1SecretRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    ItemFactory = itemFactory,
                    Mirrorer = environmentRandomizer
                }.Randomize(Settings.SecretSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeEnemies)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing enemies");
                new TR1EnemyRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    ItemFactory = itemFactory,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.EnemySeed);
            }

            // Tomp1 doesn't have droppable enmies so item rando takes places after enemy rando
            // - this allows for accounting for newly added items.
            if (!monitor.IsCancelled && Settings.RandomizeItems)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing standard items");
                itemRandomizer.Randomize(Settings.ItemSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeSecretRewardsPhysical)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing secret rewards");
                new TR1SecretRewardRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.SecretRewardsPhysicalSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeStartPosition)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing start positions");
                new TR1StartPositionRandomizer
                {
                    ScriptEditor = scriptEditor,
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

            if (!monitor.IsCancelled && Settings.RandomizeOutfits)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing Lara's appearance");
                new TR1OutfitRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.OutfitSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeAudio)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing audio tracks");
                new TR1AudioRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings
                }.Randomize(Settings.AudioSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeNightMode)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing night mode");
                new TR1NightModeRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.NightModeSeed);
            }

            if (!monitor.IsCancelled && Settings.RandomizeTextures)
            {
                monitor.FireSaveStateBeginning(TRSaveCategory.Custom, "Randomizing textures");
                new TR1TextureRandomizer
                {
                    ScriptEditor = scriptEditor,
                    Levels = levels,
                    BasePath = wipDirectory,
                    BackupPath = backupDirectory,
                    SaveMonitor = monitor,
                    Settings = Settings,
                    TextureMonitor = textureMonitor
                }.Randomize(Settings.TextureSeed);
            }
        }

        AmendTitleAndCredits(scriptEditor, monitor);
    }

    private void AmendTitleAndCredits(AbstractTRScriptEditor scriptEditor, TRSaveMonitor monitor)
    {
        TR1Script script = scriptEditor.Script as TR1Script;

        string mainMenuPic = Path.GetFileName(script.MainMenuPicture);
        string backupTitle = Path.Combine(GetReadBasePath(), mainMenuPic);
        if (File.Exists(backupTitle))
        {
            string editedTitle = Path.Combine(GetWriteBasePath(), mainMenuPic);
            using BitmapGraphics bg = new(new Bitmap(backupTitle));
            using Bitmap badge = new(@"Resources\Shared\Graphics\goldbadge-small.png");
            bg.Graphics.DrawImage(badge, new Rectangle(706, 537, badge.Width, badge.Height));
            bg.Bitmap.Save(editedTitle);
        }

        {
            string creditFile = Path.Combine(_io.OutputDirectory.FullName, "trrando.png");
            string creditPath = @"data\trrando.png";

            using (BitmapGraphics bg = new(new Bitmap(1920, 1080)))
            using (Bitmap badge = new(@"Resources\Shared\Graphics\goldbadge-large.png"))
            {
                bg.Graphics.FillRectangle(new SolidBrush(Color.Black), new Rectangle(0, 0, 1920, 1080));
                bg.Graphics.DrawImage(badge, new Rectangle(960 - badge.Width / 2, 540 - badge.Height / 2, badge.Width, badge.Height));
                bg.Bitmap.Save(creditFile);
            }

            TR1ScriptedLevel finalLevel = scriptEditor.Levels.ToList().Find(l => l.IsFinalLevel) as TR1ScriptedLevel;
            if (finalLevel.HasCutScene)
            {
                finalLevel = finalLevel.CutSceneLevel as TR1ScriptedLevel;
            }

            finalLevel.AddSequenceBefore(LevelSequenceType.Total_Stats, new DisplayPictureLevelSequence
            {
                Type = LevelSequenceType.Display_Picture,
                DisplayTime = 5,
                PicturePath = creditPath
            });

            script.AddAdditionalBackupFile(creditPath);
            scriptEditor.SaveScript();
        }

        monitor.FireSaveStateChanged(1);
    }
}
