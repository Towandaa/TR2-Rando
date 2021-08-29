﻿using System.Collections.Generic;
using TREnvironmentEditor.Helpers;
using TRFDControl;
using TRFDControl.FDEntryTypes;
using TRFDControl.Utilities;
using TRLevelReader.Model;

namespace TREnvironmentEditor.Model.Types
{
    public class EMDuplicateTriggerFunction : BaseEMFunction
    {
        public List<EMLocation> Locations { get; set; }
        public EMLocation BaseLocation { get; set; }

        public override void ApplyToLevel(TR2Level level)
        {
            FDControl control = new FDControl();
            control.ParseFromLevel(level);

            TRRoomSector baseSector = FDUtilities.GetRoomSector(BaseLocation.X, BaseLocation.Y, BaseLocation.Z, BaseLocation.Room, level, control);
            if (baseSector.FDIndex == 0)
            {
                return;
            }

            List<FDEntry> triggerEntries = control.Entries[baseSector.FDIndex].FindAll(e => e is FDTriggerEntry);
            if (triggerEntries.Count == 0)
            {
                return;
            }

            foreach (EMLocation location in Locations)
            {
                TRRoomSector sector = FDUtilities.GetRoomSector(location.X, location.Y, location.Z, location.Room, level, control);
                if (sector.FDIndex == 0)
                {
                    control.CreateFloorData(sector);
                }

                List<FDEntry> entries = control.Entries[sector.FDIndex];
                if (entries.FindIndex(e => e is FDTriggerEntry) == -1)
                {
                    entries.AddRange(triggerEntries);
                }
            }

            control.WriteToLevel(level);
        }
    }
}