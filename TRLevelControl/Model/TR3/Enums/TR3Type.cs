﻿namespace TRLevelControl.Model;

//_S are sprites - BOLD
//_M are menu items - ITALIC

//_P are pickup items - BLUE
//_N are nullmeshes (no render/collision) - RED
//_H are helper entities (not placed) - GREEN
//_U are unused entities - PURPLE
public enum TR3Type
{
    Lara                       = 0,
    LaraPistolAnimation_H      = 1,
    LaraPonytail_H             = 2,
    LaraShotgunAnimation_H     = 3,
    LaraDeagleAnimation_H      = 4,
    LaraUziAnimation_H         = 5,
    LaraMP5Animation_H         = 6,
    LaraRocketAnimation_H      = 7,
    LaraGrenadeAnimation_H     = 8,
    LaraHarpoonAnimation_H     = 9,
    LaraFlareAnimation_H       = 10,
    LaraVehicleAnimation_H     = 11,
    VehicleExtra_H             = 12,
    LaraExtraAnimation_H       = 13,
    Kayak                      = 14,
    Boat                       = 15,
    Quad                       = 16,
    MineCart                   = 17,
    BigGun                     = 18,
    UPV                        = 19,
    TribesmanAxe               = 20,
    TribesmanDart              = 21,
    Dog                        = 22,
    Rat                        = 23,
    KillAllTriggers_N          = 24,
    KillerWhale                = 25,
    ScubaSteve                 = 26,
    Crow                       = 27,
    Tiger                      = 28,
    Vulture                    = 29,
    AssaultCourseTarget        = 30,
    CrawlerMutantInCloset      = 31,
    Croc                       = 32,
    Butterfly                  = 33,
    Compsognathus              = 34,
    LizardMan                  = 35,
    Puna                       = 36,
    Mercenary                  = 37,
    HangingRaptor              = 38,
    RXRedBoi                   = 39,
    RXGunLad                   = 40,
    DogAntarc                  = 41,
    Crawler                    = 42,
    Willard                    = 43,
    TinnosWasp                 = 44,
    TinnosMonster              = 45,
    BruteMutant                = 46,
    TinnosWaspRespawnPoint_N   = 47,
    RaptorRespawnPoint_N       = 48,
    Willie                     = 49,
    RXTechFlameLad             = 50,
    LondonMerc                 = 51,
    LonMercenary2              = 52,
    Punk                       = 53,
    Punk2                      = 54,
    WaterBloke                 = 55,
    LondonGuard                = 56,
    SophiaLee                  = 57,
    CleanerRobot               = 58,
    FloatingCorpse             = 59,
    MPWithStick                = 60,
    MPWithGun                  = 61,
    Prisoner                   = 62,
    MPWithMP5                  = 63,
    Turret                     = 64,
    DamGuard                   = 65,
    Tripwire_N                 = 66,
    ElectrifiedWire_N          = 67,
    KillerTripwire_N           = 68,
    Cobra                      = 69,
    Shiva                      = 70,
    Monkey                     = 71,
    BearTrap                   = 72,
    TonyFirehands              = 73,
    AIGuard_N                  = 74,
    AIAmbush_N                 = 75,
    AIPatrol1_N                = 76,
    AIModify_N                 = 77,
    AIFollow_N                 = 78,
    AIPatrol2_N                = 79,
    AIPath_N                   = 80,
    AICheck_N                  = 81,
    AIUnknown_N                = 82,
    FallingBlock               = 83,
    FallingBlock2              = 84,
    FallingPlank               = 85,
    SwingingThing              = 86,
    TeethSpikesOrBarbedWire    = 87,
    RollingBallOrBarrel        = 88,
    GiantBoulder               = 89,
    Disc_H                     = 90,
    DartShooter                = 91,
    HomingDartEmitter          = 92,
    DrawBridge                 = 93,
    SkeletonTrapOrSlammingDoor = 94,
    Lift                       = 95,
    MovingBar                  = 96,
    PushableBlock1             = 97,
    PushableBlock2             = 98,
    MovableBlock3              = 99,
    MovableBlock4              = 100,
    DestroyableBoardedUpWindow = 101,
    DestroyableBoardedUpWall   = 102,
    SmashObject2               = 103,
    SmashObject3               = 104,
    Saw                        = 105,
    Area51Swinger              = 106,
    FallingCeiling             = 107,
    RollingSpindle             = 108,
    CircularBlade              = 109,
    SubwayTrain                = 110,
    WallKnifeBladeKnifeDisk    = 111,
    Avalanche                  = 112,
    DetachableStalactites      = 113,
    SpikyWall                  = 114,
    SpringBoard                = 115,
    SpikyVertWallOrTunnelBorer = 116,
    ValveWheelOrPulley         = 117,
    SmallWallSwitch            = 118,
    DamagingAnimating1         = 119,
    DamagingAnimating2         = 120,
    DamagingAnimating3         = 121,
    ShivaStatue                = 122,
    MonkeyMedMeshswap          = 123,
    MonkeyKeyMeshswap          = 124,
    UIFrame_S_H                = 125,
    Oildrums                   = 126,
    ZiplineHandle              = 127,
    PushButtonSwitch           = 128,
    WallSwitch                 = 129,
    UnderwaterSwitch           = 130,
    Door1                      = 131,
    Door2                      = 132,
    Door3                      = 133,
    Door4                      = 134,
    Door5                      = 135,
    Door6                      = 136,
    Door7                      = 137,
    Door8                      = 138,
    Trapdoor1                  = 139,
    Trapdoor2                  = 140,
    Trapdoor3                  = 141,
    BridgeFlat                 = 142,
    BridgeTilt1                = 143,
    BridgeTilt2                = 144,
    PassportOpening_H          = 145,
    Stopwatch_H                = 146,
    LaraHomePhoto_H            = 147,
    CutsceneActor1             = 148,
    CutsceneActor2             = 149,
    CutsceneActor3             = 150,
    CutsceneActor4             = 151,
    CutsceneActor5             = 152,
    CutsceneActor6             = 153,
    CutsceneActor7             = 154,
    CutsceneActor8             = 155,
    CutsceneActor9             = 156,
    Player10                   = 157,
    PassportClosed_H           = 158,
    Map_H                      = 159,
    Pistols_P                  = 160,
    Shotgun_P                  = 161,
    Deagle_P                   = 162,
    Uzis_P                     = 163,
    Harpoon_P                  = 164,
    MP5_P                      = 165,
    RocketLauncher_P           = 166,
    GrenadeLauncher_P          = 167,
    PistolAmmo_P               = 168,
    ShotgunAmmo_P              = 169,
    DeagleAmmo_P               = 170,
    UziAmmo_P                  = 171,
    Harpoons_P                 = 172,
    MP5Ammo_P                  = 173,
    Rockets_P                  = 174,
    Grenades_P                 = 175,
    SmallMed_P                 = 176,
    LargeMed_P                 = 177,
    Flares_P                   = 178,
    Flare_H                    = 179,
    SaveCrystal_P              = 180,
    Sunglasses_M_H             = 181,
    CDPlayer_M_H               = 182,
    DirectionKeys_M_H          = 183,
    Globe_M_H                  = 184,
    Pistols_M_H                = 185,
    Shotgun_M_H                = 186,
    Deagle_M_H                 = 187,
    Uzis_M_H                   = 188,
    Harpoon_M_H                = 189,
    MP5_M_H                    = 190,
    RocketLauncher_M_H         = 191,
    GrenadeLauncher_M_H        = 192,
    PistolAmmo_M_H             = 193,
    ShotgunAmmo_M_H            = 194,
    DeagleAmmo_M_H             = 195,
    UziAmmo_M_H                = 196,
    Harpoons_M_H               = 197,
    MP5Ammo_M_H                = 198,
    Rockets_M_H                = 199,
    Grenades_M_H               = 200,
    SmallMed_M_H               = 201,
    LargeMed_M_H               = 202,
    Flares_M_H                 = 203,
    SaveCrystal_M_H            = 204,
    Puzzle1_P                  = 205,
    Puzzle2_P                  = 206,
    Puzzle3_P                  = 207,
    Puzzle4_P                  = 208,
    Puzzle1_M_H                = 209,
    Puzzle2_M_H                = 210,
    Puzzle3_M_H                = 211,
    Puzzle4_M_H                = 212,
    Slot1Empty                 = 213,
    Slot2Empty                 = 214,
    Slot3Empty                 = 215,
    Slot4Empty                 = 216,
    Slot1Full                  = 217,
    Slot2Full                  = 218,
    Slot3Full                  = 219,
    Slot4Full                  = 220,
    SecretItem1                = 221,
    SecretItem2                = 222,
    SecretItem3                = 223,
    Key1_P                     = 224,
    Key2_P                     = 225,
    Key3_P                     = 226,
    Key4_P                     = 227,
    Key1_M_H                   = 228,
    Key2_M_H                   = 229,
    Key3_M_H                   = 230,
    Key4_M_H                   = 231,
    Keyhole1                   = 232,
    Keyhole2                   = 233,
    Keyhole3                   = 234,
    Keyhole4                   = 235,
    Quest1_P                   = 236,
    Quest2_P                   = 237,
    Quest1_M_H                 = 238,
    Quest2_M_H                 = 239,
    Infada_P                   = 240,
    Element115_P               = 241,
    EyeOfIsis_P                = 242,
    OraDagger_P                = 243,
    Infada_M_H                 = 244,
    Element115_M_H             = 245,
    EyeOfIsis_M_H              = 246,
    OraDagger_M_H              = 247,
    DisplayPistols_U           = 248,
    DisplayShotgun_U           = 249,
    DisplayDeagle_U            = 250,
    DisplayUzis_U              = 251,
    DisplayHarpoon_U           = 252,
    DisplayMP5_U               = 253,
    DisplayRocketLauncher_U    = 254,
    DisplayGrenadeLauncher_U   = 255,
    DisplayPistolAmmo_U        = 256,
    DisplayShotgunAmmo_U       = 257,
    DisplayDeagleAmmo_U        = 258,
    DisplayUziAmmo_U           = 259,
    DisplayHarpoonAmmo_U       = 260,
    DisplayMP5Ammo_U           = 261,
    DisplayRockets_U           = 262,
    DisplayGrenades_U          = 263,
    DisplaySmallMed_U          = 264,
    DisplayLargeMed_U          = 265,
    DisplayFlares_U            = 266,
    DisplayCrystal_U           = 267,
    DisplayPuzzle1_U           = 268,
    DisplayPuzzle2_U           = 269,
    DisplayPuzzle3_U           = 270,
    DisplayPuzzle4_U           = 271,
    KeysSprite1                = 272,
    KeysSprite2                = 273,
    DisplayKey3_U              = 274,
    DisplayKey4_U              = 275,
    Infada2_P                  = 276,
    Element1152_P              = 277,
    EyeOfIsis2_P               = 278,
    OraDagger2_P               = 279,
    DisplayPickup1_U           = 280,
    DisplayPickup2_U           = 281,
    FireBreathingDragonStatue  = 282,
    Tonyfireball               = 283,
    SphereOfDoom3              = 284,
    AlarmSound_N               = 285,
    DrippingWater_N            = 286,
    Tyrannosaur                = 287,
    Raptor                     = 288,
    BirdTweeter                = 289,
    ClockChimes                = 290,
    LaserSweeper               = 291,
    ElectricalField            = 292,
    HotLiquid                  = 293,
    ShadowSprite_S_H           = 294,
    DetonatorSwitchBox         = 295,
    MiscSprites_S_H            = 296,
    Bubble_S_H                 = 297,
    Bubbles2                   = 298,
    Glow_S_H                   = 299,
    Gunflare_H                 = 300,
    GunflareMP5_H              = 301,
    DesertEagleFlash           = 302,
    BodyPart                   = 303,
    LookAtItem_H               = 304,
    WaterfallMist_H            = 305,
    HarpoonSingle              = 306,
    DragonFire                 = 307,
    Knife                      = 308,
    RocketSingle               = 309,
    HarpoonSingle2             = 310,
    GrenadeSingle              = 311,
    BigMissile                 = 312,
    Smoke_H                    = 313,
    MovableBoom                = 314,
    LaraSkin_H                 = 315,
    Glow2_S_H                  = 316,
    LavaEmitter_N              = 317,
    AlarmLight                 = 318,
    Light_N                    = 319,
    OnOffLight                 = 320,
    Light2_N                   = 321,
    PulsatingLight_N           = 322,
    ExtraLight2                = 323,
    RedLight_N                 = 324,
    GreenLight_N               = 325,
    BlueLight_N                = 326,
    Light3_N                   = 327,
    Light4_N                   = 328,
    Flame                      = 329,
    Fire_N                     = 330,
    AltFire_N                  = 331,
    AltFire2_N                 = 332,
    Fire2_N                    = 333,
    Smoke2_N                   = 334,
    Smoke3_N                   = 335,
    Smoke4_N                   = 336,
    GreenishSmoke_N            = 337,
    Piranhas_N                 = 338,
    Fish                       = 339,
    PirahnaGfx                 = 340,
    TropicalFishGfx            = 341,
    BatGfx                     = 342,
    TribebossGfx               = 343,
    SpiderGfx                  = 344,
    Tumbleweed                 = 345,
    Leaves                     = 346,
    BatSwarm_N                 = 347,
    BirdEmitter                = 348,
    Animating1                 = 349,
    Animating2                 = 350,
    Animating3                 = 351,
    Animating4                 = 352,
    Animating5                 = 353,
    Animating6                 = 354,
    Skybox_H                   = 355,
    FontGraphics_S_H           = 356,
    Doorbell_N                 = 357,
    AlarmBell_N                = 358,
    MiniCopter                 = 359,
    Winston                    = 360,
    WinstonInCamoSuit          = 361,
    TimerFontGraphics_S_H      = 362,
    FinalLevel                 = 363,
    CutShotgun                 = 364,
    EarthQuake_N               = 365,
    YellowShellCasing_H        = 366,
    RedShellCasing_H           = 367,
    ExtraFX1                   = 368,
    ExtraFX2                   = 369,
    TinnosLightShaft           = 370,
    ExtraFX4                   = 371,
    ExtraFX5                   = 372,
    ElectricalSwitchBox        = 373,

