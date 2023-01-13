﻿using System.Collections.Generic;
using TREnvironmentEditor.Helpers;
using TRLevelReader.Model;

namespace TREnvironmentEditor.Model
{
    public class EMConditionalSingleEditorSet : ITextureModifier
    {
        public BaseEMCondition Condition { get; set; }
        public EMEditorSet OnTrue { get; set; }
        public EMEditorSet OnFalse { get; set; }

        public void ApplyToLevel(TRLevel level, EMOptions options = null)
        {
            EMEditorSet edits = Condition.GetResult(level) ? OnTrue : OnFalse;
            if (edits != null)
            {
                edits.ApplyToLevel(level, options);
            }
        }

        public void ApplyToLevel(TR2Level level, EMOptions options = null)
        {
            EMEditorSet edits = Condition.GetResult(level) ? OnTrue : OnFalse;
            if (edits != null)
            {
                edits.ApplyToLevel(level, options);
            }
        }

        public void ApplyToLevel(TR3Level level, EMOptions options = null)
        {
            EMEditorSet edits = Condition.GetResult(level) ? OnTrue : OnFalse;
            if (edits != null)
            {
                edits.ApplyToLevel(level, options);
            }
        }

        public void RemapTextures(Dictionary<ushort, ushort> indexMap)
        {
            if (OnTrue != null)
            {
                OnTrue.RemapTextures(indexMap);
            }
            if (OnFalse != null)
            {
                OnFalse.RemapTextures(indexMap);
            }
        }
    }
}