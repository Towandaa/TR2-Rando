﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TRLevelReader.Model;

namespace TRLevelReader
{
    internal static class TR5FileReadUtilities
    {
        public static void PopulateRooms(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.Unused = reader.ReadUInt32();
            lvl.LevelDataChunk.NumRooms = reader.ReadUInt16();
            lvl.LevelDataChunk.Rooms = new TR5Room[lvl.LevelDataChunk.NumRooms];

            for (int i = 0; i < lvl.LevelDataChunk.NumRooms; i++)
            {
                TR5Room room = new TR5Room();

                //It seems there are two bytes before XELA landmark?
                reader.BaseStream.Position += 2;

                room.XELALandmark = reader.ReadBytes(4);

                Debug.Assert(room.XELALandmark[0] == 'X');
                Debug.Assert(room.XELALandmark[1] == 'E');
                Debug.Assert(room.XELALandmark[2] == 'L');
                Debug.Assert(room.XELALandmark[3] == 'A');

                room.RoomDataSize = reader.ReadUInt32();
                room.Seperator = reader.ReadUInt32();
                room.EndSDOffset = reader.ReadUInt32();
                room.StartSDOffset = reader.ReadUInt32();
                room.Seperator2 = reader.ReadUInt32();
                room.EndPortalOffset = reader.ReadUInt32();

                TR5RoomInfo info = new TR5RoomInfo
                {
                    X = reader.ReadInt32(),
                    Y = reader.ReadInt32(),
                    Z = reader.ReadInt32(),
                    yBottom = reader.ReadInt32(),
                    yTop = reader.ReadInt32()
                };

                room.Info = info;

                room.NumZSectors = reader.ReadUInt16();
                room.NumXSectors = reader.ReadUInt16();
                room.RoomColourARGB = reader.ReadUInt32();
                room.NumLights = reader.ReadUInt16();
                room.NumStaticMeshes = reader.ReadUInt16();

                room.Reverb = reader.ReadByte();
                room.AlternateGroup = reader.ReadByte();
                room.WaterScheme = reader.ReadUInt16();

                room.Filler = new uint[2];
                room.Filler[0] = reader.ReadUInt32();
                room.Filler[1] = reader.ReadUInt32();

                room.Seperator3 = new uint[2];
                room.Seperator3[0] = reader.ReadUInt32();
                room.Seperator3[1] = reader.ReadUInt32();

                room.Filler2 = reader.ReadUInt32();
                room.AlternateRoom = reader.ReadUInt16();
                room.Flags = reader.ReadUInt16();

                room.Unknown1 = reader.ReadUInt32();
                room.Unknown2 = reader.ReadUInt32();
                room.Unknown3 = reader.ReadUInt32();
                room.Seperator4 = reader.ReadUInt32();
                room.Unknown4 = reader.ReadUInt16();
                room.Unknown5 = reader.ReadUInt16();

                room.RoomX = reader.ReadSingle();
                room.RoomY = reader.ReadSingle();
                room.RoomZ = reader.ReadSingle();

                room.Seperator5 = new uint[4];
                room.Seperator5[0] = reader.ReadUInt32();
                room.Seperator5[1] = reader.ReadUInt32();
                room.Seperator5[2] = reader.ReadUInt32();
                room.Seperator5[3] = reader.ReadUInt32();

                room.Seperator6 = reader.ReadUInt32();
                room.Seperator7 = reader.ReadUInt32();

                room.NumRoomTriangles = reader.ReadUInt32();
                room.NumRoomRectangles = reader.ReadUInt32();

                room.RoomLightsPtr = reader.ReadUInt32();
                room.RoomFogBulbsPtr = reader.ReadUInt32();

                room.NumLights2 = reader.ReadUInt32();
                room.NumFogBulbs = reader.ReadUInt32();

                //TRosettaStone talks about some unknown here?
                reader.BaseStream.Position += 4;

                room.RoomYTop = reader.ReadInt32();
                room.RoomYBottom = reader.ReadInt32();

                room.LayersPtr = reader.ReadUInt32();
                room.VerticesPtr = reader.ReadUInt32();
                room.PolyOffset = reader.ReadUInt32();
                room.PolyOffset2 = reader.ReadUInt32();
                room.NumVertices = reader.ReadUInt32();

                room.Seperator8 = new uint[4];
                room.Seperator8[0] = reader.ReadUInt32();
                room.Seperator8[1] = reader.ReadUInt32();
                room.Seperator8[2] = reader.ReadUInt32();
                room.Seperator8[3] = reader.ReadUInt32();

                TR5RoomData data = new TR5RoomData();

                data.Lights = new TR5RoomLight[room.NumLights];
                //read lights

                data.FogBulbs = new TR5FogBulb[room.NumFogBulbs];
                //read bulbs

                data.SectorList = new TRRoomSector[room.NumXSectors * room.NumZSectors];
                //read sectors

                data.NumPortals = reader.ReadUInt16();
                data.Portals = new TRRoomPortal[data.NumPortals];
                //read portals

                data.Seperator = reader.ReadUInt16();

                data.StaticMeshes = new TR3RoomStaticMesh[room.NumStaticMeshes];
                //read static meshes

                data.Layers = new TR5RoomLayer[room.NumLayers];
                //read layers

                data.Faces = reader.ReadBytes((int)(room.NumRoomRectangles * 10) + (int)(room.NumRoomTriangles * 8));

                data.Vertices = new TR5RoomVertex[room.NumVertices];
                //read vertices

                lvl.LevelDataChunk.Rooms[i] = room;
            }
        }