    // Scenery
    Plant0                     = 374,
    Plant1                     = 375,
    Plant2                     = 376,
    Plant3                     = 377,
    Plant4                     = 378,
    Plant5                     = 379,
    Plant6                     = 380,
    Plant7                     = 381,
    Plant8                     = 382,
    Plant9                     = 383,
    Furniture0                 = 384,
    Furniture1                 = 385,
    Furniture2                 = 386,
    Furniture3                 = 387,
    Furniture4                 = 388,
    Furniture5                 = 389,
    Furniture6                 = 390,
    Furniture7                 = 391,
    Furniture8                 = 392,
    Furniture9                 = 393,
    Rock0                      = 394,
    Rock1                      = 395,
    Rock2                      = 396,
    Rock3                      = 397,
    Rock4                      = 398,
    Rock5                      = 399,
    Rock6                      = 400,
    Rock7                      = 401,
    Rock8                      = 402,
    Rock9                      = 403,
    Architecture0              = 404,
    Architecture1              = 405,
    Architecture2              = 406,
    Architecture3              = 407,
    Architecture4              = 408,
    Architecture5              = 409,
    Architecture6              = 410,
    Architecture7              = 411,
    Architecture8              = 412,
    Architecture9              = 413,
    Debris0                    = 414,
    Debris1                    = 415,
    Debris2                    = 416,
    Debris3                    = 417,
    Debris4                    = 418,
    Debris5                    = 419,
    Debris6                    = 420,
    Debris7                    = 421,
    Debris8                    = 422,
    Debris9                    = 423,
    SceneryBase                = Plant0,

