﻿using TREnvironmentEditor.Helpers;
using TRLevelControl.Model;
using TRModelTransporter.Transport;

namespace TREnvironmentEditor.Model.Types;

public class EMImportModelFunction : BaseEMFunction
{
    public List<short> Models { get; set; }

    public override void ApplyToLevel(TR1Level level)
    {
        TR1ModelImporter importer = new(Tags?.Contains(EMTag.CommunityPatchOnly) ?? false)
        {
            Level = level,
            EntitiesToImport = Models.Select(m => (TR1Type)m),
            DataFolder = @"Resources\TR1\Models"
        };

        importer.Import();
    }

    public override void ApplyToLevel(TR2Level level)
    {
        TR2ModelImporter importer = new()
        {
            Level = level,
            EntitiesToImport = Models.Select(m => (TR2Type)m),
            DataFolder = @"Resources\TR2\Models"
        };

        importer.Import();
    }

    public override void ApplyToLevel(TR3Level level)
    {
        TR3ModelImporter importer = new()
        {
            Level = level,
            EntitiesToImport = Models.Select(m => (TR3Type)m),
            DataFolder = @"Resources\TR3\Models"
        };

        importer.Import();
    }
}
