﻿using Newtonsoft.Json;
using TRFDControl;
using TRFDControl.Utilities;
using TRLevelControl;
using TRLevelControl.Helpers;
using TRLevelControl.Model;
using TRRandomizerCore.Helpers;
using TRRandomizerCore.Utilities;

namespace LocationExport;

class Program
{
    private static TR1LevelControl _reader1;
    private static TR2LevelControl _reader2;
    private static TR3LevelControl _reader3;
    private static Dictionary<string, List<Location>> _allTR1Exclusions;
    private static Dictionary<string, List<Location>> _allTR2Exclusions;
    private static Dictionary<string, List<Location>> _allTR3Exclusions;

    static void Main(string[] args)
    {
        if (args.Length == 0 || args[0].Contains('?'))
        {
            Usage();
            return;
        }

        _reader1 = new();
        _reader2 = new();
        _reader3 = new();
        _allTR1Exclusions = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(File.ReadAllText(@"Resources\TR1\Locations\invalid_item_locations.json"));
        _allTR2Exclusions = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(File.ReadAllText(@"Resources\TR2\Locations\invalid_item_locations.json"));
        _allTR3Exclusions = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(File.ReadAllText(@"Resources\TR3\Locations\invalid_item_locations.json"));
        Dictionary<string, List<Location>> allLocations = new();

        string levelType = args[0].ToUpper();

        if (levelType.EndsWith(".PHD"))
        {
            allLocations[levelType] = ExportTR1Locations(levelType);
        }
        else if (levelType == "TR1")
        {
            foreach (string lvl in TR1LevelNames.AsList)
            {
                if (File.Exists(lvl))
                {
                    allLocations[lvl] = ExportTR1Locations(lvl);
                }
            }
        }
        else if (levelType.EndsWith(".TR2"))
        {
            TRFileVersion version = DetectVersion(args[0]);
            if (version == TRFileVersion.TR2)
            {
                allLocations[levelType] = ExportTR2Locations(levelType);
            }
            else if (version == TRFileVersion.TR3a || version == TRFileVersion.TR3b)
            {
                allLocations[levelType] = ExportTR3Locations(levelType);
            }
        }
        else if (levelType == "TR2")
        {
            foreach (string lvl in TR2LevelNames.AsList)
            {
                if (File.Exists(lvl))
                {
                    allLocations[lvl] = ExportTR2Locations(lvl);
                }
            }
        }
        else if (levelType == "TR3")
        {
            foreach (string lvl in TR3LevelNames.AsList)
            {
                if (File.Exists(lvl))
                {
                    allLocations[lvl] = ExportTR3Locations(lvl);
                }
            }
        }
        else if (levelType == "TR3G")
        {
            foreach (string lvl in TR3LevelNames.AsListGold)
            {
                if (File.Exists(lvl))
                {
                    allLocations[lvl] = ExportTR3Locations(lvl);
                }
            }
        }

        Console.WriteLine();
        if (allLocations.Count > 0)
        {
            string outputPath;
            string compPath = null;
            if (args.Length > 2)
            {
                outputPath = args[2];
                compPath = args[1];
            }
            else
            {
                outputPath = args.Length > 1 ? args[1] : levelType + "-Locations.json";
            }

            // Are we running a diff?
            if (compPath != null)
            {
                Dictionary<string, List<Location>> previousLocations = JsonConvert.DeserializeObject<Dictionary<string, List<Location>>>(File.ReadAllText(compPath));
                Dictionary<string, List<Location>> newLocations = new();
                foreach (string lvl in allLocations.Keys)
                {
                    newLocations[lvl] = new List<Location>();
                    foreach (Location loc in allLocations[lvl])
                    {
                        // Is this a new location?
                        if (previousLocations[lvl].Find(l => l.X == loc.X && l.Y == loc.Y && l.Z == loc.Z && l.Room == loc.Room) == null)
                        {
                            newLocations[lvl].Add(loc);
                        }
                    }
                }

                allLocations = newLocations;
            }

            File.WriteAllText(outputPath, JsonConvert.SerializeObject(allLocations, Formatting.Indented));
            int count = 0;
            if (allLocations.Count > 1)
            {
                int width = 0;
                allLocations.Keys.ToList().ForEach(k => width = Math.Max(width, k.Length));
                foreach (string lvl in allLocations.Keys)
                {
                    Console.WriteLine("{0} : {1}", lvl.PadRight(width, ' '), allLocations[lvl].Count);
                    count += allLocations[lvl].Count;
                }
                Console.WriteLine();
            }
            else
            {
                count = allLocations.Values.ToList()[0].Count;
            }
            Console.WriteLine("{0} locations exported to {1}", count, outputPath);
        }
        else
        {
            Console.WriteLine("No locations found to export");
        }
    }

