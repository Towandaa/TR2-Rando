﻿using System.Collections.Generic;
using System.Linq;
using TREnvironmentEditor.Helpers;
using TRFDControl;
using TRFDControl.Utilities;
using TRLevelReader.Helpers.Pathing;
using TRLevelReader.Model;

namespace TREnvironmentEditor.Model.Types
{
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

        public override void ApplyToLevel(TRLevel level)
        {
            TRRoom room = new TRRoom
            {
                NumXSectors = Width,
                NumZSectors = Depth,
                AlternateRoom = -1,
                AmbientIntensity = AmbientLighting,
                NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
                NumPortals = 0,
                NumStaticMeshes = 0,
                Portals = new TRRoomPortal[] { },
                StaticMeshes = new TRRoomStaticMesh[] { },
                Info = new TRRoomInfo
                {
                    X = Location.X,
                    YBottom = Location.Y,
                    YTop = Location.Y - Height * 256,
                    Z = Location.Z
                },
                RoomData = new TRRoomData
                {
                    // Ignored for now
                    NumSprites = 0,
                    NumTriangles = 0,
                    Sprites = new TRRoomSprite[0],
                    Triangles = new TRFace3[0],
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

            sbyte ceiling = (sbyte)(room.Info.YTop / 256);
            sbyte floor = (sbyte)(room.Info.YBottom / 256);

            List<TRFace4> faces = new List<TRFace4>();
            List<TRVertex> vertices = new List<TRVertex>();

            // Make the sectors first
            List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
            room.Sectors = sectors.ToArray();

            // Generate the box, zone and overlap data
            FDControl floorData = new FDControl();
            floorData.ParseFromLevel(level);

            EMLevelData data = GetData(level);
            TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
            BoxGenerator generator = new BoxGenerator();
            generator.Generate(room, level, linkedSector);

            // Stride the sectors again and make faces
            GenerateFaces(sectors, faces, vertices, room.Info.YTop);

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
            TR2Room room = new TR2Room
            {
                NumXSectors = Width,
                NumZSectors = Depth,
                AlternateRoom = -1,
                AmbientIntensity = AmbientLighting,
                NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
                NumPortals = 0,
                NumStaticMeshes = 0,
                Portals = new TRRoomPortal[] { },
                StaticMeshes = new TR2RoomStaticMesh[] { },
                Info = new TRRoomInfo
                {
                    X = Location.X,
                    YBottom = Location.Y,
                    YTop = Location.Y - Height * 256,
                    Z = Location.Z
                },
                RoomData = new TR2RoomData
                {
                    // Ignored for now
                    NumSprites = 0,
                    NumTriangles = 0,
                    Sprites = new TRRoomSprite[0],
                    Triangles = new TRFace3[0],
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

            sbyte ceiling = (sbyte)(room.Info.YTop / 256);
            sbyte floor = (sbyte)(room.Info.YBottom / 256);

            List<TRFace4> faces = new List<TRFace4>();
            List<TRVertex> vertices = new List<TRVertex>();

            // Make the sectors first
            List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
            room.SectorList = sectors.ToArray();

            // Generate the box, zone and overlap data
            FDControl floorData = new FDControl();
            floorData.ParseFromLevel(level);

            EMLevelData data = GetData(level);
            TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
            BoxGenerator generator = new BoxGenerator();
            generator.Generate(room, level, linkedSector);

            // Stride the sectors again and make faces
            GenerateFaces(sectors, faces, vertices, room.Info.YTop);

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
            TR3Room room = new TR3Room
            {
                NumXSectors = Width,
                NumZSectors = Depth,
                AlternateRoom = -1,
                AmbientIntensity = AmbientLighting,
                NumLights = (ushort)(Lights == null ? 0 : Lights.Length),
                NumPortals = 0,
                NumStaticMeshes = 0,
                Portals = new TRRoomPortal[] { },
                StaticMeshes = new TR3RoomStaticMesh[] { },
                Info = new TRRoomInfo
                {
                    X = Location.X,
                    YBottom = Location.Y,
                    YTop = Location.Y - Height * 256,
                    Z = Location.Z
                },
                RoomData = new TR3RoomData
                {
                    // Ignored for now
                    NumSprites = 0,
                    NumTriangles = 0,
                    Sprites = new TRRoomSprite[0],
                    Triangles = new TRFace3[0],
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

            sbyte ceiling = (sbyte)(room.Info.YTop / 256);
            sbyte floor = (sbyte)(room.Info.YBottom / 256);

            List<TRFace4> faces = new List<TRFace4>();
            List<TRVertex> vertices = new List<TRVertex>();

            // Make the sectors first
            List<TRRoomSector> sectors = GenerateSectors(ceiling, floor);
            room.Sectors = sectors.ToArray();

            // Generate the box, zone and overlap data
            FDControl floorData = new FDControl();
            floorData.ParseFromLevel(level);

            EMLevelData data = GetData(level);
            TRRoomSector linkedSector = FDUtilities.GetRoomSector(LinkedLocation.X, LinkedLocation.Y, LinkedLocation.Z, data.ConvertRoom(LinkedLocation.Room), level, floorData);
            BoxGenerator generator = new BoxGenerator();
            generator.Generate(room, level, linkedSector);

            // Stride the sectors again and make faces
            GenerateFaces(sectors, faces, vertices, room.Info.YTop);

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
            List<TRRoomSector> sectors = new List<TRRoomSector>();
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    int sectorIndex = x * Depth + z;
                    bool isWall = x == 0 || x == Width - 1 || z == 0 || z == Depth - 1;

                    sbyte sectorFloor = isWall ? (sbyte)-127 : floor;
                    if (!isWall && FloorHeights != null)
                    {
                        foreach (sbyte specificFloor in FloorHeights.Keys)
                        {
                            if (FloorHeights[specificFloor].Contains(sectorIndex))
                            {
                                sectorFloor = specificFloor == -127 ? specificFloor : (sbyte)(floor + specificFloor);
                                if (sectorFloor == -127)
                                {
                                    isWall = true;
                                }
                            }
                        }
                    }

                    sectors.Add(new TRRoomSector
                    {
                        FDIndex = 0,
                        BoxIndex = ushort.MaxValue,
                        Ceiling = isWall ? (sbyte)-127 : ceiling,
                        Floor = sectorFloor,
                        RoomAbove = 255,
                        RoomBelow = 255
                    });
                }
            }

            return sectors;
        }

        private void GenerateFaces(List<TRRoomSector> sectors, List<TRFace4> faces, List<TRVertex> vertices, int roomTop)
        {
            sbyte ceiling = (sbyte)(roomTop / 256);
            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    TRRoomSector sector = sectors[x * Depth + z];
                    if (sector.IsImpenetrable)
                    {
                        continue;
                    }

                    TRRoomSector westNeighbour = sectors[(x - 1) * Depth + z];
                    TRRoomSector northNeighbour = sectors[x * Depth + z + 1];
                    TRRoomSector eastNeighbour = sectors[(x + 1) * Depth + z];
                    TRRoomSector southNeighbour = sectors[x * Depth + (z - 1)];

                    BuildFace(faces, vertices, x, z, sector.Floor * 256, Direction.Down);
                    BuildFace(faces, vertices, x, z, roomTop, Direction.Up);

                    if (westNeighbour.Floor < sector.Floor)
                    {
                        BuildWallFaces(faces, vertices, x, z, westNeighbour.Floor, sector.Floor, ceiling, Direction.West);
                    }
                    if (northNeighbour.Floor < sector.Floor)
                    {
                        BuildWallFaces(faces, vertices, x, z + 1, northNeighbour.Floor, sector.Floor, ceiling, Direction.North);
                    }
                    if (eastNeighbour.Floor < sector.Floor)
                    {
                        BuildWallFaces(faces, vertices, x + 1, z, eastNeighbour.Floor, sector.Floor, ceiling, Direction.East);
                    }
                    if (southNeighbour.Floor < sector.Floor)
                    {
                        BuildWallFaces(faces, vertices, x, z, southNeighbour.Floor, sector.Floor, ceiling, Direction.South);
                    }
                }
            }
        }

        private void BuildWallFaces(List<TRFace4> faces, List<TRVertex> vertices, int x, int z, int topY, int bottomY, int ceiling, Direction direction)
        {
            if (topY == -127)
            {
                topY = ceiling;
            }

            // If a room is 16 clicks tall
            //    => Four 1024 x 1024 faces
            // If a room is 18 clicks tall
            //    => Four 1024 x 1024 faces
            //    => One 1024 x 512 face

            int yChange = bottomY - topY;
            int height = yChange * 256;
            int squareCount = height / 1024;
            int offset = height % 1024;

            int y = bottomY * 256;

            if (Textures.WallAlignment == Direction.Down && offset > 0)
            {
                BuildFace(faces, vertices, x, z, y, direction, offset);
                y -= offset;
            }

            for (int i = 0; i < squareCount; i++)
            {
                BuildFace(faces, vertices, x, z, y - i * 1024, direction);
            }

            if (Textures.WallAlignment != Direction.Down && offset > 0)
            {
                y -= squareCount * 1024;
                BuildFace(faces, vertices, x, z, y, direction, offset);
            }
        }

        private void BuildFace(List<TRFace4> faces, List<TRVertex> vertices, int x, int z, int y, Direction direction, int height = 1024)
        {
            ushort texture;
            switch (direction)
            {
                case Direction.Down:
                    texture = Textures.Floor;
                    break;
                case Direction.Up:
                    texture = Textures.Ceiling;
                    break;
                default:
                    texture = Textures.GetWall(height);
                    break;
            }
            TRFace4 face = new TRFace4
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
        }

        private List<TRVertex> BuildFlatVertices(int x, int y, int z)
        {
            return new List<TRVertex>
            {
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)y,
                    Z = (short)((z + 1) * 1024)
                },
                new TRVertex
                {
                    X = (short)((x + 1) * 1024),
                    Y = (short)y,
                    Z = (short)((z + 1) * 1024)
                },
                new TRVertex
                {
                    X = (short)((x + 1) * 1024),
                    Y = (short)y,
                    Z = (short)(z * 1024)
                },
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)y,
                    Z = (short)(z * 1024)
                }
            };
        }

        private List<TRVertex> BuildZWallVertices(int x, int y, int z, int height)
        {
            return new List<TRVertex>
            {
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)(y - height),
                    Z = (short)(z * 1024)
                },
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)(y - height),
                    Z = (short)((z + 1) * 1024)
                },
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)y,
                    Z = (short)((z + 1) * 1024)
                },
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)y,
                    Z = (short)(z * 1024)
                }
            };
        }

        private List<TRVertex> BuildXWallVertices(int x, int y, int z, int height)
        {
            return new List<TRVertex>
            {
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)(y - height),
                    Z = (short)(z * 1024)
                },
                new TRVertex
                {
                    X = (short)((x + 1) * 1024),
                    Y = (short)(y - height),
                    Z = (short)(z * 1024)
                },
                new TRVertex
                {
                    X = (short)((x + 1) * 1024),
                    Y = (short)y,
                    Z = (short)(z * 1024)
                },
                new TRVertex
                {
                    X = (short)(x * 1024),
                    Y = (short)y,
                    Z = (short)(z * 1024)
                }
            };
        }
    }
}
