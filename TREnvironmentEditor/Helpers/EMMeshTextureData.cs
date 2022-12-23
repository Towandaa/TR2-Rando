﻿using System.Collections.Generic;
using System.ComponentModel;

namespace TREnvironmentEditor.Helpers
{
    public class EMMeshTextureData
    {
        public short ModelID { get; set; }
        [DefaultValue(-1)]
        public int TexturedFace3 { get; set; }
        [DefaultValue(-1)]
        public int TexturedFace4 { get; set; }
        [DefaultValue(-1)]
        public int ColouredFace3 { get; set; }
        [DefaultValue(-1)]
        public int ColouredFace4 { get; set; }

        // Allow specific original textures to be replaced with others. If not
        // defined here, the defaults above will be used.
        public Dictionary<ushort, ushort> TextureMap { get; set; }
    }
}