        public static void PopulateFloordata(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.NumFloorData = reader.ReadUInt32();
            lvl.LevelDataChunk.Floordata = new ushort[lvl.LevelDataChunk.NumFloorData];

            for (int i = 0; i < lvl.LevelDataChunk.NumFloorData; i++)
            {
                lvl.LevelDataChunk.Floordata[i] = reader.ReadUInt16();
            }
        }

        public static void PopulateMeshes(BinaryReader reader, TR5Level lvl)
        {
            //Mesh Data
            //This tells us how much mesh data (# of words/uint16s) coming up
            //just like the rooms previously.
            lvl.LevelDataChunk.NumMeshData = reader.ReadUInt32();
            lvl.LevelDataChunk.RawMeshData = new ushort[lvl.LevelDataChunk.NumMeshData];

            for (int i = 0; i < lvl.LevelDataChunk.NumMeshData; i++)
            {
                lvl.LevelDataChunk.RawMeshData[i] = reader.ReadUInt16();
            }

            //Mesh Pointers
            lvl.LevelDataChunk.NumMeshPointers = reader.ReadUInt32();
            lvl.LevelDataChunk.MeshPointers = new uint[lvl.LevelDataChunk.NumMeshPointers];

            for (int i = 0; i < lvl.LevelDataChunk.NumMeshPointers; i++)
            {
                lvl.LevelDataChunk.MeshPointers[i] = reader.ReadUInt32();
            }

            //Mesh Construction
            //level.Meshes = ConstructMeshData(level.NumMeshData, level.NumMeshPointers, level.RawMeshData);
            lvl.LevelDataChunk.Meshes = TR4FileReadUtilities.ConstructMeshData(lvl.LevelDataChunk.MeshPointers, lvl.LevelDataChunk.RawMeshData);
        }