    // Alias entries
    CobraIndia                    = 1000,
    CobraNevada                   = 1001,

    DogLondon                     = 2000,
    DogNevada                     = 2001,

    // Lara skins and gun anims
    LaraIndia                     = 3000,
    LaraSkin_H_India              = 3001,
    LaraPistolAnimation_H_India   = 3002,
    LaraDeagleAnimation_H_India   = 3003,
    LaraUziAnimation_H_India      = 3004,

    LaraCoastal                   = 4000,
    LaraSkin_H_Coastal            = 4001,
    LaraPistolAnimation_H_Coastal = 4002,
    LaraDeagleAnimation_H_Coastal = 4003,
    LaraUziAnimation_H_Coastal    = 4004,

    LaraLondon                    = 5000,
    LaraSkin_H_London             = 5001,
    LaraPistolAnimation_H_London  = 5002,
    LaraDeagleAnimation_H_London  = 5003,
    LaraUziAnimation_H_London     = 5004,

    LaraNevada                    = 6000,
    LaraSkin_H_Nevada             = 6001,
    LaraPistolAnimation_H_Nevada  = 6002,
    LaraDeagleAnimation_H_Nevada  = 6003,
    LaraUziAnimation_H_Nevada     = 6004,

    LaraAntarc                    = 7000,
    LaraSkin_H_Antarc             = 7001,
    LaraPistolAnimation_H_Antarc  = 7002,
    LaraDeagleAnimation_H_Antarc  = 7003,
    LaraUziAnimation_H_Antarc     = 7004,

