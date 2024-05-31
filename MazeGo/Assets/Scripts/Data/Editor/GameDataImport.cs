using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using GKBase;

public class GameDataImport
{
    public static GameData LoadOrCreateGameData()
    {
        return GKEditor.LoadOrCreateAsset<GameData>("Assets/Resources/Data/_AutoGen_/GameData.asset");
    }

    public static void OnImportData(string filename)
    {
        var data = LoadOrCreateGameData();
        EditorUtility.SetDirty(data);

        var basename = System.IO.Path.GetFileName(filename);

        if (basename == "GameData_AchievementData.csv") { OnImportAchievementData(filename, data); return; }
        if (basename == "GameData_LocalizationData.csv") { OnImportLocalizationData(filename, data); return; }
        if (basename == "GameData_LocalizationErrorCodeData.csv") { OnImportLocalizationErrorCodeData(filename, data); return; }
        if (basename == "GameData_LocalizationAchiData.csv") { OnImportLocalizationAchiData(filename, data); return; }
        if (basename == "GameData_LocalizationAchiDescData.csv") { OnImportLocalizationAchiDescData(filename, data); return; }
        if (basename == "GameData_LocalizationTitleData.csv") { OnImportLocalizationTitleData(filename, data); return; }
        if (basename == "GameData_LocalizationTitleDescData.csv") { OnImportLocalizationTitleDescData(filename, data); return; }
        if (basename == "GameData_LocalizationMazeData.csv") { OnImportLocalizationMazeData(filename, data); return; }
            
    }
    static void OnImportAchievementData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetAchievementDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.AchievementData();
            p.RowToObject<GameData.AchievementData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._achievementData.Length)
                continue;

            data._achievementData[d.id] = d;
        }
    }

    static void OnImportLocalizationData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationData.Length)
                continue;

            data._localizationData[d.id] = d;
        }
    }

    static void OnImportLocalizationErrorCodeData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationErrorCodeDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationErrorCodeData.Length)
                continue;

            data._localizationErrorCodeData[d.id] = d;
        }
    }

    static void OnImportLocalizationAchiData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationAchiDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationAchiData.Length)
                continue;

            data._localizationAchiData[d.id] = d;
        }
    }

    static void OnImportLocalizationAchiDescData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationAchiDescDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationAchiDescData.Length)
                continue;

            data._localizationAchiDescData[d.id] = d;
        }
    }

    static void OnImportLocalizationTitleData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationTitleDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationTitleData.Length)
                continue;

            data._localizationTitleData[d.id] = d;
        }
    }

    static void OnImportLocalizationTitleDescData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationTitleDescDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationTitleDescData.Length)
                continue;

            data._localizationTitleDescData[d.id] = d;
        }
    }

    static void OnImportLocalizationMazeData(string filename, GameData data)
    {
        var p = GKCSVParser.OpenFile(filename, "#columns");
        if (p == null) return;

        int row = 0;

        // Calc valid lines.
        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            row++;
        }

        // Reset readIndex to 3.
        p.ResetReadIndex();
        // Init item data array.
        data.ResetLocalizationMazeBuffDataTypeArray(row);

        while (p.NextRow())
        {
            if (p.isRowStartWith("#")) continue;

            var d = new GameData.LocalizationData();
            p.RowToObject<GameData.LocalizationData>(ref d);

            if (null == d || d.id < 0 || d.id >= data._localizationMazeData.Length)
                continue;

            data._localizationMazeData[d.id] = d;
        }
    }
}
