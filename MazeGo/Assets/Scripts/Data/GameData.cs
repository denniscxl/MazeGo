using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GKData;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameData : GKGameData
{
    #region InventoryUpgrade
    [System.Serializable]
    public class InventoryUpgradeData
    {
        public int id;
        public int coin;
        public int diamond;
    }
    [SerializeField]
    public InventoryUpgradeData[] _inventoryUpgradeData;
    public InventoryUpgradeData GetInventoryUpgradeData(int id)
    {
        if (id < 0 || id >= _inventoryUpgradeData.Length)
        {
            Debug.LogError(string.Format("Get inventory upgrade data faile. id: {0}", id));
            return null;
        }
        return _inventoryUpgradeData[id];
    }
#if UNITY_EDITOR
    public void InitInventoryUpgradeProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _inventoryUpgradeData[idx].id;
        p.FindPropertyRelative("coin").intValue = _inventoryUpgradeData[idx].coin;
        p.FindPropertyRelative("diamond").intValue = _inventoryUpgradeData[idx].diamond;
    }
    public void ResetInventoryUpgradeDataTypeArray(int length) { ResetDataArray<InventoryUpgradeData>(length, ref _inventoryUpgradeData); }
#endif
    #endregion

    #region AchievementData
    [System.Serializable]
    public class AchievementData
    {
        public int id;
        public int action;
        public int points;
        public int title;
        public List<int> parameter;
    }
    [SerializeField]
    public AchievementData[] _achievementData;
    public AchievementData GetAchievementData(int id)
    {
        if (id < 0 || id >= _achievementData.Length)
        {
            Debug.LogError(string.Format("Get achievement data faile. id: {0}", id));
            return null;
        }
        return _achievementData[id];
    }
#if UNITY_EDITOR
    public void InitAchievementProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _achievementData[idx].id;
        p.FindPropertyRelative("action").intValue = _achievementData[idx].action;
        p.FindPropertyRelative("points").intValue = _achievementData[idx].points;
        p.FindPropertyRelative("title").intValue = _achievementData[idx].title;
    }
    public void ResetAchievementDataTypeArray(int length) { ResetDataArray<AchievementData>(length, ref _achievementData); }
#endif
    #endregion

    #region MazeMonsterData
    [System.Serializable]
    public class MazeMonsterData
    {
        public int id;
        public int hp;
        public float speed;
    }
    [SerializeField]
    public MazeMonsterData[] _mazeMonsterData;
    public MazeMonsterData GetMazeMonsterData(int id)
    {
        if (id < 0 || id >= _mazeMonsterData.Length)
        {
            Debug.LogError(string.Format("Get maze monster data faile. id: {0}", id));
            return null;
        }
        return _mazeMonsterData[id];
    }
#if UNITY_EDITOR
    public void InitMazeMonsterProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _mazeMonsterData[idx].id;
        p.FindPropertyRelative("hp").intValue = _mazeMonsterData[idx].hp;
        p.FindPropertyRelative("speed").floatValue = _mazeMonsterData[idx].speed;
    }
    public void ResetMazeMonsterDataTypeArray(int length) { ResetDataArray<MazeMonsterData>(length, ref _mazeMonsterData); }
#endif
    #endregion

    #region MazeTownData
    [System.Serializable]
    public class MazeTownData
    {
        public int id;
        public int damage;
        public float atkSpeed;
        public float range;
        public int targetType;
        public float bulletSpeed;
        public float time;
        public float bulletRange;
        public int bulletType;
    }
    [SerializeField]
    public MazeTownData[] _mazeTownData;
    public MazeTownData GetMazeTownData(int id)
    {
        if (id < 0 || id >= _mazeTownData.Length)
        {
            Debug.LogError(string.Format("Get maze town data faile. id: {0}", id));
            return null;
        }
        return _mazeTownData[id];
    }
#if UNITY_EDITOR
    public void InitMazeTownProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _mazeTownData[idx].id;
        p.FindPropertyRelative("damage").intValue = _mazeTownData[idx].damage;
        p.FindPropertyRelative("atkSpeed").floatValue = _mazeTownData[idx].atkSpeed;
        p.FindPropertyRelative("range").floatValue = _mazeTownData[idx].range;
        p.FindPropertyRelative("targetType").intValue = _mazeTownData[idx].targetType;
        p.FindPropertyRelative("bulletSpeed").floatValue = _mazeTownData[idx].bulletSpeed;
        p.FindPropertyRelative("time").floatValue = _mazeTownData[idx].time;
        p.FindPropertyRelative("bulletRange").floatValue = _mazeTownData[idx].bulletRange;
        p.FindPropertyRelative("bulletType").intValue = _mazeTownData[idx].bulletType;
    }
    public void ResetMazeTownDataTypeArray(int length) { ResetDataArray<MazeTownData>(length, ref _mazeTownData); }
#endif
    #endregion

    #region Localization
    [System.Serializable]
    public class LocalizationData
    {
        public int id;
        public string english;
        public string chinese;
    }
    [SerializeField]
    public LocalizationData[] _localizationData;
    public LocalizationData GetLocalizationData(int id)
    {
        if (id < 0 || id >= _localizationData.Length)
        {
            Debug.LogError(string.Format("Get localization data faile. id: {0}", id));
            return null;
        }
        return _localizationData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationData[idx].chinese;
    }
    public void ResetLocalizationDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationData); }
