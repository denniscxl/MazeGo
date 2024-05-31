using UnityEngine.UI;
using UnityEngine;
using GKBase;
using GKUI;

public class UILobby : SingletonUIBase<UILobby>
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Button AchievementBtn;
        public Button InformationBtn;
        public Button AthleticsBtn;
        public Button StoryBtn;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    private UITitle_Maze _uiTitle;
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
        GKUIEventTriggerListener.Get(m_ctl.AchievementBtn.gameObject).onClick = OnAchievement;
        GKUIEventTriggerListener.Get(m_ctl.InformationBtn.gameObject).onClick = OnInformation;
        GKUIEventTriggerListener.Get(m_ctl.AthleticsBtn.gameObject).onClick = OnAthletics;
        GKUIEventTriggerListener.Get(m_ctl.StoryBtn.gameObject).onClick = OnStory;

    }

    private void Init()
    {
        _uiTitle = UITitle_Maze.Open();
        _uiTitle.SetState(false);
    }

    private void OnAchievement(GameObject go)
    {
        UIAchievement.Open();
    }

    private void OnInformation(GameObject go)
    {
        UIInformation.Open();
    }

    private void OnStory(GameObject go)
    {
        StartGame(LoadingType.Fight);
    }

    private void OnAthletics(GameObject go)
    {
        StartGame(LoadingType.Fight);
    }

    private void StartGame(LoadingType type)
    {
        // 临时设置玩家阵营.
        //PlayerController.Instance().Camp = CampType.Blue;
        UILoading.Open().Next(type);
        Close();
    }
    #endregion
}
