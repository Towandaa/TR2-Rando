﻿using TRGE.Core;

namespace TRRandomizerCore;

internal class TRVersionSupport
{
    private static readonly List<TRRandomizerType> _tr1Types = new()
    {
        TRRandomizerType.AtlanteanEggBehaviour,
        TRRandomizerType.Audio,
        TRRandomizerType.ChallengeRooms,
        TRRandomizerType.DynamicTextures,
        TRRandomizerType.DynamicEnemyTextures,
        TRRandomizerType.Enemy,
        TRRandomizerType.Environment,
        TRRandomizerType.ExtraPickups,
        TRRandomizerType.GymOutfit,
        TRRandomizerType.GlitchedSecrets,
        TRRandomizerType.HardEnvironment,
        TRRandomizerType.HardSecrets,
        TRRandomizerType.HiddenEnemies,
        TRRandomizerType.Item,
        TRRandomizerType.ItemSprite,
        TRRandomizerType.KeyItems,
        TRRandomizerType.KeyItemTextures,
        TRRandomizerType.LarsonBehaviour,
        TRRandomizerType.MeshSwaps,
        TRRandomizerType.NightMode,
        TRRandomizerType.Outfit,
        TRRandomizerType.RewardRooms,
        TRRandomizerType.Secret,
        TRRandomizerType.SecretModels,
        TRRandomizerType.SecretReward,
        TRRandomizerType.SecretTextures,
        TRRandomizerType.SFX,
        TRRandomizerType.StartPosition,
        TRRandomizerType.Traps,
        TRRandomizerType.Texture
    };

    private static readonly List<TRRandomizerType> _tr1MainTypes = new()
    {
        TRRandomizerType.AmbientTracks,
        TRRandomizerType.Ammoless,
        TRRandomizerType.Braid,
        TRRandomizerType.ClonedEnemies,
        TRRandomizerType.DisableDemos,
        TRRandomizerType.Health,
        TRRandomizerType.LevelSequence,
        TRRandomizerType.Mediless,
        TRRandomizerType.SecretCount,
        TRRandomizerType.Text,
        TRRandomizerType.Unarmed,
        TRRandomizerType.WaterColour
    };

    private static readonly List<TRRandomizerType> _tr2Types = new()
    {
        TRRandomizerType.AmbientTracks,
        TRRandomizerType.Ammoless,
        TRRandomizerType.Audio,
        TRRandomizerType.BirdMonsterBehaviour,
        TRRandomizerType.Braid,
        TRRandomizerType.DisableDemos,
        TRRandomizerType.DragonSpawn,
        TRRandomizerType.DynamicEnemyTextures,
        TRRandomizerType.DynamicTextures,
        TRRandomizerType.Enemy,
        TRRandomizerType.Environment,
        TRRandomizerType.GlitchedSecrets,
        TRRandomizerType.HardSecrets,
        TRRandomizerType.Item,
        TRRandomizerType.KeyItems,
        TRRandomizerType.KeyItemTextures,
        TRRandomizerType.Ladders,
        TRRandomizerType.LevelSequence,
        TRRandomizerType.MeshSwaps,
        TRRandomizerType.NightMode,
        TRRandomizerType.Outfit,
        TRRandomizerType.OutfitDagger,
        TRRandomizerType.Secret,
        TRRandomizerType.SecretAudio,
        TRRandomizerType.SecretReward,
        TRRandomizerType.SecretTextures,
        TRRandomizerType.SFX,
        TRRandomizerType.StartPosition,
        TRRandomizerType.Sunset,
        TRRandomizerType.Text,
        TRRandomizerType.Texture,
        TRRandomizerType.Unarmed,
        TRRandomizerType.ItemSprite
    };

    private static readonly List<TRRandomizerType> _tr3Types = new()
    {
        TRRandomizerType.AmbientTracks,
        TRRandomizerType.Ammoless,
        TRRandomizerType.Audio,
        TRRandomizerType.Braid,
        TRRandomizerType.DynamicEnemyTextures,
        TRRandomizerType.Enemy,
        TRRandomizerType.Environment,
        TRRandomizerType.GlitchedSecrets,
        TRRandomizerType.GlobeDisplay,
        TRRandomizerType.HardSecrets,
        TRRandomizerType.Item,
        TRRandomizerType.KeyItems,
        TRRandomizerType.Ladders,
        TRRandomizerType.LevelSequence,
        TRRandomizerType.NightMode,
        TRRandomizerType.Outfit,
        TRRandomizerType.RewardRooms,
        TRRandomizerType.Secret,
        TRRandomizerType.SecretAudio,
        TRRandomizerType.SecretReward,
        TRRandomizerType.SecretTextures,
        TRRandomizerType.SFX,
        TRRandomizerType.StartPosition,
        TRRandomizerType.Text,
        TRRandomizerType.Texture,
        TRRandomizerType.Unarmed,
        TRRandomizerType.VFX
    };

    private static readonly List<TRRandomizerType> _tr3MainTypes = new()
    {
        TRRandomizerType.Weather
    };

    private static readonly Dictionary<TRVersion, TRVersionSupportGroup> _supportedTypes = new()
    {
        [TRVersion.TR1] = new TRVersionSupportGroup
        {
            DefaultSupport = _tr1Types,
            PatchSupport = _tr1MainTypes
        },
        [TRVersion.TR2] = new TRVersionSupportGroup
        {
            DefaultSupport = _tr2Types
        },
        [TRVersion.TR3] = new TRVersionSupportGroup
        {
            DefaultSupport = _tr3Types,
            PatchSupport = _tr3MainTypes
        }
    };

    private static readonly Dictionary<TRVersion, List<string>> _versionExes = new()
    {
        [TRVersion.TR1] = new List<string> { "Tomb1Main.exe", "tombati.exe" },
        [TRVersion.TR2] = new List<string> { "Tomb2.exe" },
        [TRVersion.TR3] = new List<string> { "Tomb3.exe" }
    };

    public static bool IsRandomizationSupported(TREdition edition)
    {
        return _supportedTypes.ContainsKey(edition.Version);
    }

    public static bool IsRandomizationSupported(TREdition edition, TRRandomizerType randomizerType)
    {
        if (!IsRandomizationSupported(edition))
        {
            return false;
        }

        TRVersion version = edition.Version;
        TRVersionSupportGroup supportGroup = _supportedTypes[version];
        bool supported = supportGroup.DefaultSupport.Contains(randomizerType);

        // If not supported but we're using a community patch, does that support it?
        if (!supported && edition.IsCommunityPatch && supportGroup.HasPatchSupport)
        {
            supported = supportGroup.PatchSupport.Contains(randomizerType);
        }

        return supported;
    }

    public static List<string> GetExecutables(TREdition edition)
    {
        List<string> exes = new();
        if (_versionExes.ContainsKey(edition.Version))
        {
            exes.AddRange(_versionExes[edition.Version]);
        }
        return exes;
    }
}

internal class TRVersionSupportGroup
{
    internal List<TRRandomizerType> DefaultSupport { get; set; }
    internal List<TRRandomizerType> PatchSupport { get; set; }
    internal bool HasPatchSupport => PatchSupport != null;
}
