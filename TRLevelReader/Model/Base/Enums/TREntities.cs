﻿namespace TRLevelReader.Model.Enums
{
    //_S are sprites
    //_M are menu items

    //_P are pickup items
    //_N are nullmeshes (no render/collision)
    //_H are helper entities (not placed)
    //_U are unused entities
    public enum TREntities
    {
        Lara = 0,
        LaraPistolAnim_H,
        LaraShotgunAnim_H,
        LaraMagnumAnim_H,
        LaraUziAnimation_H,
        LaraMiscAnim_H,
        Doppelganger,
        Wolf,
        Bear,
        Bat,
        CrocodileLand,
        CrocodileWater,
        Lion,
        Lioness,
        Panther,
        Gorilla,
        RatLand,
        RatWater,
        TRex,
        Raptor,
        FlyingAtlantean,
        ShootingAtlantean_N,    // Requires FlyingAtlantean
        NonShootingAtlantean_N, // Requires FlyingAtlantean
        Centaur,
        Mummy, // Qualopec only, Egypt = NonShootingAtlantean_N
        DinoWarrior_U,
        Fish_U,
        Larson,
        Pierre,
        Skateboard,
        SkateboardKid,
        Cowboy,
        Kold,
        Natla,
        Adam,
        FallingBlock,
        SwingingBlade,
        TeethSpikes,
        RollingBall,
        Dart_H,
        DartEmitter,
        LiftingDoor,
        SlammingDoor,
        DamoclesSword,
        ThorHammerHandle,
        ThorHammerBlock,
        ThorLightning,
        Barricade,
        PushBlock1,
        PushBlock2,
        PushBlock3,
        PushBlock4,
        MovingBlock,
        FallingCeiling1,
        FallingCeiling2,
        WallSwitch,
        UnderwaterSwitch,
        Door1,
        Door2,
        Door3,
        Door4,
        Door5,
        Door6,
        Door7,
        Door8,
        Trapdoor1,
        Trapdoor2,
        Trapdoor3,
        BridgeFlat,
        BridgeTilt1,
        BridgeTilt2,
        PassportOpen_M_H,
        Compass_M_H,
        LaraHomePhoto_M_H,
        Animating1,
        Animating2,
        Animating3,
        CutsceneActor1,
        CutsceneActor2,
        CutsceneActor3,
        CutsceneActor4,
        PassportClosed_M_H,
        Map_M_U,
        SavegameCrystal_P,
        Pistols_S_P,
        Shotgun_S_P,
        Magnums_S_P,
        Uzis_S_P,
        PistolAmmo_S_P,
        ShotgunAmmo_S_P,
        MagnumAmmo_S_P,
        UziAmmo_S_P,
        Explosive_S_U,
        SmallMed_S_P,
        LargeMed_S_P,
        Sunglasses_M_H,
        CassettePlayer_M_H,
        DirectionKeys_M_H,
        Flashlight_U,
        Pistols_M_H,
        Shotgun_M_H,
        Magnums_M_H,
        Uzis_M_H,
        PistolAmmo_M_H,
        ShotgunAmmo_M_H,
        MagnumAmmo_M_H,
        UziAmmo_M_H,
        Explosive_M_H_U,
        SmallMed_M_H,
        LargeMed_M_H,
        Puzzle1_S_P,
        Puzzle2_S_P,
        Puzzle3_S_P,
        Puzzle4_S_P,
        Puzzle1_M_H,
        Puzzle2_M_H,
        Puzzle3_M_H,
        Puzzle4_M_H,
        PuzzleHole1,
        PuzzleHole2,
        PuzzleHole3,
        PuzzleHole4,
        PuzzleDone1,
        PuzzleDone2,
        PuzzleDone3,
        PuzzleDone4,
        LeadBar_S_P,
        LeadBar_M_H,
        MidasHand_N,
        Key1_S_P,
        Key2_S_P,
        Key3_S_P,
        Key4_S_P,
        Key1_M_H,
        Key2_M_H,
        Key3_M_H,
        Key4_M_H,
        Keyhole1,
        Keyhole2,
        Keyhole3,
        Keyhole4,
        Quest1_S_P,
        Quest2_S_P,
        ScionPiece1_S_P, // ToQ and Sanctuary
        ScionPiece2_S_P, // From Pierre
        ScionPiece3_S_P, // Atlantis and Great Pyramid
        ScionPiece4_S_P, // ?
        ScionHolder,
        Quest1_M_H,
        Quest2_M_H,
        ScionPiece_M_H,
        Explosion1_S_H,
        Explosion2_S_H,
        WaterRipples1_S_H,
        WaterRipples2_S_H,
        Bubbles1_S_H,
        Bubbles2_S_H,
        BubbleEmitter_N,
        Blood1_S_H,
        Blood2_S_H,
        DartEffect_S_H,
        CentaurStatue,
        NatlasMineShack,
        AtlanteanEgg,
        Ricochet_S_H,
        Sparkles_S_H,
        Gunflare_H,
        Dust_S_H,
        BodyPart_N,
        CameraTarget_N,
        WaterfallMist_N,
        Missile1_H, // Natla
        Missile2_H, // Meatball
        Missile3_H, // Bone
        Missile4_U,
        Missile5_U,
        LavaParticles_S_H,
        LavaEmitter_N,
        Flame_S_H,
        FlameEmitter_N,
        AtlanteanLava,
        AdamEgg,
        Motorboat,
        Earthquake_N,
        SecretScion_M_H, // Unused model, so repurposed as duplicate scion for importing for secret pickups
        Unused2_U,
        Unused3_U,
        Unused4_U,
        Unused5_U,
        LaraPonytail_H_U,
        FontGraphics_S_H,

        // Egyptian mummies are just NonShootingAtlantean_N, but both depend on FlyingMutant for meshes
        BandagedAtlantean = 1000,
        MeatyAtlantean,
        BandagedFlyer,
        MeatyFlyer,

        // Extra Lara animations
        LaraMiscAnim_H_General = 2000,
        LaraMiscAnim_H_Valley,    // T-Rex death
        LaraMiscAnim_H_Qualopec,  // Scion pickup
        LaraMiscAnim_H_Midas,     // Turning to gold
        LaraMiscAnim_H_Sanctuary, // Scion pickup (includes level end)
        LaraMiscAnim_H_Atlantis,  // Scion grab (includes level end)
        LaraMiscAnim_H_Pyramid,   // Adam death

        // Key Item Alias = 10000 + ((level.OriginalSequence - 1) * 1000) + KeyTypeID + Room
        CavesKeyItemBase = 10000,

        VilcabambaKeyItemBase = 11000,
        VilcabambaGoldIdol = 11124,
        VilcabambaSilverKey = 11143,
        
        ValleyKeyItemBase = 12000,
        ValleyCogAbovePool = 12150,
        ValleyCogInWater = 12158,
        ValleyCogAtBridge = 12168,
        
        QualopecKeyItemBase = 13000,

        FollyKeyItemBase = 14000,
        FollyNeptuneKey = 14162,
        FollyDamoclesKey = 14169,
        FollyThorKey = 14176,
        FollyAtlasKey = 14177,
        
        ColosseumKeyItemBase = 15000,
        ColosseumSuperBrightKey = 15183,
        
        MidasKeyItemBase = 16000,
        MidasLeadBarTempleRoom = 16133,
        MidasLeadBarFireRoom = 16149,
        MidasLeadBarSpikeRoom = 16154,
        
        CisternKeyItemBase = 17000,
        CisternRustyKeyNearPierre = 17151,
        CisternSilverKeyBehindDoor = 17170,
        CisternSilverKeyBetweenDoors = 17179,
        CisternGoldKey = 17185,
        CisternRustyKeyMainRoomLedge = 17222,
        
        TihocanKeyItemBase = 18000,
        TihocanGoldKeyBeforeBlockRoom = 18144,
        TihocanRustyKeyDoubleBoulders = 18209,
        TihocanRustyKeyClangClang = 18210,
        TihocanGoldKeyPierre = 18239,

        KhamoonKeyItemBase = 19000,
        KhamoonSapphireKeySphinx = 19143,
        KhamoonSapphireKeyEnd = 19162,
        
        ObeliskKeyItemBase = 20000,
        ObeliskEyeOfHorus = 20126,
        ObeliskScarab = 20127,
        ObeliskSealOfAnubis = 20128,
        ObeliskAnkh = 20129,
        ObeliskSapphireKey = 20199,
        
        SanctuaryKeyItemBase = 21000,
        SanctuaryAnkhBehindSphinx = 21126,
        SanctuarySapphireKey = 21148,
        SanctuaryAnkhAfterKey = 21150,
        SanctuaryScarab = 21155,
        
        MinesKeyItemBase = 22000,
        MinesFuseNearCowboy = 22127,
        MinesFuseNearShack = 22147,
        MinesFuseNearConveyor = 22155,
        MinesPyramidKey = 22179,

        AtlantisKeyItemBase = 23000,

        PyramidKeyItemBase = 24000
    }
}