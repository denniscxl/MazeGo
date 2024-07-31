using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GKBase;
using GKData;

public class ConfigController : GKSingleton<ConfigController>
{
    #region PublicField
    #endregion

    #region PrivateField
    #endregion

    #region PublicMethod
    
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

    public Sprite GetUISprite(string  spritePath)
    {
        var go = GK.LoadPrefab("UI/Sprites/" + spritePath);
        if (null == go)
            Debug.LogError(string.Format("GetUISprite Faile. path: {0}", "UI/Sprites/" + spritePath));

        var sprite = go.GetComponent<SpriteRenderer>();
        if (null == go && null == sprite)
            return null;
        return sprite.sprite;
    }

    // 获取异常文本信息
    public string GetErrorCode(ErrorCodeType id)
    {
        return DataController.Instance().GetLocalization((int)id, LocalizationSubType.ErrorCode);
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
