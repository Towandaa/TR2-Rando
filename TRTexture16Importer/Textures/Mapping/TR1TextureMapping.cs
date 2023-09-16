﻿using System.Drawing;
using TRLevelControl.Helpers;
using TRLevelControl.Model;
using TRLevelControl.Model.Enums;
using TRTexture16Importer.Helpers;

namespace TRTexture16Importer.Textures;

public class TR1TextureMapping : AbstractTextureMapping<TREntities, TR1Level>
{
    public TRPalette8Control PaletteManager { get; set; }

    protected TR1TextureMapping(TR1Level level)
        : base(level) { }

    public static TR1TextureMapping Get(TR1Level level, string mappingFilePrefix, TR1TextureDatabase database, Dictionary<StaticTextureSource<TREntities>, List<StaticTextureTarget>> predefinedMapping = null, List<TREntities> entitiesToIgnore = null, Dictionary<TREntities, TREntities> entityMap = null)
    {
        string mapFile = Path.Combine(@"Resources\TR1\Textures\Mapping\", mappingFilePrefix + "-Textures.json");
        if (!File.Exists(mapFile))
        {
            return null;
        }

        TR1TextureMapping mapping = new(level);
        LoadMapping(mapping, mapFile, database, predefinedMapping, entitiesToIgnore);
        mapping.EntityMap = entityMap;
        return mapping;
    }

    protected override List<TRColour> GetPalette8()
    {
        return _level.Palette;
    }

    protected override List<TRColour4> GetPalette16()
    {
        return null;
    }

    protected override int ImportColour(Color colour)
    {
        PaletteManager ??= new()
        {
            Level = _level
        };
        return PaletteManager.AddPredefinedColour(colour);
    }

    protected override TRMesh[] GetModelMeshes(TREntities entity)
    {
        return TRMeshUtilities.GetModelMeshes(_level, entity);
    }

    protected override TRSpriteSequence[] GetSpriteSequences()
    {
        return _level.SpriteSequences;
    }

    protected override TRSpriteTexture[] GetSpriteTextures()
    {
        return _level.SpriteTextures;
    }

    protected override Bitmap GetTile(int tileIndex)
    {
        return _level.Images8[tileIndex].ToBitmap(_level.Palette);
    }

    protected override void SetTile(int tileIndex, Bitmap bitmap)
    {
        PaletteManager ??= new()
        {
            Level = _level
        };
        PaletteManager.ChangedTiles[tileIndex] = bitmap;
    }

    public override void CommitGraphics()
    {
        if (!_committed)
        {
            foreach (int tile in _tileMap.Keys)
            {
                SetTile(tile, _tileMap[tile].Bitmap);
            }

            PaletteManager?.MergeTiles();

            foreach (int tile in _tileMap.Keys)
            {
                _tileMap[tile].Dispose();
            }

            _committed = true;
        }
    }
}