        public static void PopulateAnimations(BinaryReader reader, TR5Level lvl)
        {
            //Animations
            lvl.LevelDataChunk.NumAnimations = reader.ReadUInt32();
            lvl.LevelDataChunk.Animations = new TR4Animation[lvl.LevelDataChunk.NumAnimations];
            for (int i = 0; i < lvl.LevelDataChunk.NumAnimations; i++)
            {
                lvl.LevelDataChunk.Animations[i] = TR4FileReadUtilities.ReadAnimation(reader);
            }

            //State Changes
            lvl.LevelDataChunk.NumStateChanges = reader.ReadUInt32();
            lvl.LevelDataChunk.StateChanges = new TRStateChange[lvl.LevelDataChunk.NumStateChanges];
            for (int i = 0; i < lvl.LevelDataChunk.NumStateChanges; i++)
            {
                lvl.LevelDataChunk.StateChanges[i] = TR2FileReadUtilities.ReadStateChange(reader);
            }

            //Animation Dispatches
            lvl.LevelDataChunk.NumAnimDispatches = reader.ReadUInt32();
            lvl.LevelDataChunk.AnimDispatches = new TRAnimDispatch[lvl.LevelDataChunk.NumAnimDispatches];
            for (int i = 0; i < lvl.LevelDataChunk.NumAnimDispatches; i++)
            {
                lvl.LevelDataChunk.AnimDispatches[i] = TR2FileReadUtilities.ReadAnimDispatch(reader);
            }

            //Animation Commands
            lvl.LevelDataChunk.NumAnimCommands = reader.ReadUInt32();
            lvl.LevelDataChunk.AnimCommands = new TRAnimCommand[lvl.LevelDataChunk.NumAnimCommands];
            for (int i = 0; i < lvl.LevelDataChunk.NumAnimCommands; i++)
            {
                lvl.LevelDataChunk.AnimCommands[i] = TR2FileReadUtilities.ReadAnimCommand(reader);
            }
        }

        public static void PopulateMeshTreesFramesModels(BinaryReader reader, TR5Level lvl)
        {
            //Mesh Trees
            lvl.LevelDataChunk.NumMeshTrees = reader.ReadUInt32();
            lvl.LevelDataChunk.NumMeshTrees /= 4;
            lvl.LevelDataChunk.MeshTrees = new TRMeshTreeNode[lvl.LevelDataChunk.NumMeshTrees];
            for (int i = 0; i < lvl.LevelDataChunk.NumMeshTrees; i++)
            {
                lvl.LevelDataChunk.MeshTrees[i] = TR2FileReadUtilities.ReadMeshTreeNode(reader);
            }

            //Frames
            lvl.LevelDataChunk.NumFrames = reader.ReadUInt32();
            lvl.LevelDataChunk.Frames = new ushort[lvl.LevelDataChunk.NumFrames];
            for (int i = 0; i < lvl.LevelDataChunk.NumFrames; i++)
            {
                lvl.LevelDataChunk.Frames[i] = reader.ReadUInt16();
            }

            //Models
            lvl.LevelDataChunk.NumModels = reader.ReadUInt32();
            lvl.LevelDataChunk.Models = new TRModel[lvl.LevelDataChunk.NumModels];

            for (int i = 0; i < lvl.LevelDataChunk.NumModels; i++)
            {
                lvl.LevelDataChunk.Models[i] = TR2FileReadUtilities.ReadModel(reader);
            }
        }

        public static void PopulateStaticMeshes(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.NumStaticMeshes = reader.ReadUInt32();
            lvl.LevelDataChunk.StaticMeshes = new TRStaticMesh[lvl.LevelDataChunk.NumStaticMeshes];

            for (int i = 0; i < lvl.LevelDataChunk.NumStaticMeshes; i++)
            {
                lvl.LevelDataChunk.StaticMeshes[i] = TR2FileReadUtilities.ReadStaticMesh(reader);
            }
        }

        public static void VerifySPRMarker(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.SPRMarker = reader.ReadBytes(3);

            Debug.Assert(lvl.LevelDataChunk.SPRMarker[0] == 0x53);
            Debug.Assert(lvl.LevelDataChunk.SPRMarker[1] == 0x50);
            Debug.Assert(lvl.LevelDataChunk.SPRMarker[2] == 0x52);
        }

        public static void PopulateSprites(BinaryReader reader, TR5Level lvl)
        {
            //Sprite Textures
            lvl.LevelDataChunk.NumSpriteTextures = reader.ReadUInt32();
            lvl.LevelDataChunk.SpriteTextures = new TRSpriteTexture[lvl.LevelDataChunk.NumSpriteTextures];

            for (int i = 0; i < lvl.LevelDataChunk.NumSpriteTextures; i++)
            {
                lvl.LevelDataChunk.SpriteTextures[i] = TR2FileReadUtilities.ReadSpriteTexture(reader);
            }

            //Sprite Sequences
            lvl.LevelDataChunk.NumSpriteSequences = reader.ReadUInt32();
            lvl.LevelDataChunk.SpriteSequences = new TRSpriteSequence[lvl.LevelDataChunk.NumSpriteSequences];

            for (int i = 0; i < lvl.LevelDataChunk.NumSpriteSequences; i++)
            {
                lvl.LevelDataChunk.SpriteSequences[i] = TR2FileReadUtilities.ReadSpriteSequence(reader);
            }
        }

