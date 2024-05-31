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
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    private MazeBuffType _buffType;
    #endregion

    #region PublicMethod
    public void SetData(MazeBuffType t)
    {
        _buffType = t;
        
            
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
            m_ctl.NameText.text = imgs[(int)_buffType];
        }
    }

    private void OnSelected(GameObject go)
    {
        MazeSystem.Instance().GetData().SetAttribute((int)EObjectAttr.MazeBuffArrow, 1, true);
    }

    private Sprite GetBuffTypeSprite(string iconName)
    {
        return ConfigController.Instance().GetUISprite("Maze/" + iconName);
    }
    #endregion
}
