﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GKData;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameData : GKGameData
{
    #region Store
    [System.Serializable]
    public class StoreData
    {
        public int id;
        public int goldPay;
        public int goldEarnings;
        public int diamondPay;
        public int diamondEarnings;
        public int itemPay;
        public int itemEarnings;
    }
    [SerializeField]
    public StoreData[] _storeData;
    public StoreData GetStoreData(int id)
    {
        if (id < 0 || id >= _storeData.Length)
        {
            Debug.LogError(string.Format("Get store data faile. id: {0}", id));
            return null;
        }
        return _storeData[id];
    }
#if UNITY_EDITOR
    public void InitStoreProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _storeData[idx].id;
        p.FindPropertyRelative("goldPay").intValue = _storeData[idx].goldPay;
        p.FindPropertyRelative("goldEarnings").intValue = _storeData[idx].goldEarnings;
        p.FindPropertyRelative("diamondPay").intValue = _storeData[idx].diamondPay;
        p.FindPropertyRelative("diamondEarnings").intValue = _storeData[idx].diamondEarnings;
        p.FindPropertyRelative("itemPay").intValue = _storeData[idx].itemPay;
        p.FindPropertyRelative("itemEarnings").intValue = _storeData[idx].itemEarnings;
    }
    public void ResetStoreDataTypeArray(int length) { ResetDataArray<StoreData>(length, ref _storeData); }
#endif
    #endregion

    #region Lottery
    [System.Serializable]
    public class LotteryData
    {
        public int id;
        public int coin;
        public int diamond;
    }
    [SerializeField]
    public LotteryData[] _lotteryData;
    public LotteryData GetLotteryData(int id)
    {
        if (id < 0 || id >= _lotteryData.Length)
        {
            Debug.LogError(string.Format("Get lottery data faile. id: {0}", id));
            return null;
        }
        return _lotteryData[id];
    }
#if UNITY_EDITOR
    public void InitLotteryProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _lotteryData[idx].id;
        p.FindPropertyRelative("coin").intValue = _lotteryData[idx].coin;
        p.FindPropertyRelative("diamond").intValue = _lotteryData[idx].diamond;
    }
    public void ResetLotteryDataTypeArray(int length) { ResetDataArray<LotteryData>(length, ref _lotteryData); }
#endif
    #endregion

    #region Equipment
    [System.Serializable]
    public class EquipmentData
    {
        public int id;
        public int name;
        public int part;
        public string job;
        public int strength;
        public int agility;
        public int intelligence;
        public int skillEffectA;
        public int skillEffectB;
        public int skillEffectC;
        public int description;
    }
    [SerializeField]
    public EquipmentData[] _equipmentData;
    public EquipmentData GetEquipmentData(int id)
    {
        if (id < 0 || id >= _equipmentData.Length)
        {
            Debug.LogError(string.Format("Get equipment data faile. id: {0}", id));
            return null;
        }
        return _equipmentData[id];
    }
#if UNITY_EDITOR
    public void InitEquipmentProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _equipmentData[idx].id;
        p.FindPropertyRelative("name").intValue = _equipmentData[idx].name;
        p.FindPropertyRelative("part").intValue = _equipmentData[idx].part;
        p.FindPropertyRelative("job").stringValue = _equipmentData[idx].job;
        p.FindPropertyRelative("strength").intValue = _equipmentData[idx].strength;
        p.FindPropertyRelative("agility").intValue = _equipmentData[idx].agility;
        p.FindPropertyRelative("intelligence").intValue = _equipmentData[idx].intelligence;
        p.FindPropertyRelative("skillEffectA").intValue = _equipmentData[idx].skillEffectA;
        p.FindPropertyRelative("skillEffectB").intValue = _equipmentData[idx].skillEffectB;
        p.FindPropertyRelative("skillEffectC").intValue = _equipmentData[idx].skillEffectC;
        p.FindPropertyRelative("description").intValue = _equipmentData[idx].description;
    }
    public void ResetEquipmentDataTypeArray(int length) { ResetDataArray<EquipmentData>(length, ref _equipmentData); }
#endif
    #endregion

    #region ConsumeData
    [System.Serializable]
    public class ConsumeData
    {
        public int id;
        public int name;
        public List<int> effect;
        public int description;
    }
    [SerializeField]
    public ConsumeData[] _consumeData;
    public ConsumeData GetConsumeData(int id)
    {
        if (id < 0 || id >= _consumeData.Length)
        {
            Debug.LogError(string.Format("Get consume data faile. id: {0}", id));
            return null;
        }
        return _consumeData[id];
    }
#if UNITY_EDITOR
    public void InitConsumeProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _consumeData[idx].id;
        p.FindPropertyRelative("name").intValue = _consumeData[idx].name;
        p.FindPropertyRelative("description").intValue = _consumeData[idx].description;
    }
    public void ResetConsumeDataTypeArray(int length) { ResetDataArray<ConsumeData>(length, ref _consumeData); }
#endif
    #endregion

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

    #region LocalizationUnit
    [SerializeField]
    public LocalizationData[] _localizationUnitData;
    public LocalizationData GetLocalizationUnitData(int id)
    {
        if (id < 0 || id >= _localizationUnitData.Length)
        {
            Debug.LogError(string.Format("Get localization unit data faile. id: {0}", id));
            return null;
        }
        return _localizationUnitData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationUnitProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationUnitData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationUnitData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationUnitData[idx].chinese;
    }
    public void ResetLocalizationUnitDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationUnitData); }
#endif
    #endregion

    #region LocalizationItem
    [SerializeField]
    public LocalizationData[] _localizationItemData;
    public LocalizationData GetLocalizationItemData(int id)
    {
        if (id < 0 || id >= _localizationItemData.Length)
        {
            Debug.LogError(string.Format("Get localization item data faile. id: {0}", id));
            return null;
        }
        return _localizationItemData[id];
    }
#if UNITY_EDITOR
    public void InitLocalizationItemProperty(ref SerializedProperty p, int idx)
    {
        p.FindPropertyRelative("id").intValue = _localizationItemData[idx].id;
        p.FindPropertyRelative("english").stringValue = _localizationItemData[idx].english;
        p.FindPropertyRelative("chinese").stringValue = _localizationItemData[idx].chinese;
    }
    public void ResetLocalizationItemDataTypeArray(int length) { ResetDataArray<LocalizationData>(length, ref _localizationItemData); }
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

}
