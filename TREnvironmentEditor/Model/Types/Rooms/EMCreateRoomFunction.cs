﻿using TREnvironmentEditor.Helpers;
using TRFDControl;
using TRFDControl.Utilities;
using TRLevelControl;
using TRLevelControl.Helpers.Pathing;
using TRLevelControl.Model;

namespace TREnvironmentEditor.Model.Types;

public class EMCreateRoomFunction : BaseEMFunction
{
    public EMLocation Location { get; set; }
    public EMLocation LinkedLocation { get; set; }
    public EMTextureGroup Textures { get; set; }
    public short AmbientLighting { get; set; }
    public EMRoomVertex DefaultVertex { get; set; }
    public EMRoomLight[] Lights { get; set; }
    public byte Height { get; set; }
    public ushort Width { get; set; }
    public ushort Depth { get; set; }
    public Dictionary<sbyte, List<int>> FloorHeights { get; set; }
    public Dictionary<sbyte, List<int>> CeilingHeights { get; set; }

    public override void ApplyToLevel(TR1Level level)
    {
        TRRoom room = new()
        {
            NumXSectors = Width,
            NumZSectors = Depth,
            AlternateRoom = -1,
            AmbientIntensity = AmbientLighting,
            NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
            NumPortals = 0,
            NumStaticMeshes = 0,
            Portals = Array.Empty<TRRoomPortal>(),
            StaticMeshes = Array.Empty<TRRoomStaticMesh>(),
            Info = new TRRoomInfo
            {
                X = Location.X,
                YBottom = Location.Y,
                YTop = Location.Y - Height * TRConsts.Step1,
                Z = Location.Z
            },
            RoomData = new TRRoomData
            {
                // Ignored for now
                NumSprites = 0,
                NumTriangles = 0,
                Sprites = Array.Empty<TRRoomSprite>(),
                Triangles = Array.Empty<TRFace3>(),
            }
        };

        room.Lights = new TRRoomLight[room.NumLights];
        for (int i = 0; i < room.NumLights; i++)
        {
            EMRoomLight light = Lights[i];
            room.Lights[i] = new TRRoomLight
            {
                X = light.X + Location.X,
                Y = light.Y + Location.Y,
                Z = light.Z + Location.Z,
                Fade = light.Fade1,
                Intensity = light.Intensity1
            };
        }

        sbyte ceiling = (sbyte)(room.Info.YTop / TRConsts.Step1);
        sbyte floor = (sbyte)(room.Info.YBottom / TRConsts.Step1);

        List<TRFace4> faces = new();
        List<TRVertex> vertices = new();

        // Make the sectors first
        List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
        room.Sectors = sectors.ToArray();

        // Generate the box, zone and overlap data
        FDControl floorData = new();
        floorData.ParseFromLevel(level);

        EMLevelData data = GetData(level);
        TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
        BoxGenerator.Generate(room, level, linkedSector);

        // Stride the sectors again and make faces
        GenerateFaces(sectors, faces, vertices);

        // Write it all to the room
        room.RoomData.NumRectangles = (short)faces.Count;
        room.RoomData.NumVertices = (short)vertices.Count;
        room.RoomData.Rectangles = faces.ToArray();
        room.RoomData.Vertices = vertices.Select(v => new TRRoomVertex
        {
            Lighting = DefaultVertex.Lighting,
            Vertex = v
        }).ToArray();

        room.NumDataWords = (uint)(room.RoomData.Serialize().Length / 2);

        List<TRRoom> rooms = level.Rooms.ToList();
        rooms.Add(room);
        level.Rooms = rooms.ToArray();
        level.NumRooms++;
    }

