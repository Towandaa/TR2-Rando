﻿using TRGE.Core;
using TRLevelControl.Helpers;
using TRRandomizerCore.Globalisation;

namespace TRRandomizerCore.Randomizers;

public class TR1GameStringRandomizer : BaseTR1Randomizer
{
    private const int _maxLevelNameLength = 24;

    private G11N _g11n;
    private TR1GameStrings _gameStrings, _defaultGameStrings;

    public override void Randomize(int seed)
    {
        _generator = new Random(seed);
        _g11n = new G11N(G11NGame.TR1);
        _defaultGameStrings = _g11n.GetDefaultGameStrings() as TR1GameStrings;

        if (Settings.RandomizeGameStrings)
        {
            if (!Settings.GameStringLanguage.IsHybrid)
            {
                _gameStrings = _g11n.GetGameStrings(Settings.GameStringLanguage) as TR1GameStrings;
            }
            _defaultGameStrings = _g11n.GetDefaultGameStrings() as TR1GameStrings;

            TR1Script script = ScriptEditor.Script as TR1Script;

            ProcessGlobalStrings(script.Strings);
            ProcessLevelStrings(ScriptEditor.AssaultLevel);
            script.Strings["INV_ITEM_LARAS_HOME"] = ScriptEditor.AssaultLevel.Name;

            foreach (AbstractTRScriptedLevel level in ScriptEditor.ScriptedLevels)
            {
                ProcessLevelStrings(level);
            }
        }

        AmendDefaultStrings();

        SaveScript();

        TriggerProgress();
    }

    private void AmendDefaultStrings()
    {
        List<AbstractTRScriptedLevel> levels = ScriptEditor.Levels.ToList();
        AbstractTRScriptedLevel cistern = levels.Find(l => l.Is(TR1LevelNames.CISTERN));
        AbstractTRScriptedLevel mines = levels.Find(l => l.Is(TR1LevelNames.MINES));

        // Duplicate whatever Cistern has for "Rusty Key" into Mines
        mines.Keys.Add(cistern.Keys.Count > 2 ? cistern.Keys[2] : "Rusty Key");
    }

    private TR1GameStrings GetGameStrings()
    {
        // This allows for a hybrid language to be used, so each call will randomly pick another language.
        if (Settings.GameStringLanguage.IsHybrid)
        {
            Language[] availableLangs = _g11n.RealLanguages;
            return _g11n.GetGameStrings(availableLangs[_generator.Next(0, availableLangs.Length)]) as TR1GameStrings;
        }

        return _gameStrings;
    }

    private List<string> GetGlobalStrings(string key)
    {
        return GetGameStrings().GlobalStrings[key];
    }

    private TR1LevelStrings GetLevelStrings(string lvlName)
    {
        return GetGameStrings().LevelStrings[lvlName];
    }

    private void ProcessGlobalStrings(Dictionary<string, string> scriptStrings)
    {
        Dictionary<string, List<string>> defaultGlobalStrings = _defaultGameStrings.GlobalStrings;

        foreach (string stringKey in defaultGlobalStrings.Keys)
        {
            List<string> options = GetGlobalStrings(stringKey);
            scriptStrings[stringKey] = _defaultGameStrings.Encode(options[_generator.Next(0, options.Count)]);
        }
    }

    private void ProcessLevelStrings(AbstractTRScriptedLevel level)
    {
        string levelID = level.LevelFileBaseName.ToUpper();
        if (!_defaultGameStrings.LevelStrings.ContainsKey(levelID))
        {
            return;
        }

        TR1LevelStrings defaultLevelStrings = _defaultGameStrings.LevelStrings[levelID];

        if (!Settings.RetainLevelNames && defaultLevelStrings.Names != null && defaultLevelStrings.Names.Count > 0)
        {
            List<string> options = GetLevelStrings(levelID).Names;
            if (options.Any(o => o.Length <= _maxLevelNameLength))
            {
                string levelName;
                do
                {
                    levelName = options[_generator.Next(0, options.Count)];
                }
                while (levelName.Length > _maxLevelNameLength);

                level.Name = _defaultGameStrings.Encode(levelName);
            }
        }

        if (Settings.RetainKeyItemNames)
        {
            return;
        }

        for (int i = 0; i < level.Keys.Count; i++)
        {
            if (GenerateKeyItemName(levelID, "key" + (i + 1)) is string newName)
            {
                level.Keys[i] = newName;
            }
        }

        for (int i = 0; i < level.Pickups.Count; i++)
        {
            if (GenerateKeyItemName(levelID, "pickup" + (i + 1)) is string newName)
            {
                level.Pickups[i] = newName;
            }
        }

        for (int i = 0; i < level.Puzzles.Count; i++)
        {
            if (GenerateKeyItemName(levelID, "puzzle" + (i + 1)) is string newName)
            {
                level.Puzzles[i] = newName;
            }
        }
    }

    private string GenerateKeyItemName(string levelID, string keyName)
    {
        Dictionary<string, List<string>> optionMap = GetLevelStrings(levelID).KeyItems;
        if (!optionMap.ContainsKey(keyName))
        {
            return null;
        }

        List<string> options = optionMap[keyName];
        return _defaultGameStrings.Encode(options[_generator.Next(0, options.Count)]);
    }
}