        public static void PopulateCameras(BinaryReader reader, TR5Level lvl)
        {
            //Cameras
            lvl.LevelDataChunk.NumCameras = reader.ReadUInt32();
            lvl.LevelDataChunk.Cameras = new TRCamera[lvl.LevelDataChunk.NumCameras];

            for (int i = 0; i < lvl.LevelDataChunk.NumCameras; i++)
            {
                lvl.LevelDataChunk.Cameras[i] = TR2FileReadUtilities.ReadCamera(reader);
            }

            //Flyby Cameras
            lvl.LevelDataChunk.NumFlybyCameras = reader.ReadUInt32();
            lvl.LevelDataChunk.FlybyCameras = new TR4FlyByCamera[lvl.LevelDataChunk.NumFlybyCameras];

            for (int i = 0; i < lvl.LevelDataChunk.NumFlybyCameras; i++)
            {
                lvl.LevelDataChunk.FlybyCameras[i] = TR4FileReadUtilities.ReadFlybyCamera(reader);
            }
        }

        public static void PopulateSoundSources(BinaryReader reader, TR5Level lvl)
        {
            //Sound Sources
            lvl.LevelDataChunk.NumSoundSources = reader.ReadUInt32();
            lvl.LevelDataChunk.SoundSources = new TRSoundSource[lvl.LevelDataChunk.NumSoundSources];

            for (int i = 0; i < lvl.LevelDataChunk.NumSoundSources; i++)
            {
                lvl.LevelDataChunk.SoundSources[i] = TR2FileReadUtilities.ReadSoundSource(reader);
            }
        }

        public static void PopulateBoxesOverlapsZones(BinaryReader reader, TR5Level lvl)
        {
            //Boxes
            lvl.LevelDataChunk.NumBoxes = reader.ReadUInt32();
            lvl.LevelDataChunk.Boxes = new TR2Box[lvl.LevelDataChunk.NumBoxes];

            for (int i = 0; i < lvl.LevelDataChunk.NumBoxes; i++)
            {
                lvl.LevelDataChunk.Boxes[i] = TR2FileReadUtilities.ReadBox(reader);
            }

            //Overlaps & Zones
            lvl.LevelDataChunk.NumOverlaps = reader.ReadUInt32();
            lvl.LevelDataChunk.Overlaps = new ushort[lvl.LevelDataChunk.NumOverlaps];
            lvl.LevelDataChunk.Zones = new short[10 * lvl.LevelDataChunk.NumBoxes];

            for (int i = 0; i < lvl.LevelDataChunk.NumOverlaps; i++)
            {
                lvl.LevelDataChunk.Overlaps[i] = reader.ReadUInt16();
            }

            for (int i = 0; i < lvl.LevelDataChunk.Zones.Count(); i++)
            {
                lvl.LevelDataChunk.Zones[i] = reader.ReadInt16();
            }
        }

        public static void PopulateAnimatedTextures(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.NumAnimatedTextures = reader.ReadUInt32();
            lvl.LevelDataChunk.AnimatedTextures = new TRAnimatedTexture[reader.ReadUInt16()];
            for (int i = 0; i < lvl.LevelDataChunk.AnimatedTextures.Length; i++)
            {
                lvl.LevelDataChunk.AnimatedTextures[i] = TR2FileReadUtilities.ReadAnimatedTexture(reader);
            }

            //TR4+ Specific
            lvl.LevelDataChunk.AnimatedTexturesUVCount = reader.ReadByte();
        }

        public static void VerifyTEXMarker(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.TEXMarker = reader.ReadBytes(3);

            Debug.Assert(lvl.LevelDataChunk.TEXMarker[0] == 0x54);
            Debug.Assert(lvl.LevelDataChunk.TEXMarker[1] == 0x45);
            Debug.Assert(lvl.LevelDataChunk.TEXMarker[2] == 0x58);
        }

