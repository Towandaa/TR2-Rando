﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TRRandomizerCore.Helpers;
using TRRandomizerCore.Utilities;
using TRRandomizerCore.Zones;
using TRGE.Core;
using TRLevelReader.Helpers;
using TRLevelReader.Model;
using TRLevelReader.Model.Enums;
using TRModelTransporter.Packing;
using TRModelTransporter.Transport;

namespace TRRandomizerCore.Randomizers
{
    public class TR2ItemRandomizer : BaseTR2Randomizer
    {
        private static readonly List<int> _devRooms = null;

        internal TexturePositionMonitorBroker TextureMonitor { get; set; }

        // This replaces plane cargo index as TRGE may have randomized the weaponless level(s), but will also have injected pistols
        // into predefined locations. See FindUnarmedPistolsLocation below.
        private int _unarmedLevelPistolIndex;
        private readonly Dictionary<string, List<Location>> _pistolLocations;

        public TR2ItemRandomizer()
        {
            _pistolLocations = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(ReadResource(@"TR2\Locations\unarmed_locations.json"));
        }

        public override void Randomize(int seed)
        {
            _generator = new Random(seed);

            Dictionary<string, List<Location>> locations = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(ReadResource(@"TR2\Locations\item_locations.json"));

            foreach (TR2ScriptedLevel lvl in Levels)
            {
                //Read the level into a combined data/script level object
                LoadLevelInstance(lvl);

                FindUnarmedPistolsLocation();

                //Apply the modifications
                RepositionItems(locations[_levelInstance.Name]);

                RandomizeVehicles();

                //Write back the level file
                SaveLevelInstance();

                if (!TriggerProgress())
                {
                    break;
                }
            }
        }

        // Called post enemy randomization if used to allow accurate enemy scoring
        public void RandomizeAmmo()
        {
            foreach (TR2ScriptedLevel lvl in Levels)
            {
                //Read the level into a combined data/script level object
                LoadLevelInstance(lvl);

                if (Settings.DevelopmentMode && _pistolLocations.ContainsKey(_levelInstance.Name))
                {
                    PlaceAllItems(_pistolLocations[_levelInstance.Name], TR2Entities.Pistols_S_P, false);
                }

                FindUnarmedPistolsLocation();

                //#44 - Randomize unarmed level weapon type
                if (lvl.RemovesWeapons) { RandomizeUnarmedLevelWeapon(); }

                //#47 - Randomize the HSH weapon closet
                if (lvl.Is(TR2LevelNames.HOME)) { PopulateHSHCloset(); }

                //Write back the level file
                SaveLevelInstance();

                if (!TriggerProgress())
                {
                    break;
                }
            }
        }

        private void PlaceAllItems(List<Location> locations, TR2Entities entityToAdd = TR2Entities.LargeMed_S_P, bool transformToLevelSpace = true)
        {
            List<TR2Entity> ents = _levelInstance.Data.Entities.ToList();

            foreach (Location loc in locations)
            {
                Location copy = transformToLevelSpace ? SpatialConverters.TransformToLevelSpace(loc, _levelInstance.Data.Rooms[loc.Room].Info) : loc;

                if (_devRooms == null || _devRooms.Contains(copy.Room))
                {
                    ents.Add(new TR2Entity
                    {
                        TypeID = (short)entityToAdd,
                        Room = Convert.ToInt16(copy.Room),
                        X = copy.X,
                        Y = copy.Y,
                        Z = copy.Z,
                        Angle = 0,
                        Intensity1 = -1,
                        Intensity2 = -1,
                        Flags = 0
                    });
                }
            }

            _levelInstance.Data.NumEntities = (uint)ents.Count;
            _levelInstance.Data.Entities = ents.ToArray();
        }

