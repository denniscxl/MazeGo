using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using GKBase;
using GKUI;
using Unity.VisualScripting;

public class UIBuildSample : UIBase
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
    private int _type;
    private int _x;
    private int _y;
    #endregion

    #region PublicMethod
    public void SetData(int t, int x, int y)
    {
        _type = t;
        _x = x;
        _y = y;
    }
    public int GetBuildType()
    { 
        return _type; 
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
        m_ctl.Icon.sprite = GetBuildTypeSprite(_type.ToString());
        m_ctl.NameText.text = DataController.Instance().GetLocalization(_type, LocalizationSubType.MazeTown);
    }

    private void OnSelected(GameObject go)
    {
        //Debug.Log("OnSelected");
        MazeSystem.Instance().SpawnTown(_type, _y, _x);
        if (null != UIMazes_TownBuild.instance)
            UIMazes_TownBuild.instance.CloseSelf();
    }

    private Sprite GetBuildTypeSprite(string iconName)
    {
        return ConfigController.Instance().GetUISprite("Town/" + iconName);
    }
    #endregion
}
