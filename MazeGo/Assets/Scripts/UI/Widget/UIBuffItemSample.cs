using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GKBase;
using GKUI;

public class UIBuffItemSample : UIBase
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Text LvText;
        public Text PassTimeText;
        public Text BestTimeText;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    #endregion

    #region PublicMethod
    public void SetData(int lv, int passTime, int bestTime)
    {
        m_ctl.LvText.text = lv.ToString();
        m_ctl.PassTimeText.text = string.Format("{0} : {1}", passTime / 60, passTime % 60);
        if (-1 != bestTime)
            m_ctl.BestTimeText.text = string.Format("{0} : {1}", passTime / 60, passTime % 60);
        else
            m_ctl.BestTimeText.text = "------";
    }
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

    }

    private void Init()
    {
       
    }
    #endregion
}