    public override void ApplyToLevel(TR2Level level)
    {
        TR2Room room = new()
        {
            NumXSectors = Width,
            NumZSectors = Depth,
            AlternateRoom = -1,
            AmbientIntensity = AmbientLighting,
            NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
            NumPortals = 0,
            NumStaticMeshes = 0,
            Portals = Array.Empty<TRRoomPortal>(),
            StaticMeshes = Array.Empty<TR2RoomStaticMesh>(),
            Info = new TRRoomInfo
            {
                X = Location.X,
                YBottom = Location.Y,
                YTop = Location.Y - Height * TRConsts.Step1,
                Z = Location.Z
            },
            RoomData = new TR2RoomData
            {
                // Ignored for now
                NumSprites = 0,
                NumTriangles = 0,
                Sprites = Array.Empty<TRRoomSprite>(),
                Triangles = Array.Empty<TRFace3>(),
            }
        };

        room.Lights = new TR2RoomLight[room.NumLights];
        for (int i = 0; i < room.NumLights; i++)
        {
            EMRoomLight light = Lights[i];
            room.Lights[i] = new TR2RoomLight
            {
                X = light.X + Location.X,
                Y = light.Y + Location.Y,
                Z = light.Z + Location.Z,
                Fade1 = light.Fade1,
                Fade2 = light.Fade2,
                Intensity1 = light.Intensity1,
                Intensity2 = light.Intensity2,
            };
        }

        sbyte ceiling = (sbyte)(room.Info.YTop / TRConsts.Step1);
        sbyte floor = (sbyte)(room.Info.YBottom / TRConsts.Step1);

        List<TRFace4> faces = new();
        List<TRVertex> vertices = new();

        // Make the sectors first
        List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
        room.SectorList = sectors.ToArray();

        // Generate the box, zone and overlap data
        FDControl floorData = new();
        floorData.ParseFromLevel(level);

        EMLevelData data = GetData(level);
        TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
        BoxGenerator.Generate(room, level, linkedSector);

        // Stride the sectors again and make faces
        GenerateFaces(sectors, faces, vertices);

        // Write it all to the room
        room.RoomData.NumRectangles = (short)faces.Count;
        room.RoomData.NumVertices = (short)vertices.Count;
        room.RoomData.Rectangles = faces.ToArray();
        room.RoomData.Vertices = vertices.Select(v => new TR2RoomVertex
        {
            Lighting = DefaultVertex.Lighting,
            Lighting2 = DefaultVertex.Lighting2,
            Attributes = DefaultVertex.Attributes,
            Vertex = v
        }).ToArray();

        room.NumDataWords = (uint)(room.RoomData.Serialize().Length / 2);

        List<TR2Room> rooms = level.Rooms.ToList();
        rooms.Add(room);
        level.Rooms = rooms.ToArray();
        level.NumRooms++;
    }

    public override void ApplyToLevel(TR3Level level)
    {
        TR3Room room = new()
        {
            NumXSectors = Width,
            NumZSectors = Depth,
            AlternateRoom = -1,
            AmbientIntensity = AmbientLighting,
            NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
            NumPortals = 0,
            NumStaticMeshes = 0,
            Portals = Array.Empty<TRRoomPortal>(),
            StaticMeshes = Array.Empty<TR3RoomStaticMesh>(),
            Info = new TRRoomInfo
            {
                X = Location.X,
                YBottom = Location.Y,
                YTop = Location.Y - Height * TRConsts.Step1,
                Z = Location.Z
            },
            RoomData = new TR3RoomData
            {
                // Ignored for now
                NumSprites = 0,
                NumTriangles = 0,
                Sprites = Array.Empty<TRRoomSprite>(),
                Triangles = Array.Empty<TRFace3>(),
            }
        };

        room.Lights = new TR3RoomLight[room.NumLights];
        for (int i = 0; i < room.NumLights; i++)
        {
            EMRoomLight light = Lights[i];
            room.Lights[i] = new TR3RoomLight
            {
                X = light.X + Location.X,
                Y = light.Y + Location.Y,
                Z = light.Z + Location.Z,
                Colour = light.Colour,
                LightProperties = light.LightProperties,
                LightType = light.LightType,
            };
        }

        sbyte ceiling = (sbyte)(room.Info.YTop / TRConsts.Step1);
        sbyte floor = (sbyte)(room.Info.YBottom / TRConsts.Step1);

        List<TRFace4> faces = new();
        List<TRVertex> vertices = new();

        // Make the sectors first
        List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
        room.Sectors = sectors.ToArray();

        // Generate the box, zone and overlap data
        FDControl floorData = new();
        floorData.ParseFromLevel(level);

        EMLevelData data = GetData(level);
        TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
        BoxGenerator.Generate(room, level, linkedSector);

        // Stride the sectors again and make faces
        GenerateFaces(sectors, faces, vertices);

        // Write it all to the room
        room.RoomData.NumRectangles = (short)faces.Count;
        room.RoomData.NumVertices = (short)vertices.Count;
        room.RoomData.Rectangles = faces.ToArray();
        room.RoomData.Vertices = vertices.Select(v => new TR3RoomVertex
        {
            Lighting = DefaultVertex.Lighting,
            Attributes = DefaultVertex.Attributes,
            Colour = DefaultVertex.Colour,
            Vertex = v
        }).ToArray();

        room.NumDataWords = (uint)(room.RoomData.Serialize().Length / 2);

        List<TR3Room> rooms = level.Rooms.ToList();
        rooms.Add(room);
        level.Rooms = rooms.ToArray();
        level.NumRooms++;
    }

