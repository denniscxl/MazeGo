﻿using UnityEngine;
using GKBase;
using GKUI;

public class UILotteryItemSample : UIBase
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {

    }
    #endregion

    #region PublicField
    public LotteryType type = LotteryType.Coin;
    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    #endregion

    #region PublicMethod

    #endregion

    #region PrivateMethod
    private void Start()
    {
        Serializable();
        InitListener();
        Init();
    }

    private void Serializable()
    {
        GK.FindControls(this.gameObject, ref m_ctl);
    }

    private void InitListener()
    {
        //GKUIEventTriggerListener.Get(gameObject).onClick = OnClick;
    }

    private void Init()
    {

    }

    public void OnClick(GameObject go)
    {
        UILotteryNormal.Open().SetData(type);
    }
    #endregion
}