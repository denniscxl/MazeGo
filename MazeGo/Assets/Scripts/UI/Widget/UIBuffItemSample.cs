using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GKBase;
using GKUI;
using Unity.VisualScripting;

public class UIBuffItemSample : UIBase
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Image Icon;
        public Text NameText;
        public Button SelectBtn;
        public Image Highlight;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    private MazeBuffType _buffType;
    private int _idx = 0;
    #endregion

    #region PublicMethod
    public void SetData(MazeBuffType t, int idx)
    {
        _buffType = t;
        _idx = idx;
    }

    public void UpdateHighlight(bool bShow)
    {
        if (m_ctl != null)
            m_ctl.Highlight.gameObject.SetActive(bShow);
    }

    public MazeBuffType GetBuffType()
    { 
        return _buffType; 
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
        GKUIEventTriggerListener.Get(m_ctl.SelectBtn.gameObject).onClick = OnSelected;
    }

    private void Init()
    {
        string[] imgs = GK.EnumNames<MazeBuffType>();
        if (0 <= (int)_buffType && (int)_buffType < imgs.Length)
        {
            m_ctl.Icon.sprite = GetBuffTypeSprite(imgs[(int)_buffType]);
            //m_ctl.NameText.text = imgs[(int)_buffType];
            m_ctl.NameText.text = DataController.Instance().GetLocalization((int)_buffType, LocalizationSubType.MazeBuff);
        }
    }

    private void OnSelected(GameObject go)
    {
        UIResult_Maze.instance.BuffSelected(_idx);
        //MazeSystem.Instance().GetData().SetAttribute((int)EObjectAttr.MazeBuffArrow, 1, true);
    }

    private Sprite GetBuffTypeSprite(string iconName)
    {
        return ConfigController.Instance().GetUISprite("Buff/" + iconName);
    }
    #endregion
}