    private List<TRRoomSector> GenerateSectors(sbyte ceiling, sbyte floor)
    {
        List<TRRoomSector> sectors = new();
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Depth; z++)
            {
                int sectorIndex = x * Depth + z;
                bool isWall = x == 0 || x == Width - 1 || z == 0 || z == Depth - 1;

                sbyte sectorFloor = floor;
                sbyte sectorCeiling = ceiling;
                if (!isWall)
                {
                    sbyte height = GetSectorHeight(sectorIndex, FloorHeights);
                    sectorFloor = height == TRConsts.WallClicks ? height : (sbyte)(sectorFloor + height);
                }
                if (!isWall)
                {
                    sbyte height = GetSectorHeight(sectorIndex, CeilingHeights);
                    sectorCeiling = height == TRConsts.WallClicks ? height : (sbyte)(sectorCeiling + height);
                }

                if (isWall || sectorFloor == TRConsts.WallClicks || sectorCeiling == TRConsts.WallClicks)
                {
                    sectorFloor = sectorCeiling = TRConsts.WallClicks;
                }

                sectors.Add(new TRRoomSector
                {
                    FDIndex = 0,
                    BoxIndex = ushort.MaxValue,
                    Ceiling = sectorCeiling,
                    Floor = sectorFloor,
                    RoomAbove = TRConsts.NoRoom,
                    RoomBelow = TRConsts.NoRoom
                });
            }
        }

        return sectors;
    }

    private static sbyte GetSectorHeight(int sectorIndex, Dictionary<sbyte, List<int>> specificHeights)
    {
        if (specificHeights != null)
        {
            foreach (sbyte height in specificHeights.Keys)
            {
                if (specificHeights[height].Contains(sectorIndex))
                {
                    return height;
                }
            }
        }

        return 0;
    }

    private void GenerateFaces(List<TRRoomSector> sectors, List<TRFace4> faces, List<TRVertex> vertices)
    {
        for (int x = 0; x < Width; x++)
        {
            for (int z = 0; z < Depth; z++)
            {
                TRRoomSector sector = sectors[x * Depth + z];
                if (sector.IsWall)
                {
                    continue;
                }

                TRRoomSector westNeighbour = sectors[(x - 1) * Depth + z];
                TRRoomSector northNeighbour = sectors[x * Depth + z + 1];
                TRRoomSector eastNeighbour = sectors[(x + 1) * Depth + z];
                TRRoomSector southNeighbour = sectors[x * Depth + (z - 1)];

                BuildFace(faces, vertices, x, z, sector.Floor * TRConsts.Step1, Direction.Down);
                BuildFace(faces, vertices, x, z, sector.Ceiling * TRConsts.Step1, Direction.Up);

                if (westNeighbour.Floor < sector.Floor)
                {
                    BuildWallFaces(faces, vertices, x, z, westNeighbour.Floor, sector.Floor, sector.Ceiling, Direction.West);
                }
                if (westNeighbour.Ceiling > sector.Ceiling)
                {
                    BuildWallFaces(faces, vertices, x, z, sector.Ceiling, westNeighbour.Ceiling, sector.Ceiling, Direction.West);
                }

                if (northNeighbour.Floor < sector.Floor)
                {
                    BuildWallFaces(faces, vertices, x, z + 1, northNeighbour.Floor, sector.Floor, sector.Ceiling, Direction.North);
                }
                if (northNeighbour.Ceiling > sector.Ceiling)
                {
                    BuildWallFaces(faces, vertices, x, z + 1, sector.Ceiling, northNeighbour.Ceiling, sector.Ceiling, Direction.North);
                }

                if (eastNeighbour.Floor < sector.Floor)
                {
                    BuildWallFaces(faces, vertices, x + 1, z, eastNeighbour.Floor, sector.Floor, sector.Ceiling, Direction.East);
                }
                if (eastNeighbour.Ceiling > sector.Ceiling)
                {
                    BuildWallFaces(faces, vertices, x + 1, z, sector.Ceiling, eastNeighbour.Ceiling, sector.Ceiling, Direction.East);
                }

                if (southNeighbour.Floor < sector.Floor)
                {
                    BuildWallFaces(faces, vertices, x, z, southNeighbour.Floor, sector.Floor, sector.Ceiling, Direction.South);
                }
                if (southNeighbour.Ceiling > sector.Ceiling)
                {
                    BuildWallFaces(faces, vertices, x, z, sector.Ceiling, southNeighbour.Ceiling, sector.Ceiling, Direction.South);
                }
            }
        }

        // This is more in line with OG where vertices are shared and avoids odd caustic effects.
        List<TRVertex> distinctVertices = vertices.GroupBy(v => new { v.X, v.Y, v.Z })
            .Select(v => v.First())
            .ToList();

        foreach (TRFace4 face in faces)
        {
            for (int i = 0; i < face.Vertices.Length; i++)
            {
                TRVertex vertex = vertices[face.Vertices[i]];
                face.Vertices[i] = (ushort)distinctVertices.FindIndex(v => v.X == vertex.X && v.Y == vertex.Y && v.Z == vertex.Z);
            }
        }

        vertices.Clear();
        vertices.AddRange(distinctVertices);
    }

    private void BuildWallFaces(List<TRFace4> faces, List<TRVertex> vertices, int x, int z, int topY, int bottomY, int ceiling, Direction direction)
    {
        if (topY == TRConsts.WallClicks)
        {
            topY = ceiling;
        }

        // If a room is 16 clicks tall
        //    => Four 1024 x 1024 faces
        // If a room is 18 clicks tall
        //    => Four 1024 x 1024 faces
        //    => One 1024 x 512 face

        int yChange = bottomY - topY;
        int height = yChange * TRConsts.Step1;
        int squareCount = height / TRConsts.Step4;
        int offset = height % TRConsts.Step4;

        int y = bottomY * TRConsts.Step1;

        if (Textures.WallAlignment == Direction.Down && offset > 0)
        {
            BuildFace(faces, vertices, x, z, y, direction, offset);
            y -= offset;
        }

        for (int i = 0; i < squareCount; i++)
        {
            BuildFace(faces, vertices, x, z, y - i * TRConsts.Step4, direction);
        }

        if (Textures.WallAlignment != Direction.Down && offset > 0)
        {
            y -= squareCount * TRConsts.Step4;
            BuildFace(faces, vertices, x, z, y, direction, offset);
        }
    }

    private void BuildFace(List<TRFace4> faces, List<TRVertex> vertices, int x, int z, int y, Direction direction, int height = TRConsts.Step4)
    {
        ushort texture = direction switch
        {
            Direction.Down => Textures.Floor,
            Direction.Up => Textures.Ceiling,
            _ => Textures.GetWall(height),
        };
        TRFace4 face = new()
        {
            Texture = texture,
            Vertices = new ushort[]
            {
                (ushort)vertices.Count,
                (ushort)(vertices.Count + 1),
                (ushort)(vertices.Count + 2),
                (ushort)(vertices.Count + 3),
            }
        };
        faces.Add(face);
        if (direction == Direction.Up
            || direction == Direction.East
            || direction == Direction.South)
        {
            (face.Vertices[1], face.Vertices[0]) = (face.Vertices[0], face.Vertices[1]);
            (face.Vertices[3], face.Vertices[2]) = (face.Vertices[2], face.Vertices[3]);
        }

        switch (direction)
        {
            case Direction.Down:
            case Direction.Up:
                vertices.AddRange(BuildFlatVertices(x, y, z));
                break;
            case Direction.North:
            case Direction.South:
                vertices.AddRange(BuildXWallVertices(x, y, z, height));
                break;
            case Direction.West:
            case Direction.East:
                vertices.AddRange(BuildZWallVertices(x, y, z, height));
                break;
        }

        Textures.RandomizeRotation(face, height);
    }

    private static List<TRVertex> BuildFlatVertices(int x, int y, int z)
    {
        return new List<TRVertex>
        {
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)y,
                Z = (short)((z + 1) * TRConsts.Step4)
            },
            new() {
                X = (short)((x + 1) * TRConsts.Step4),
                Y = (short)y,
                Z = (short)((z + 1) * TRConsts.Step4)
            },
            new() {
                X = (short)((x + 1) * TRConsts.Step4),
                Y = (short)y,
                Z = (short)(z * TRConsts.Step4)
            },
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)y,
                Z = (short)(z * TRConsts.Step4)
            }
        };
    }

    private static List<TRVertex> BuildZWallVertices(int x, int y, int z, int height)
    {
        return new List<TRVertex>
        {
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)(y - height),
                Z = (short)(z * TRConsts.Step4)
            },
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)(y - height),
                Z = (short)((z + 1) * TRConsts.Step4)
            },
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)y,
                Z = (short)((z + 1) * TRConsts.Step4)
            },
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)y,
                Z = (short)(z * TRConsts.Step4)
            }
        };
    }

    private static List<TRVertex> BuildXWallVertices(int x, int y, int z, int height)
    {
        return new List<TRVertex>
        {
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)(y - height),
                Z = (short)(z * TRConsts.Step4)
            },
            new() {
                X = (short)((x + 1) * TRConsts.Step4),
                Y = (short)(y - height),
                Z = (short)(z * TRConsts.Step4)
            },
            new() {
                X = (short)((x + 1) * TRConsts.Step4),
                Y = (short)y,
                Z = (short)(z * TRConsts.Step4)
            },
            new() {
                X = (short)(x * TRConsts.Step4),
                Y = (short)y,
                Z = (short)(z * TRConsts.Step4)
            }
        };
    }
}
