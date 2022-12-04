﻿using System;
using System.Collections.Generic;
using TREnvironmentEditor.Helpers;
using TRLevelReader.Model;

namespace TREnvironmentEditor.Model.Types
{
    public class EMCopyVertexAttributesFunction : BaseEMFunction
    {
        public Dictionary<short, Dictionary<EMTextureFaceType, Dictionary<int, int>>> FaceMap { get; set; }
        public Dictionary<short, TR3RoomVertex> RoomMap { get; set; }

        public override void ApplyToLevel(TRLevel level)
        {
            EMLevelData data = GetData(level);

            // Allow changes per face
            if (FaceMap != null)
            {
                foreach (short roomNumber in FaceMap.Keys)
                {
                    TRRoom room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (EMTextureFaceType faceType in FaceMap[roomNumber].Keys)
                    {
                        foreach (int baseFaceIndex in FaceMap[roomNumber][faceType].Keys)
                        {
                            ushort[] baseVertices, copyVertices;
                            switch (faceType)
                            {
                                case EMTextureFaceType.Rectangles:
                                    baseVertices = room.RoomData.Rectangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Rectangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                case EMTextureFaceType.Triangles:
                                    baseVertices = room.RoomData.Triangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Triangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }

                            for (int i = 0; i < baseVertices.Length; i++)
                            {
                                TRRoomVertex baseVertex = room.RoomData.Vertices[baseVertices[i]];
                                TRRoomVertex copyVertex = room.RoomData.Vertices[copyVertices[i]];
                                CopyAttributes(baseVertex, copyVertex);
                            }
                        }
                    }
                }
            }

            // Allow entire room changes
            if (RoomMap != null)
            {
                foreach (short roomNumber in RoomMap.Keys)
                {
                    TRRoom room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (TRRoomVertex copyVertex in room.RoomData.Vertices)
                    {
                        CopyAttributes(RoomMap[roomNumber], copyVertex);
                    }
                }
            }
        }

        public override void ApplyToLevel(TR2Level level)
        {
            EMLevelData data = GetData(level);

            // Allow changes per face
            if (FaceMap != null)
            {
                foreach (short roomNumber in FaceMap.Keys)
                {
                    TR2Room room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (EMTextureFaceType faceType in FaceMap[roomNumber].Keys)
                    {
                        foreach (int baseFaceIndex in FaceMap[roomNumber][faceType].Keys)
                        {
                            ushort[] baseVertices, copyVertices;
                            switch (faceType)
                            {
                                case EMTextureFaceType.Rectangles:
                                    baseVertices = room.RoomData.Rectangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Rectangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                case EMTextureFaceType.Triangles:
                                    baseVertices = room.RoomData.Triangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Triangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }

                            for (int i = 0; i < baseVertices.Length; i++)
                            {
                                TR2RoomVertex baseVertex = room.RoomData.Vertices[baseVertices[i]];
                                TR2RoomVertex copyVertex = room.RoomData.Vertices[copyVertices[i]];
                                CopyAttributes(baseVertex, copyVertex);
                            }
                        }
                    }
                }
            }

            // Allow entire room changes
            if (RoomMap != null)
            {
                foreach (short roomNumber in RoomMap.Keys)
                {
                    TR2Room room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (TR2RoomVertex copyVertex in room.RoomData.Vertices)
                    {
                        CopyAttributes(RoomMap[roomNumber], copyVertex);
                    }
                }
            }
        }

        public override void ApplyToLevel(TR3Level level)
        {
            EMLevelData data = GetData(level);

            // Allow changes per face
            if (FaceMap != null)
            {
                foreach (short roomNumber in FaceMap.Keys)
                {
                    TR3Room room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (EMTextureFaceType faceType in FaceMap[roomNumber].Keys)
                    {
                        foreach (int baseFaceIndex in FaceMap[roomNumber][faceType].Keys)
                        {
                            ushort[] baseVertices, copyVertices;
                            switch (faceType)
                            {
                                case EMTextureFaceType.Rectangles:
                                    baseVertices = room.RoomData.Rectangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Rectangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                case EMTextureFaceType.Triangles:
                                    baseVertices = room.RoomData.Triangles[baseFaceIndex].Vertices;
                                    copyVertices = room.RoomData.Triangles[FaceMap[roomNumber][faceType][baseFaceIndex]].Vertices;
                                    break;
                                default:
                                    throw new ArgumentException();
                            }

                            for (int i = 0; i < baseVertices.Length; i++)
                            {
                                TR3RoomVertex baseVertex = room.RoomData.Vertices[baseVertices[i]];
                                TR3RoomVertex copyVertex = room.RoomData.Vertices[copyVertices[i]];
                                CopyAttributes(baseVertex, copyVertex);
                            }
                        }
                    }
                }
            }

            // Allow entire room changes
            if (RoomMap != null)
            {
                foreach (short roomNumber in RoomMap.Keys)
                {
                    TR3Room room = level.Rooms[data.ConvertRoom(roomNumber)];
                    foreach (TR3RoomVertex copyVertex in room.RoomData.Vertices)
                    {
                        CopyAttributes(RoomMap[roomNumber], copyVertex);
                    }
                }
            }
        }

        // TR3RoomVertex is a placeholder in the data to cover all levels
        private void CopyAttributes(TR3RoomVertex baseVertex, TRRoomVertex copyVertex)
        {
            copyVertex.Lighting = baseVertex.Lighting;
        }

        private void CopyAttributes(TRRoomVertex baseVertex, TRRoomVertex copyVertex)
        {
            copyVertex.Lighting = baseVertex.Lighting;
        }

        private void CopyAttributes(TR3RoomVertex baseVertex, TR2RoomVertex copyVertex)
        {
            copyVertex.Lighting = baseVertex.Lighting;
            copyVertex.Attributes = baseVertex.Attributes;
        }

        private void CopyAttributes(TR2RoomVertex baseVertex, TR2RoomVertex copyVertex)
        {
            copyVertex.Lighting = baseVertex.Lighting;
            copyVertex.Attributes = baseVertex.Attributes;
        }

        private void CopyAttributes(TR3RoomVertex baseVertex, TR3RoomVertex copyVertex)
        {
            copyVertex.Attributes = baseVertex.Attributes;
            copyVertex.Colour = baseVertex.Colour;
            copyVertex.Lighting = baseVertex.Lighting;
        }
    }
}