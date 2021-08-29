﻿using System;
using System.Collections.Generic;
using TR2RandomizerCore.Helpers;
using TREnvironmentEditor;
using TREnvironmentEditor.Model;
using TRGE.Core;

namespace TR2RandomizerCore.Randomizers
{
    public class EnvironmentRandomizer : RandomizerBase
    {
        public bool EnforcedModeOnly { get; set; }
        public bool RandomizeWater { get; set; }
        public bool RandomizeSlots { get; set; }

        private List<EMType> _disallowedTypes;

        public override void Randomize(int seed)
        {
            _generator = new Random(seed);

            _disallowedTypes = new List<EMType>();
            if (!RandomizeWater)
            {
                _disallowedTypes.Add(EMType.Flood);
                _disallowedTypes.Add(EMType.Drain);
            }
            if (!RandomizeSlots)
            {
                _disallowedTypes.Add(EMType.MoveSlot);
            }

            foreach (TR23ScriptedLevel lvl in Levels)
            {
                LoadLevelInstance(lvl);

                RandomizeEnvironment(_levelInstance);

                SaveLevelInstance();

                if (!TriggerProgress())
                {
                    break;
                }
            }
        }

        private void RandomizeEnvironment(TR2CombinedLevel level)
        {
            EMEditorMapping mapping = EMEditorMapping.Get(level.Name);
            if (mapping == null)
            {
                return;
            }

            // Process enforced packs first. We do not pass disallowed types here.
            mapping.All.ApplyToLevel(level.Data, new EMType[] { });

            if (EnforcedModeOnly)
            {
                return;
            }

            if (mapping.Any.Count > 0)
            {
                // Pick a random number of packs to apply, but at least 1
                int packCount = _generator.Next(1, mapping.Any.Count + 1);
                List<EMEditorSet> randomSet = mapping.Any.RandomSelection(_generator, packCount);
                foreach (EMEditorSet mod in randomSet)
                {
                    mod.ApplyToLevel(level.Data, _disallowedTypes);
                }
            }

            // AllWithin means one from each set will be applied. Used for the likes of choosing a new
            // keyhole position from a set.
            if (mapping.AllWithin.Count > 0)
            {
                foreach (List<EMEditorSet> modList in mapping.AllWithin)
                {
                    EMEditorSet mod = modList[_generator.Next(0, modList.Count)];
                    mod.ApplyToLevel(level.Data, _disallowedTypes);
                }
            }

            // OneOf is used for a leader-follower situation, but where only one follower from
            // a group is wanted. An example is removing a ladder (the leader) and putting it in 
            // a different position, so the followers are the different positions from which we pick one.
            if (mapping.OneOf.Count > 0)
            {
                foreach (EMEditorGroupedSet mod in mapping.OneOf)
                {
                    EMEditorSet follower = mod.Followers[_generator.Next(0, mod.Followers.Length)];
                    mod.ApplyToLevel(level.Data, follower, _disallowedTypes);
                }
            }
        }
    }
}