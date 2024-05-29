using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GKBase;
using GKUI;
using UnityEngine.Rendering;

public class UIPassTimeItemSample : UIBase
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
    private int _lv;
    private int _passTime;
    private int _bestTime;
    #endregion

    #region PublicMethod
    public void SetData(int lv, int passTime, int bestTime)
    {
        _lv = lv;
        _passTime = passTime;
        _bestTime = bestTime;
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
        m_ctl.LvText.text = _lv.ToString();
        m_ctl.PassTimeText.text = string.Format("{0} : {1}", _passTime / 60, _passTime % 60);
        if (-1 != _bestTime)
            m_ctl.BestTimeText.text = string.Format("{0} : {1}", _bestTime / 60, _bestTime % 60);
        else
            m_ctl.BestTimeText.text = "------";
    }
    #endregion
}