#endif
    #endregion

    #region LocalizationErroeCode
    [SerializeField]
    public LocalizationData[] _localizationErrorCodeData;
    public LocalizationData GetLocalizationErrorCodeData(int id)
    {
        if (id < 0 || id >= _localizationErrorCodeData.Length)
        {
            Debug.LogError(string.Format("Get localization error code data faile. id: {0}", id));
            return null;
        }
        return _localizationErrorCodeData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationErrorCodeProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationErrorCodeData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationErrorCodeData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationErrorCodeData[idx].chinese;
    }
    public void ResetLocalizationErrorCodeDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationErrorCodeData); }
#endif
    #endregion

    #region LocalizationAchievement
    [SerializeField]
    public LocalizationData[] _localizationAchiData;
    public LocalizationData GetLocalizationAchiData(int id)
    {
        if (id < 0 || id >= _localizationAchiData.Length)
        {
            Debug.LogError(string.Format("Get localization achi data faile. id: {0}", id));
            return null;
        }
        return _localizationAchiData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationAchiProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationAchiData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationAchiData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationAchiData[idx].chinese;
    }
    public void ResetLocalizationAchiDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationAchiData); }
#endif
    #endregion

    #region LocalizationAchievementDescription
    [SerializeField]
    public LocalizationData[] _localizationAchiDescData;
    public LocalizationData GetLocalizationAchiDescData(int id)
    {
        if (id < 0 || id >= _localizationAchiDescData.Length)
        {
            Debug.LogError(string.Format("Get localization achievement description data faile. id: {0}", id));
            return null;
        }
        return _localizationAchiDescData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationAchiDescProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationAchiDescData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationAchiDescData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationAchiDescData[idx].chinese;
    }
    public void ResetLocalizationAchiDescDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationAchiDescData); }
#endif
    #endregion

    #region LocalizationTitle
    [SerializeField]
    public LocalizationData[] _localizationTitleData;
    public LocalizationData GetLocalizationTitleData(int id)
    {
        if (id < 0 || id >= _localizationTitleData.Length)
        {
            Debug.LogError(string.Format("Get localization title data faile. id: {0}", id));
            return null;
        }
        return _localizationTitleData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationTitleProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationTitleData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationTitleData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationTitleData[idx].chinese;
    }
    public void ResetLocalizationTitleDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationTitleData); }
#endif
    #endregion

    #region LocalizationTitleDescription
    [SerializeField]
    public LocalizationData[] _localizationTitleDescData;
    public LocalizationData GetLocalizationTitleDescData(int id)
    {
        if (id < 0 || id >= _localizationTitleDescData.Length)
        {
            Debug.LogError(string.Format("Get localization title description data faile. id: {0}", id));
            return null;
        }
        return _localizationTitleDescData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationTitleDescProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationTitleDescData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationTitleDescData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationTitleDescData[idx].chinese;
    }
    public void ResetLocalizationTitleDescDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationTitleDescData); }
#endif
    #endregion

    #region LocalizationMaze
    [SerializeField]
    public LocalizationData[] _localizationMazeData;
    public LocalizationData GetLocalizationMazeData(int id)
    {
        if (id < 0 || id >= _localizationMazeData.Length)
        {
            Debug.LogError(string.Format("Get localization Maze data faile. id: {0}", id));
            return null;
        }
        return _localizationMazeData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationMazeProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationMazeData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationMazeData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationMazeData[idx].chinese;
    }
    public void ResetLocalizationMazeDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationMazeData); }
#endif
    #endregion

    #region LocalizationMazeBuff
    [SerializeField]
    public LocalizationData[] _localizationMazeBuffData;
    public LocalizationData GetLocalizationMazeBuffData(int id)
    {
        if (id < 0 || id >= _localizationMazeBuffData.Length)
        {
            Debug.LogError(string.Format("Get localization Maze Buff data faile. id: {0}", id));
            return null;
        }
        return _localizationMazeBuffData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationMazeBuffProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationMazeBuffData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationMazeBuffData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationMazeBuffData[idx].chinese;
    }
    public void ResetLocalizationMazeBuffDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationMazeBuffData); }
#endif
    #endregion

    #region LocalizationMazeMonster
    [SerializeField]
    public LocalizationData[] _localizationMazeMonsterData;
    public LocalizationData GetLocalizationMazeMonsterData(int id)
    {
        if (id < 0 || id >= _localizationMazeMonsterData.Length)
        {
            Debug.LogError(string.Format("Get localization Maze Monster data faile. id: {0}", id));
            return null;
        }
        return _localizationMazeMonsterData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationMazeMonsterProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationMazeMonsterData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationMazeMonsterData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationMazeMonsterData[idx].chinese;
    }
    public void ResetLocalizationMazeMonsterDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationMazeMonsterData); }
#endif
    #endregion

    #region LocalizationMazeTown
    [SerializeField]
    public LocalizationData[] _localizationMazeTownData;
    public LocalizationData GetLocalizationMazeTownData(int id)
    {
        if (id < 0 || id >= _localizationMazeTownData.Length)
        {
            Debug.LogError(string.Format("Get localization Maze Town data faile. id: {0}", id));
            return null;
        }
        return _localizationMazeTownData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationMazeTownProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationMazeTownData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationMazeTownData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationMazeTownData[idx].chinese;
    }
    public void ResetLocalizationMazeTownDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationMazeTownData); }
#endif
    #endregion
}