        public static void PopulateObjectTextures(BinaryReader reader, TR5Level lvl)
        {
            //Object Textures
            lvl.LevelDataChunk.NumObjectTextures = reader.ReadUInt32();
            lvl.LevelDataChunk.ObjectTextures = new TR4ObjectTexture[lvl.LevelDataChunk.NumObjectTextures];

            for (int i = 0; i < lvl.LevelDataChunk.NumObjectTextures; i++)
            {
                lvl.LevelDataChunk.ObjectTextures[i] = TR4FileReadUtilities.ReadObjectTexture(reader);
            }
        }

        public static void PopulateEntitiesAndAI(BinaryReader reader, TR5Level lvl)
        {
            //Entities
            lvl.LevelDataChunk.NumEntities = reader.ReadUInt32();
            lvl.LevelDataChunk.Entities = new TR4Entity[lvl.LevelDataChunk.NumEntities];

            for (int i = 0; i < lvl.LevelDataChunk.NumEntities; i++)
            {
                lvl.LevelDataChunk.Entities[i] = TR4FileReadUtilities.ReadEntity(reader);
            }

            //AIObjects
            lvl.LevelDataChunk.NumAIObjects = reader.ReadUInt32();
            lvl.LevelDataChunk.AIObjects = new TR4AIObject[lvl.LevelDataChunk.NumAIObjects];

            for (int i = 0; i < lvl.LevelDataChunk.NumAIObjects; i++)
            {
                lvl.LevelDataChunk.AIObjects[i] = TR4FileReadUtilities.ReadAIObject(reader);
            }
        }

        public static void PopulateDemoSoundSampleIndices(BinaryReader reader, TR5Level lvl)
        {
            //Demo Data
            lvl.LevelDataChunk.NumDemoData = reader.ReadUInt16();
            lvl.LevelDataChunk.DemoData = new byte[lvl.LevelDataChunk.NumDemoData];

            for (int i = 0; i < lvl.LevelDataChunk.NumDemoData; i++)
            {
                lvl.LevelDataChunk.DemoData[i] = reader.ReadByte();
            }

            //Sound Map (370 shorts) & Sound Details
            lvl.LevelDataChunk.SoundMap = new short[370];

            for (int i = 0; i < lvl.LevelDataChunk.SoundMap.Count(); i++)
            {
                lvl.LevelDataChunk.SoundMap[i] = reader.ReadInt16();
            }

            lvl.LevelDataChunk.NumSoundDetails = reader.ReadUInt32();
            lvl.LevelDataChunk.SoundDetails = new TR3SoundDetails[lvl.LevelDataChunk.NumSoundDetails];

            for (int i = 0; i < lvl.LevelDataChunk.NumSoundDetails; i++)
            {
                lvl.LevelDataChunk.SoundDetails[i] = TR3FileReadUtilities.ReadSoundDetails(reader);
            }

            //Samples
            lvl.LevelDataChunk.NumSampleIndices = reader.ReadUInt32();
            lvl.LevelDataChunk.SampleIndices = new uint[lvl.LevelDataChunk.NumSampleIndices];

            for (int i = 0; i < lvl.LevelDataChunk.NumSampleIndices; i++)
            {
                lvl.LevelDataChunk.SampleIndices[i] = reader.ReadUInt32();
            }
        }

        public static void VerifyLevelDataFinalSeperator(BinaryReader reader, TR5Level lvl)
        {
            lvl.LevelDataChunk.Seperator = reader.ReadBytes(6);

            Debug.Assert(lvl.LevelDataChunk.Seperator[0] == 0x00);
            Debug.Assert(lvl.LevelDataChunk.Seperator[1] == 0x00);
            Debug.Assert(lvl.LevelDataChunk.Seperator[2] == 0x00);
            Debug.Assert(lvl.LevelDataChunk.Seperator[3] == 0x00);
            Debug.Assert(lvl.LevelDataChunk.Seperator[4] == 0x00);
            Debug.Assert(lvl.LevelDataChunk.Seperator[5] == 0x00);
        }
    }
}
