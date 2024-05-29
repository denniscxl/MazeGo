using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GKBase;
using GKData;

public class ConfigController : GKSingleton<ConfigController>
{
    #region PublicField
    static public readonly int unitCount = 6;
    static public readonly int equipmentCount = 64;
    static public readonly int consumeCount = 11;
    #endregion

    #region PrivateField
    #endregion

    #region PublicMethod
    

    // 获取商店商品数据.
    public void GetStoreItemData(UIStoreClassType type, int level, out int pay, out int earnings)
    {
        pay = 0;
        earnings = 0;
        // level - 1 == id.
        var store = DataController.Data.GetStoreData(level-1);
        if (null == store)
            return;
        
        switch(type)
        {
            case UIStoreClassType.Conin:
                pay = store.goldPay;
                earnings = store.goldEarnings;
                break;
            case UIStoreClassType.Diamond:
                pay = store.diamondPay;
                earnings = store.diamondEarnings;
                break;
            case UIStoreClassType.Item:
                pay = store.itemPay;
                earnings = store.itemEarnings;
                break;
        }
    }

    public string GetSpriteName(EObjectAttr type)
    {
        switch(type)
        {
            case EObjectAttr.Coins:
                return "Icon/coin";
            case EObjectAttr.Diamond:
                return "Icon/diamond";
        }
        return "";
    }

    // 获取移动图标.
    public Sprite GetMoveTypeSprite(int id)
    {
        return GetUISprite("MoveType/" + id.ToString());
    }

    // 获取技能图标.
    public Sprite GetSkillSprite(int id)
    {
        return GetUISprite("Skill/" + id.ToString());
    }

    //  获取装备图标.
    public Sprite GetEquipmentSprite(int id)
    {
        return GetUISprite("Equipment/" + id.ToString());
    } 

    //  获取消耗品图标.
    public Sprite GetConsumeSprite(int id)
    {
        return GetUISprite("Consume/" + id.ToString());
    } 

    public Sprite GetUISprite(string  spritePath)
    {
        var go = GK.LoadPrefab("UI/Sprites/" + spritePath);
        var sprite = go.GetComponent<SpriteRenderer>();
        if (null == go && null == sprite)
            return null;
        return sprite.sprite;
    }

    public Texture2D GetCardIconTexture(int id)
    {
        return GK.LoadTexture2D(string.Format("CardIcons/{0}", id));
    }

    // 获取异常文本信息
    public string GetErrorCode(ErrorCodeType id)
    {
        return DataController.Instance().GetLocalization((int)id, LocalizationSubType.ErrorCode);
    }

    // 通过名称获取物品名称.
    public string GetItemNameByType(int type, int id)
    {
        string name = string.Empty;
        switch (type)
        {
            case (int)ItemType.Consume:
                var consume = DataController.Data.GetConsumeData(id);
                if (null != consume)
                {
                    name = DataController.Instance().GetLocalization(consume.name, LocalizationSubType.Item);
                }
                break;
            case (int)ItemType.Equipment:
                var equipment = DataController.Data.GetEquipmentData(id);
                if (null != equipment)
                {
                    name = DataController.Instance().GetLocalization(equipment.name, LocalizationSubType.Item);
                }
                break;
        }
        return name;
    }

    #region HUDText
    // 显示伤害/回复值.
    public void ShowDamageText(int val, Transform target)
    {
        bool isSub = (val <= 0);
        string sub = isSub ? "-" : "+";
        HUDTextInfo info = new HUDTextInfo(target, string.Format("{1}{0}", Mathf.Abs(val), sub));
        info.Color = isSub ? Color.red : Color.green;
        InitTextInfo(info);
        MyGame.HUDText.NewText(info);
    }

    // 显示格挡文字.
    public void ShowDoge(Transform target)
    {
        HUDTextInfo info = new HUDTextInfo(target, string.Format("{0}", DataController.Instance().GetLocalization(138)));
        info.Color = Color.yellow;
        InitTextInfo(info);
        MyGame.HUDText.NewText(info);
    }

    private void InitTextInfo(HUDTextInfo info)
    {
        info.Size = 40;
        info.Speed = 0.6f;
        info.VerticalAceleration = 0;
        info.VerticalPositionOffset = 2;
        info.VerticalFactorScale = 2;
        info.Side = (Random.Range(0, 2) == 1) ? bl_Guidance.LeftDown : bl_Guidance.RightDown;
        info.ExtraDelayTime = -1;
        info.AnimationType = bl_HUDText.TextAnimationType.PingPong;
        info.FadeSpeed = 100;
        info.ExtraFloatSpeed = -11;
        info.AnimationSpeed = 0.1f;
    }
    #endregion

    #endregion

    #region PrivateMethod
    #endregion
}

public enum ErrorCodeType
{
    CardDataMissing = 0,                // 卡牌数据丢失.
    EquipmentDataMissing,               // 装备数据丢失.
    SkillDataMissing,                   // 技能数据丢失.
    InventoryFull,                      // 背包已满.
    JobMismatching,                     // 职业不匹配.
    CoinNotEnough,                      // 金币不足.
    DiamondNotEnough,                   // 钻石不足.
    BeliefNotEnough,                    // 信仰不足.
    FoodNotEnough,                      // 食物不足.
    SkillPointNotEnough,                // 技能点数不足.
    DependentSkillLevelNotEnough,       // 依赖技能等级不足.
    AttributeNotEnough,                 // 属性不足.
    MaxLevel,                           // 已经为最高等级.
}
