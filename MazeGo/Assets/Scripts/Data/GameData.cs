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

    #region LocalizationMazeDescription
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
    public void InitLocalizationMazeBuffProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationMazeData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationMazeData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationMazeData[idx].chinese;
    }
    public void ResetLocalizationMazeBuffDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationMazeData); }
#endif
    #endregion
}