    LaraInvisible                 = 7500,

    LaraHome                      = 7600,
    LaraSkin_H_Home               = 7601,
    LaraPistolAnimation_H_Home    = 7602,
    LaraDeagleAnimation_H_Home    = 7603,
    LaraUziAnimation_H_Home       = 7604,

    // Lara + Vehicle anims
    LaraVehicleAnimation_H_Quad   = 8000,
    LaraVehicleAnimation_H_BigGun = 8001,
    LaraVehicleAnimation_H_Kayak  = 8002,
    LaraVehicleAnimation_H_UPV    = 8003,
    LaraVehicleAnimation_H_Boat   = 8004,

    // Key item aliases
    JungleKeyItemBase             = 10000,
    Jungle_K4_IndraKey            = 10590,

    TempleKeyItemBase             = 11000,
    Temple_K1_GaneshaCurrentPool  = 11723,
    Temple_K1_GaneshaFlipmapPool  = 11641,
    Temple_K1_GaneshaMudslide     = 11539,
    Temple_K1_GaneshaRandyRory    = 11708,
    Temple_K1_GaneshaSpikeCeiling = 11692,
    Temple_P1_ScimitarEast        = 11636,
    Temple_P2_ScimitarWest        = 11648,

    GangesKeyItemBase             = 12000,
    Ganges_K1_GateKeyMonkeyPit    = 12425,
    Ganges_K1_GateKeySnakePit     = 12258,

