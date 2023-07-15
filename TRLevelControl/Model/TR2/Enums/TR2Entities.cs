﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRLevelControl.Model.Enums
{
    //_S are sprites
    //_M are menu items

    //_P are pickup items
    //_N are nullmeshes (no render/collision)
    //_H are helper entities (not placed)
    //_U are unused entities
    public enum TR2Entities
    {
        Lara = 0,
        LaraPistolAnim_H,
        LaraPonytail_H,
        LaraShotgunAnim_H,
        LaraAutoAnim_H,
        LaraUziAnim_H,
        LaraM16Anim_H,
        LaraGrenadeAnim_H,
        LaraHarpoonAnim_H,
        LaraFlareAnim_H,
        LaraSnowmobAnim_H,
        LaraBoatAnim_H,
        LaraMiscAnim_H,
        RedSnowmobile,
        Boat,
        Doberman,
        MaskedGoon1,
        MaskedGoon2,
        MaskedGoon3,
        Knifethrower,
        ShotgunGoon,
        Rat,
        DragonFront_H,
        DragonBack_H,
        Gondola,
        Shark,
        YellowMorayEel,
        BlackMorayEel,
        Barracuda,
        ScubaDiver,
        Gunman1,
        Gunman2,
        StickWieldingGoon1,
        StickWieldingGoon2,
        FlamethrowerGoon,
        Spider = 36,
        GiantSpider,
        Crow,
        TigerOrSnowLeopard,
        MarcoBartoli,
        XianGuardSpear,
        XianGuardSpearStatue,
        XianGuardSword,
        XianGuardSwordStatue,
        Yeti,
        BirdMonster,
        Eagle,
        Mercenary1,
        Mercenary2,
        Mercenary3,
        BlackSnowmob,
        MercSnowmobDriver,
        MonkWithLongStick,
        MonkWithKnifeStick,
        FallingBlock,
        FallingBlock2,
        LooseBoards = 57,
        SandbagOrBallsack,
        TeethSpikesOrGlassShards,
        RollingBall,
        Disc_H,
        Discgun,
        Drawbridge,
        SlammingDoor,
        Elevator,
        Minisub,
        PushBlock1,
        PushBlock2,
        PushBlock3,
        PushBlock4,
        LavaBowl,
        BreakableWindow1,
        BreakableWindow2,
        AirplanePropeller = 76,
        PowerSaw,
        OverheadPulleyHook,
        FallingCeilingOrSandbag,
        RollingSpindle,
        WallMountedKnifeBlade,
        StatueWithKnifeBlade,
        BouldersOrSnowballs,
        DetatchableIcicles,
        SpikyWall,
        BouncePad,
        SpikyCeiling,
        TibetanBell,
        BoatWake_S_H,
        SnowmobileWake_S_H = 90,
        SnowmobileBelt = 91,
        WheelKnob,
        SmallWallSwitch,
        UnderwaterPropeller,
        AirFan,
        SwingingBoxOrBall,
        CutsceneActor1,
        CutsceneActor2,
        CutsceneActor3,
        UIFrame_H,
        RollingStorageDrums,
        ZiplineHandle,
        PushButtonSwitch,
        WallSwitch,
        UnderwaterSwitch,
        Door1,
        Door2,
        Door3,
        Door4,
        Door5,
        LiftingDoor1,
        LiftingDoor2,
        LiftingDoor3,
        Trapdoor1,
        Trapdoor2,
        Trapdoor3,
        BridgeFlat,
        BridgeTilt1,
        BridgeTilt2,
        JadeSecret_M_H,
        SilverSecret_M_H,
        LaraHomePhoto_M_H,
        CutsceneActor4,
        CutsceneActor5,
        CutsceneActor6,
        CutsceneActor7,
        CutsceneActor8,
        CutsceneActor9,
        CutsceneActor10,
        CutsceneActor11,
        GoldSecret_M_H = 133,
        Map_M_U,
        Pistols_S_P,
        Shotgun_S_P,
        Automags_S_P,
        Uzi_S_P,
        Harpoon_S_P,
        M16_S_P,
        GrenadeLauncher_S_P,
        PistolAmmo_S_P,
        ShotgunAmmo_S_P,
        AutoAmmo_S_P,
        UziAmmo_S_P,
        HarpoonAmmo_S_P,
        M16Ammo_S_P,
        Grenades_S_P,
        SmallMed_S_P,
        LargeMed_S_P,
        Flares_S_P,
        Flare_H,
        Sunglasses_M_H,
        CDPlayer_M_H,
        DirectionKeys_M_H,
        Pistols_M_H = 157,
        Shotgun_M_H,
        Autos_M_H,
        Uzi_M_H,
        Harpoon_M_H,
        M16_M_H,
        GrenadeLauncher_M_H,
        PistolAmmo_M_H,
        ShotgunAmmo_M_H,
        AutoAmmo_M_H,
        UziAmmo_M_H,
        HarpoonAmmo_M_H,
        M16Ammo_M_H,
        Grenades_M_H,
        SmallMed_M_H,
        LargeMed_M_H,
        Flares_M_H,
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
        GoldSecret_S_P,
        JadeSecret_S_P,
        StoneSecret_S_P,
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
        Quest1_M_H,
        Quest2_M_H,
        DragonExplosion1_H,
        DragonExplosion2_H,
        DragonExplosion3_H,
        Alarm_N,
        DrippingWater_N,
        TRex,
        SingingBirds_N,
        BartoliHideoutClock_N,
        Placeholder_N,
        DragonBonesFront_H,
        DragonBonesBack_H,
        ExtraFire_S_H,
        AquaticMine = 222,
        MenuBackground_H,
        GrayDisk_S_H,
        GongStick_H,
        Gong,
        DetonatorBox,
        Helicopter,
        Explosion_S_H,
        WaterRipples_S_H,
        Bubbles_S_H,
        Blood_S_H = 233,
        FlareSparks_S_H = 235,
        Glow_S_H,
        Ricochet_S_H = 238,
        XianGuardSparkles_S_H = 239, // shows when XianGuardSword is hovering
        Gunflare_H = 240,
        M16Gunflare_H,
        CameraTarget_N = 243,
        WaterfallMist_N,
        ScubaHarpoonProjectile_H, // a repeat of HarpoonProjectile_H
        FireBlast_S_H, // a repeat of Explosion_S_H, used by Dragon and Flamethrower
        KnifeProjectile_H = 247, // thrown by Knifethrower in Floater/DL
        GrenadeProjectile_H,
        HarpoonProjectile_H,
        LavaParticles_S_H,
        LavaAirParticleEmitter_N,
        Flame_S_H,
        FlameEmitter_N,
        Skybox_H,
        FontGraphics_S_H,
        Monk,
        DoorBell_N,
        AlarmBell_N,
        Helicopter2,
        Winston,
        LaraCutscenePlacement_N = 262,
        ShotgunShowerAnimation_H,
        DragonExplosionEmitter_N,

        // We split some entities here to allow us to differentiate between the different textures they
        // use. Entity type IDs within levels must always be TigerOrSnowLeopard for the first 3, or
        // StickWieldingGoon1 for the rest - these aliases should only be used for selecting the models.
        BengalTiger = 1000, 
        SnowLeopard,        
        WhiteTiger,
        StickWieldingGoon1Bandana,
        StickWieldingGoon1BlackJacket,
        StickWieldingGoon1BodyWarmer,
        StickWieldingGoon1GreenVest,
        StickWieldingGoon1WhiteVest,
        BarracudaUnwater, //40F, MD, LQ, Deck
        BarracudaIce,     //CoT, Ice Palace
        BarracudaXian,    //Temple

        Gunman1OG,
        Gunman1TopixtorORC,
        Gunman1TopixtorCAC,
        FlamethrowerGoonOG,
        FlamethrowerGoonTopixtor,

        // These splits allow us to import misc animations into levels that do not have LaraMiscAnim_H
        // already defined, otherwise Lara tends to void when killed by these enemies and this sometimes crashes
        // the game. For levels that already have misc animations, the death animation will default to "normal".
        LaraMiscAnim_H_Ice = 2000, // Death-by-Yeti, Gong action
        LaraMiscAnim_H_Unwater,    // Death-by-Shark, opening wheel door
        LaraMiscAnim_H_Xian,       // Death-by-Guard, inspecting dagger
        LaraMiscAnim_H_Wall,       // Death-by-Barney
        LaraMiscAnim_H_HSH,        // Inspecting dagger at home
        LaraMiscAnim_H_Venice,     // Bartoli's detonator

        // Split Lara's outfits and weapon animations
        LaraSun = 3000,
        LaraPistolAnim_H_Sun,
        LaraAutoAnim_H_Sun,
        LaraUziAnim_H_Sun,

        LaraUnwater = 3100,
        LaraPistolAnim_H_Unwater,
        LaraAutoAnim_H_Unwater,
        LaraUziAnim_H_Unwater,

        LaraSnow = 3200,
        LaraPistolAnim_H_Snow,
        LaraAutoAnim_H_Snow,
        LaraUziAnim_H_Snow,

        LaraHome = 3300,
        LaraPistolAnim_H_Home,
        LaraAutoAnim_H_Home,
        LaraUziAnim_H_Home,

        LaraInvisible = 3400,

        // Puzzle and quest aliases
        Puzzle1_M_H_RelayBox = 3500,
        Puzzle2_M_H_CircuitBoard,

        Puzzle1_M_H_CircuitBoard,

        Puzzle1_M_H_CircuitBreaker,

        Puzzle4_M_H_Seraph,

        Puzzle1_M_H_PrayerWheel,
        Puzzle2_M_H_GemStone,

        Puzzle1_M_H_TibetanMask,
        Quest1_M_H_GongHammer,
        Quest2_M_H_GongHammer,

        Puzzle1_M_H_DragonSeal,

        Puzzle1_M_H_MysticPlaque,
        Puzzle2_M_H_MysticPlaque,

        Puzzle1_M_H_Dagger,
        Puzzle2_M_H_Dagger
    }
}