using UnityEngine.UI;
using UnityEngine;
using GKBase;
using GKData;
using GKUI;
using System.Collections;
using System.Collections.Generic;

public class UIMazes_TownBuild : SingletonUIBase<UIMazes_TownBuild>
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Button ExitBtn;
        public GameObject Content;
        public UIBuildSample UIBuildSample;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    private int _x;
    private int _y;
    #endregion

    #region PublicMethod
    public void SetData(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public void CloseSelf()
    {
        Close();
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
        MazeSystem.Instance().OnGameOverEvent -= OnGameOver;
        MazeSystem.Instance().OnGameOverEvent += OnGameOver;
        GKUIEventTriggerListener.Get(m_ctl.ExitBtn.gameObject).onClick = OnClick;
    }

    private void Init()
    {
        UpdateBuild();
    }

    private void UpdateBuild()
    {
        GK.DestroyAllChildren(m_ctl.Content);

        for (int i = 0; i < DataController.Data._mazeTownData.Length; i++)
        {
            var go = GameObject.Instantiate(m_ctl.UIBuildSample.gameObject);
            go.SetActive(true);
            GK.SetParent(go, m_ctl.Content, false);
            GK.GetOrAddComponent<UIBuildSample>(go).SetData(i, _x, _y);
        }
    }

    private void OnClick(GameObject go)
    {
        Close();
    }

    /// <summary>
    /// 游戏结束回调.
    /// </summary>
    private void OnGameOver()
    {
        Close();
    }
    #endregion
}