        private void RepositionItems(List<Location> ItemLocs)
        {
            if (Settings.DevelopmentMode)
            {
                PlaceAllItems(ItemLocs);
                return;
            }

            if (ItemLocs.Count > 0)
            {
                //We are currently looking guns + ammo
                List<TR2Entities> targetents = TR2EntityUtilities.GetListOfGunTypes();
                targetents.AddRange(TR2EntityUtilities.GetListOfAmmoTypes());

                //And also key items...
                if (Settings.IncludeKeyItems)
                {
                    targetents.AddRange(TR2EntityUtilities.GetListOfKeyItemTypes());
                }

                //It's important to now start zoning key items as softlocks must be avoided.
                ZonedLocationCollection ZonedLocations = new ZonedLocationCollection();
                ZonedLocations.PopulateZones(GetResourcePath(@"TR2\Zones\" + _levelInstance.Name + "-Zones.json"), ItemLocs, ZonePopulationMethod.KeyPuzzleQuestOnly);

                for (int i = 0; i < _levelInstance.Data.Entities.Count(); i++)
                {
                    if (targetents.Contains((TR2Entities)_levelInstance.Data.Entities[i].TypeID) && (i != _unarmedLevelPistolIndex))
                    {
                        Location RandomLocation = new Location();
                        bool FoundPossibleLocation = false;

                        if (TR2EntityUtilities.IsKeyItemType((TR2Entities)_levelInstance.Data.Entities[i].TypeID))
                        {
                            TR2Entities type = (TR2Entities)_levelInstance.Data.Entities[i].TypeID;

                            // Apply zoning for key items
                            switch (type)
                            {
                                case TR2Entities.Puzzle1_S_P:
                                    if (ZonedLocations.Puzzle1Zone.Count > 0)
                                    {
                                        if (_levelInstance.Name == TR2LevelNames.DA)
                                        {
                                            int burnerChipID = 120;
                                            int consoleChipID = 7;

                                            RandomLocation = ZonedLocations.Puzzle1Zone[_generator.Next(0, ZonedLocations.Puzzle1Zone.Count)];

                                            //Special case - multiple chips
                                            if (i == burnerChipID)
                                            {
                                                //Burner Chip
                                                List<int> AllowedBurnerRooms = new List<int>() { 13, 14, 15, 16, 21, 22, 23, 24, 25, 26, 29, 32, 75, 80, 83, 84, 85, 86, 87, 88, 89 };

                                                while (!AllowedBurnerRooms.Contains(RandomLocation.Room))
                                                {
                                                    RandomLocation = ZonedLocations.Puzzle1Zone[_generator.Next(0, ZonedLocations.Puzzle1Zone.Count)];
                                                }

                                                FoundPossibleLocation = true;
                                            }
                                            else if (i == consoleChipID)
                                            {
                                                //Center Console Chip
                                                List<int> AllowedConsoleRooms = new List<int>() { 2, 12, 13, 14, 15, 16, 17, 21, 22, 23, 24, 25, 26, 29, 30, 32, 34, 35, 64, 65, 66, 68, 69, 70, 75, 80, 82, 83, 84, 85, 86, 87, 88, 89 };

                                                while (!AllowedConsoleRooms.Contains(RandomLocation.Room))
                                                {
                                                    RandomLocation = ZonedLocations.Puzzle1Zone[_generator.Next(0, ZonedLocations.Puzzle1Zone.Count)];
                                                }

                                                FoundPossibleLocation = true;
                                            }
                                            else
                                            {
                                                RandomLocation = ZonedLocations.Puzzle1Zone[_generator.Next(0, ZonedLocations.Puzzle1Zone.Count)];
                                                FoundPossibleLocation = true;
                                            }
                                        }
                                        else
                                        {
                                            RandomLocation = ZonedLocations.Puzzle1Zone[_generator.Next(0, ZonedLocations.Puzzle1Zone.Count)];
                                            FoundPossibleLocation = true;
                                        }
                                    }
                                    break;
                                case TR2Entities.Puzzle2_S_P:
                                    if (ZonedLocations.Puzzle2Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Puzzle2Zone[_generator.Next(0, ZonedLocations.Puzzle2Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Puzzle3_S_P:
                                    if (ZonedLocations.Puzzle3Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Puzzle3Zone[_generator.Next(0, ZonedLocations.Puzzle3Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Puzzle4_S_P:
                                    if (ZonedLocations.Puzzle4Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Puzzle4Zone[_generator.Next(0, ZonedLocations.Puzzle4Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Key1_S_P:
                                    if (ZonedLocations.Key1Zone.Count > 0)
                                    {
                                        if (_levelInstance.Name == TR2LevelNames.OPERA)
                                        {
                                            int startKeyID = 172;
                                            int fanKeyID = 118;

                                            //Special case - multiple keys
                                            if (i == startKeyID)
                                            {
                                                //Start key
                                                List<int> AllowedStartRooms = new List<int>() { 10, 23, 25, 27, 29, 30, 31, 32, 33, 35, 127, 162, 163 };

                                                while (!AllowedStartRooms.Contains(RandomLocation.Room))
                                                {
                                                    RandomLocation = ZonedLocations.Key1Zone[_generator.Next(0, ZonedLocations.Key1Zone.Count)];
                                                }

                                                FoundPossibleLocation = true;
                                            }
                                            else if (i == fanKeyID)
                                            {
                                                //Fan area key
                                                List<int> AllowedFanRooms = new List<int>() { 1, 5, 8, 16, 37, 38, 44, 46, 47, 48, 49, 50, 52, 53, 55, 57, 59, 60, 63, 65, 66, 67, 68, 69, 70, 71, 72, 75, 76, 77, 78, 82, 83, 86, 87, 88, 89, 90, 93, 95, 96, 100, 102, 103, 105, 107, 109, 111, 120, 132, 139, 141, 143, 144, 151, 153, 154, 155, 156, 158, 159, 161, 174, 176, 177, 178, 179, 183, 185, 187, 188, 189 };

                                                while (!AllowedFanRooms.Contains(RandomLocation.Room))
                                                {
                                                    RandomLocation = ZonedLocations.Key1Zone[_generator.Next(0, ZonedLocations.Key1Zone.Count)];
                                                }

                                                FoundPossibleLocation = true;
                                            }
                                            else
                                            {
                                                RandomLocation = ZonedLocations.Key1Zone[_generator.Next(0, ZonedLocations.Key1Zone.Count)];
                                                FoundPossibleLocation = true;
                                            }
                                        }
                                        else
                                        {
                                            RandomLocation = ZonedLocations.Key1Zone[_generator.Next(0, ZonedLocations.Key1Zone.Count)];
                                            FoundPossibleLocation = true;
                                        }
                                    }
                                    break;
                                case TR2Entities.Key2_S_P:
                                    if (ZonedLocations.Key2Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Key2Zone[_generator.Next(0, ZonedLocations.Key2Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Key3_S_P:
                                    if (ZonedLocations.Key3Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Key3Zone[_generator.Next(0, ZonedLocations.Key3Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Key4_S_P:
                                    if (ZonedLocations.Key4Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Key4Zone[_generator.Next(0, ZonedLocations.Key4Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Quest1_S_P:
                                    if (ZonedLocations.Quest1Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Quest1Zone[_generator.Next(0, ZonedLocations.Quest1Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                case TR2Entities.Quest2_S_P:
                                    if (ZonedLocations.Quest2Zone.Count > 0)
                                    {
                                        RandomLocation = ZonedLocations.Quest2Zone[_generator.Next(0, ZonedLocations.Quest2Zone.Count)];
                                        FoundPossibleLocation = true;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            //Place standard items as normal for now
                            RandomLocation = ItemLocs[_generator.Next(0, ItemLocs.Count)];
                            FoundPossibleLocation = true;
                        }

                        if (FoundPossibleLocation)
                        {
                            Location GlobalizedRandomLocation = SpatialConverters.TransformToLevelSpace(RandomLocation, _levelInstance.Data.Rooms[RandomLocation.Room].Info);

                            _levelInstance.Data.Entities[i].Room = Convert.ToInt16(GlobalizedRandomLocation.Room);
                            _levelInstance.Data.Entities[i].X = GlobalizedRandomLocation.X;
                            _levelInstance.Data.Entities[i].Y = GlobalizedRandomLocation.Y;
                            _levelInstance.Data.Entities[i].Z = GlobalizedRandomLocation.Z;
                            _levelInstance.Data.Entities[i].Intensity1 = -1;
                            _levelInstance.Data.Entities[i].Intensity2 = -1;
                        }
                    }
                }

                if (Settings.RandoItemDifficulty == ItemDifficulty.OneLimit)
                {
                    List<TR2Entities> oneOfEachType = new List<TR2Entities>();
                    List<TR2Entity> allEntities = _levelInstance.Data.Entities.ToList();

                    // look for extra utility/ammo items and hide them
                    foreach (TR2Entity ent in allEntities)
                    {
                        TR2Entities eType = (TR2Entities)ent.TypeID;
                        if (TR2EntityUtilities.IsUtilityType(eType) ||
                            TR2EntityUtilities.IsGunType(eType))
                        {
                            if (oneOfEachType.Contains(eType))
                            {
                                ItemUtilities.HideEntity(ent);
                            }
                            else
                                oneOfEachType.Add((TR2Entities)ent.TypeID);
                        }
                    }
                }
            }
        }

        private void FindUnarmedPistolsLocation()
        {
            // #66 - checks were previously performed to clean locations from previous
            // randomization sessions to avoid item pollution. This is no longer required
            // as randomization is now always performed on the original level files.

            // #124 Default pistol locations are no longer limited to one per level.

            _unarmedLevelPistolIndex = -1;

            if (_levelInstance.Script.RemovesWeapons && _pistolLocations.ContainsKey(_levelInstance.Name))
            {
                short pistolID = (short)TR2Entities.Pistols_S_P;
                int pistolIndex = _levelInstance.Data.Entities.ToList().FindIndex(e => e.TypeID == pistolID);
                if (pistolIndex != -1)
                {
                    // Sanity check that the location is one that we expect
                    TR2Entity pistols = _levelInstance.Data.Entities[pistolIndex];
                    Location pistolLocation = new Location
                    {
                        X = pistols.X,
                        Y = pistols.Y,
                        Z = pistols.Z,
                        Room = pistols.Room
                    };

                    int match = _pistolLocations[_levelInstance.Name].FindIndex
                    (
                        location =>
                            location.X == pistolLocation.X &&
                            location.Y == pistolLocation.Y &&
                            location.Z == pistolLocation.Z &&
                            location.Room == pistolLocation.Room
                    );

                    if (match != -1)
                    {
                        _unarmedLevelPistolIndex = pistolIndex;
                    }
                }
            }
        }

        private readonly Dictionary<TR2Entities, uint> _startingAmmoToGive = new Dictionary<TR2Entities, uint>()
        {
            {TR2Entities.Shotgun_S_P, 8},
            {TR2Entities.Automags_S_P, 4},
            {TR2Entities.Uzi_S_P, 4},
            {TR2Entities.Harpoon_S_P, 4}, // #149 Agreed that a low number of harpoons will be given for unarmed levels, but pistols will also be included
            {TR2Entities.M16_S_P, 2},
            {TR2Entities.GrenadeLauncher_S_P, 4},
        };

        private void RandomizeUnarmedLevelWeapon()
        {
            //Is there something in the unarmed level pistol location?
            if (_unarmedLevelPistolIndex != -1)
            {
                List<TR2Entities> replacementWeapons = TR2EntityUtilities.GetListOfGunTypes();
                replacementWeapons.Add(TR2Entities.Pistols_S_P);
                TR2Entities weaponType = replacementWeapons[_generator.Next(0, replacementWeapons.Count)];

                // force pistols for OneLimit and then we're done
                if (Settings.RandoItemDifficulty == ItemDifficulty.OneLimit)
                {
                    weaponType = replacementWeapons[replacementWeapons.Count - 1];
                    return;
                }

                if (_levelInstance.Is(TR2LevelNames.CHICKEN))
                {
                    // Grenade Launcher and Harpoon cannot trigger the bells in Ice Palace
                    while (weaponType.Equals(TR2Entities.GrenadeLauncher_S_P) || weaponType.Equals(TR2Entities.Harpoon_S_P))
                    {
                        weaponType = replacementWeapons[_generator.Next(0, replacementWeapons.Count)];
                    }
                }

                uint ammoToGive = 0;
                bool addPistols = false;
                uint smallMediToGive = 0;
                uint largeMediToGive = 0;

                if (_startingAmmoToGive.ContainsKey(weaponType))
                {
                    ammoToGive = _startingAmmoToGive[weaponType];
                    if (Settings.RandomizeEnemies && Settings.CrossLevelEnemies)
                    {
                        // Create a score based on each type of enemy in this level and increase the ammo count based on this
                        EnemyDifficulty difficulty = EnemyUtilities.GetEnemyDifficulty(_levelInstance.GetEnemyEntities());
                        ammoToGive *= (uint)difficulty;

                        // Depending on how difficult the enemy combination is, allocate some extra helpers.
                        addPistols = difficulty > EnemyDifficulty.Easy;

                        if (difficulty == EnemyDifficulty.Medium || difficulty == EnemyDifficulty.Hard)
                        {
                            smallMediToGive++;
                        }
                        if (difficulty > EnemyDifficulty.Medium)
                        {
                            largeMediToGive++;
                        }
                        if (difficulty == EnemyDifficulty.VeryHard)
                        {
                            largeMediToGive++;
                        }
                    }
                    else if (_levelInstance.Is(TR2LevelNames.LAIR))
                    {
                        ammoToGive *= 6;
                    }
                }

                TR2Entity unarmedLevelWeapons = _levelInstance.Data.Entities[_unarmedLevelPistolIndex];
                unarmedLevelWeapons.TypeID = (short)weaponType;

                if (weaponType != TR2Entities.Pistols_S_P)
                {
                    //#68 - Provide some additional ammo for a weapon if not pistols
                    AddUnarmedLevelAmmo(GetWeaponAmmo(weaponType), ammoToGive);

                    // If we haven't decided to add the pistols (i.e. for enemy difficulty)
                    // add a 1/3 chance of getting them anyway. #149 If the harpoon is being
                    // given, the pistols will be included.
                    if (addPistols || weaponType == TR2Entities.Harpoon_S_P || _generator.Next(0, 3) == 0)
                    {
                        CopyEntity(unarmedLevelWeapons, TR2Entities.Pistols_S_P);
                    }
                }

                for (int i = 0; i < smallMediToGive; i++)
                {
                    CopyEntity(unarmedLevelWeapons, TR2Entities.SmallMed_S_P);
                }
                for (int i = 0; i < largeMediToGive; i++)
                {
                    CopyEntity(unarmedLevelWeapons, TR2Entities.LargeMed_S_P);
                }
            }
        }

        private void CopyEntity(TR2Entity entity, TR2Entities newType)
        {
            List<TR2Entity> ents = _levelInstance.Data.Entities.ToList();
            if (ents.Count < _levelInstance.GetMaximumEntityLimit())
            {
                TR2Entity copy = entity.Clone();
                copy.TypeID = (short)newType;
                ents.Add(copy);
                _levelInstance.Data.NumEntities++;
                _levelInstance.Data.Entities = ents.ToArray();
            }
        }

        private TR2Entities GetWeaponAmmo(TR2Entities weapon)
        {
            switch (weapon)
            {
                case TR2Entities.Shotgun_S_P:
                    return TR2Entities.ShotgunAmmo_S_P;
                case TR2Entities.Automags_S_P:
                    return TR2Entities.AutoAmmo_S_P;
                case TR2Entities.Uzi_S_P:
                    return TR2Entities.UziAmmo_S_P;
                case TR2Entities.Harpoon_S_P:
                    return TR2Entities.HarpoonAmmo_S_P;
                case TR2Entities.M16_S_P:
                    return TR2Entities.M16Ammo_S_P;
                case TR2Entities.GrenadeLauncher_S_P:
                    return TR2Entities.Grenades_S_P;
                default:
                    return TR2Entities.PistolAmmo_S_P;
            }
        }

        private void AddUnarmedLevelAmmo(TR2Entities ammoType, uint count)
        {
            // #216 - Avoid bloating the entity list by creating additional pickups
            // and instead add the extra ammo directly to the inventory.
            _levelInstance.Script.AddStartInventoryItem(ItemUtilities.ConvertToScriptItem(ammoType), count);
        }

        private void PopulateHSHCloset()
        {
            List<TR2Entities> replacementWeapons = TR2EntityUtilities.GetListOfGunTypes();
            if (_levelInstance.Script.RemovesWeapons)
            {
                replacementWeapons.Add(TR2Entities.Pistols_S_P);
            }

            // Pick a new weapon, but exclude the grenade launcher because it affects the kill count
            TR2Entities replacementWeapon;
            do
            {
                replacementWeapon = replacementWeapons[_generator.Next(0, replacementWeapons.Count)];
            }
            while (replacementWeapon == TR2Entities.GrenadeLauncher_S_P);

            TR2Entities replacementAmmo = GetWeaponAmmo(replacementWeapon);

            List<TR2Entity> ents = _levelInstance.Data.Entities.ToList();
            TR2Entity harpoonWeapon = null;
            List<TR2Entities> oneOfEachType = new List<TR2Entities>();
            foreach (TR2Entity entity in ents)
            {
                if (entity.Room != 57)
                {
                    continue;
                }

                TR2Entities entityType = (TR2Entities)entity.TypeID;
                if (TR2EntityUtilities.IsGunType(entityType))
                {
                    entity.TypeID = (short)replacementWeapon;

                    if (replacementWeapon == TR2Entities.Harpoon_S_P || (Settings.RandoItemDifficulty == ItemDifficulty.OneLimit && replacementWeapon != TR2Entities.Pistols_S_P))
                    {
                        harpoonWeapon = entity;
                    }
                }
                else if (TR2EntityUtilities.IsAmmoType(entityType) && replacementWeapon != TR2Entities.Pistols_S_P)
                {
                    entity.TypeID = (short)replacementAmmo;
                }

                if (Settings.RandoItemDifficulty == ItemDifficulty.OneLimit)
                {
                    // look for extra utility/ammo items and hide them
                    TR2Entities eType = (TR2Entities)entity.TypeID;
                    if (TR2EntityUtilities.IsUtilityType(eType) ||
                        TR2EntityUtilities.IsGunType(eType))
                    {
                        if (oneOfEachType.Contains(eType))
                        {
                            ItemUtilities.HideEntity(entity);
                        }
                        else
                            oneOfEachType.Add((TR2Entities)entity.TypeID);
                    }
                }
            }

            // if weapon is harpoon OR difficulty is OneLimit, spawn pistols as well (see #149)
            if (harpoonWeapon != null)
                CopyEntity(harpoonWeapon, TR2Entities.Pistols_S_P);
        }

        private void RandomizeVehicles()
        {
            // For now, we only add the boat if it has a location defined for a level. The skidoo is added
            // to levels that have MercSnowMobDriver present (see EnemyRandomizer) but we could alter this
            // to include it potentially in any level.
            // This perhaps needs better tracking, for example if every level has a vehicle location defined
            // we might not necessarily want to include it in every level.
            Dictionary<TR2Entities, Location> vehicles = new Dictionary<TR2Entities, Location>();
            PopulateVehicleLocation(TR2Entities.Boat, vehicles);
            if (_levelInstance.IsAssault)
            {
                // The assault course doesn't have enemies i.e. MercSnowMobDriver, so just add the skidoo too
                PopulateVehicleLocation(TR2Entities.RedSnowmobile, vehicles);
            }

            List<TR2Entity> levelEntities = _levelInstance.Data.Entities.ToList();
            int entityLimit = _levelInstance.GetMaximumEntityLimit();
            if (vehicles.Count == 0 || vehicles.Count + levelEntities.Count > entityLimit)
            {
                return;
            }

            TR2ModelImporter importer = new TR2ModelImporter
            {
                Level = _levelInstance.Data,
                LevelName = _levelInstance.Name,
                ClearUnusedSprites = false,
                EntitiesToImport = vehicles.Keys,
                DataFolder = GetResourcePath(@"TR2\Models"),
                TexturePositionMonitor = TextureMonitor.CreateMonitor(_levelInstance.Name, vehicles.Keys.ToList())
            };

            try
            {
                importer.Import();

                foreach (TR2Entities entity in vehicles.Keys)
                {
                    if (levelEntities.Count == entityLimit)
                    {
                        break;
                    }

                    Location location = vehicles[entity];
                    levelEntities.Add(new TR2Entity
                    {
                        TypeID = (short)entity,
                        Room = (short)location.Room,
                        X = location.X,
                        Y = location.Y,
                        Z = location.Z,
                        Angle = location.Angle,
                        Flags = 0,
                        Intensity1 = -1,
                        Intensity2 = -1
                    });
                }

                if (levelEntities.Count > _levelInstance.Data.NumEntities)
                {
                    _levelInstance.Data.Entities = levelEntities.ToArray();
                    _levelInstance.Data.NumEntities = (uint)levelEntities.Count;
                }
            }
            catch (PackingException)
            {
                // Silently ignore failed imports for now as these are nice-to-have only
            }
        }

        private void PopulateVehicleLocation(TR2Entities entity, Dictionary<TR2Entities, Location> locationMap)
        {
            Location location = VehicleUtilities.GetRandomLocation(_levelInstance.Name, entity, _generator);
            if (location != null)
            {
                locationMap[entity] = location;
            }
        }
    }
}