    private static TRFileVersion DetectVersion(string path)
    {
        using BinaryReader reader = new(File.Open(path, FileMode.Open));
        return (TRFileVersion)reader.ReadUInt32();
    }

    private static List<Location> ExportTR1Locations(string lvl)
    {
        TR1Level level = _reader1.Read(lvl);
        FDControl floorData = new();
        floorData.ParseFromLevel(level);
        List<Location> exclusions = new();
        if (_allTR1Exclusions.ContainsKey(lvl))
        {
            exclusions.AddRange(_allTR1Exclusions[lvl]);
        }

        foreach (TR1Entity entity in level.Entities)
        {
            if (!TR1TypeUtilities.CanSharePickupSpace(entity.TypeID))
            {
                exclusions.Add(entity.GetFloorLocation(loc =>
                    FDUtilities.GetRoomSector(loc.X, loc.Y, loc.Z, (short)loc.Room, level, floorData)));
            }
        }

        TR1LocationGenerator generator = new();
        return generator.Generate(level, exclusions);
    }

    private static List<Location> ExportTR2Locations(string lvl)
    {
        TR2Level level = _reader2.Read(lvl);
        FDControl floorData = new();
        floorData.ParseFromLevel(level);
        List<Location> exclusions = new();
        if (_allTR2Exclusions.ContainsKey(lvl))
        {
            exclusions.AddRange(_allTR2Exclusions[lvl]);
        }

        foreach (TR2Entity entity in level.Entities)
        {
            if (!TR2TypeUtilities.CanSharePickupSpace(entity.TypeID))
            {
                exclusions.Add(entity.GetFloorLocation(loc =>
                    FDUtilities.GetRoomSector(loc.X, loc.Y, loc.Z, (short)loc.Room, level, floorData)));
            }
        }

        TR2LocationGenerator generator = new();
        return generator.Generate(level, exclusions);
    }

    private static List<Location> ExportTR3Locations(string lvl)
    {
        TR3Level level = _reader3.Read(lvl);
        FDControl floorData = new();
        floorData.ParseFromLevel(level);
        List<Location> exclusions = new();
        if (_allTR3Exclusions.ContainsKey(lvl))
        {
            exclusions.AddRange(_allTR3Exclusions[lvl]);
        }

        foreach (TR3Entity entity in level.Entities)
        {
            if (!TR3TypeUtilities.CanSharePickupSpace(entity.TypeID))
            {
                exclusions.Add(entity.GetFloorLocation(loc =>
                    FDUtilities.GetRoomSector(loc.X, loc.Y, loc.Z, (short)loc.Room, level, floorData)));
            }
        }

        TR3LocationGenerator generator = new();
        return generator.Generate(level, exclusions);
    }

    private static void Usage()
    {
        Console.WriteLine();
        Console.WriteLine("Usage: LocationExport [tr1 | tr2 | tr3 | tr3g | *.phd | *.tr2] [export_path.json] [previous_path.json]");
        Console.WriteLine();

        Console.WriteLine("Target Levels");
        Console.WriteLine("\ttr1      - The original TR1 levels.");
        Console.WriteLine("\ttr2      - The original TR2 levels.");
        Console.WriteLine("\ttr3      - The original TR3 levels.");
        Console.WriteLine("\ttr3g     - The TR3 Lost Artefact levels.");
        Console.WriteLine("\t*.phd    - Use a specific TR1 level file.");
        Console.WriteLine("\t*.tr2    - Use a specific TR2/TR3 level file.");
        Console.WriteLine();

        Console.WriteLine("Export Path");
        Console.WriteLine("\tOptionally set the export JSON path. If blank, the level name/type will be used.");
        Console.WriteLine();

        Console.WriteLine("Previous Path");
        Console.WriteLine("\tOptionally set the previous export JSON path to compare differences with this export.");
        Console.WriteLine();

        Console.WriteLine("Examples");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\tLocationExport TR3");
        Console.ResetColor();
        Console.WriteLine("\t\tGenerate all locations for TR3 to TR3-Locations.json");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\tLocationExport RAPIDS.TR2 rapids.json");
        Console.ResetColor();
        Console.WriteLine("\t\tGenerate all locations for Madubu Gorge to rapids.json");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\tLocationExport TR3 old_locations.json new_locations.json");
        Console.ResetColor();
        Console.WriteLine("\t\tGenerate all locations for TR3 and output only the differences to new_locations.json (excludes old_loctions.json)");
        Console.WriteLine();
    }
}