    KaliyaKeyItemBase             = 13000,

    CoastalKeyItemBase            = 17000,
    Coastal_K1_SmugglersKey       = 14300,
    Coastal_P1_StoneAbovePool     = 14447,
    Coastal_P1_StoneTreetops      = 14440,
    Coastal_P1_StoneWaterfall     = 14252,

    CrashKeyItemBase              = 18000,
    Crash_K1_BishopsKey           = 15364,
    Crash_K2_TuckermansKey        = 15278,

    MadubuKeyItemBase             = 19000,

    PunaKeyItemBase               = 20000,

    ThamesKeyItemBase             = 21000,
    Thames_K1_FlueRoomKey         = 18524,
    Thames_K2_CathedralKey        = 18161,

    AldwychKeyItemBase            = 22000,
    Aldwych_K1_MaintenanceKey     = 19279,
    Aldwych_K2_SolomonKey3Doors   = 19362,
    Aldwych_K3_SolomonKeyDrill    = 19239,
    Aldwych_P1_OldCoin            = 19343,
    Aldwych_P2_Ticket             = 19302,
    Aldwych_P3_Hammer             = 19465,
    Aldwych_P4_OrnateStar         = 19448,

    LudsKeyItemBase               = 23000,
    Luds_K1_BoilerRoomKey         = 20447,
    Luds_P1_EmbalmingFluid        = 20240,

    CityKeyItemBase               = 24000,

    NevadaKeyItemBase             = 14000,
    Nevada_K1_GeneratorAccessCard = 22578,
    Nevada_K2_DetonatorKey        = 22359,
    Nevada_K2_DetonatorKeyUnused  = 22419,

    HSCKeyItemBase                = 15000,
    HSC_K1_KeycardTypeA           = 23260,
    HSC_K2_KeycardTypeBSatellite  = 23490,
    HSC_K2_KeycardTypeBTurrets    = 23454,
    HSC_P1_BluePass               = 23352,
    HSC_P2_YellowPassEnd          = 23322,
    HSC_P2_YellowPassHangar       = 23537,
    HSC_P2_YellowPassSatellite    = 23463,

    Area51KeyItemBase             = 16000,
    Area51_K1_LaunchCodeCard      = 24433,
    Area51_P2_CodeCDSilo          = 24322,
    Area51_P3_CodeCDWatchTower    = 24253,
    Area51_P4_HangarAccessPass    = 24434,

    AntarcticaKeyItemBase         = 25000,
    Antarc_K1_HutKey              = 25526,
    Antarc_P1_CrowbarGateControl  = 25374,
    Antarc_P1_CrowbarRegular      = 25456,
    Antarc_P1_CrowbarTower        = 25371,
    Antarc_P2_GateControlKey      = 25446,

    RXKeyItemBase                 = 26000,
    RX_P1_Crowbar                 = 26679,
    RX_P2_LeadAcidBattery         = 26444,
    RX_P3_WinchStarter            = 26509,

    TinnosKeyItemBase             = 27000,
    Tinnos_K1_UliKeyEnd           = 27686,
    Tinnos_K1_UliKeyStart         = 27269,
    Tinnos_P1_OceanicMaskEarth    = 27421,
    Tinnos_P1_OceanicMaskFire     = 27449,
    Tinnos_P1_OceanicMaskWater    = 27395,
    Tinnos_P1_OceanicMaskWind     = 27368,

    CavernKeyItemBase             = 28000,

    HallowsKeyItemBase            = 29000,
    Hallows_K1_VaultKey           = 29220,
}
