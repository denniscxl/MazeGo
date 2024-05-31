using System.Collections.Generic;
using UnityEngine;
using GKBase;
using GKData;
using GKUI;

public class PlayerController : GKSingleton<PlayerController>
{
    #region PublicField
    
    #endregion

    #region PrivateField
    private GKDataBase _data = new GKDataBase();
    #endregion

    #region PublicMethod
    public void Init()
    {
        _data.GetAttribute((int)EObjectAttr.Coins).OnAttrbutChangedEvent += OnAttrChanged;
        _data.GetAttribute((int)EObjectAttr.Diamond).OnAttrbutChangedEvent += OnAttrChanged;
    }

    public GKDataBase GetDataBase()
    {
        return _data;
    }
    public void SetDataBase(GKDataBase data)
    {
        _data = data;
    }
    #endregion

    #region PrivateMethod

    #endregion

    #region Langage
    // 语言切换.
    public System.Action OnLanguageChangedEvent = null;

    public int Language
    {
        get { return _data.GetAttribute((int)EObjectAttr.Language).ValInt; }
        set
        {
            _data.SetAttribute((int)EObjectAttr.Language, value, true);
            DataController.Instance().SavePlayerData();
            if (null != OnLanguageChangedEvent)
                OnLanguageChangedEvent();
        }
    }
    #endregion

    #region BaseAttr
    public int Coin
    {
        get { return _data.GetAttribute((int)EObjectAttr.Coins).ValInt; }
        set
        {
            _data.SetAttribute((int)EObjectAttr.Coins, value, true);
            DataController.Instance().SavePlayerData();
        }
    }
    public int Diamond
    {
        get { return _data.GetAttribute((int)EObjectAttr.Diamond).ValInt; }
        set
        {
            _data.SetAttribute((int)EObjectAttr.Diamond, value, true);
            DataController.Instance().SavePlayerData();
        }
    }

    // 消耗量统计.
    private void OnAttrChanged(object obj, GKCommonValue attr)
    {
        //Debug.Log("PlayerController OnAttrChanged");

        if (null != attr)
        {
            // 计算增值.
            int count = attr.ValInt - attr.LastValInt;
            if (count >= 0)
                return;
            // 更新成就数据.
            AchievementController.Instance().UpdateAchievementCount((EObjectAttr)attr.index, -count);
        }
    }
    #endregion

}
