using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GKBase;
using GKUI;
using GKData;

public class UIResult_Maze : SingletonUIBase<UIResult_Maze>
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Text TitleText;
        public Text ScoreText;
        public GameObject ResultContent;
        public ScrollRect PassTimeScrollView;
        public UIPassTimeItemSample UIPassTimeItemSample;
        public GameObject BuffContent;
        public UIBuffItemSample UIBuffItemSample;
        public GameObject DeBuffContent;
        public UIBuffItemSample UIDeBuffItemSample;
        public Button BackBtn;
    }
    #endregion

    #region PublicField
    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;

    // 游戏结果标志位.
    private bool _bVictory = false;
    private int _score = 0;

    private int _buffIndex = -1;
    private int _debuffIndex = -1;
    private UIBuffItemSample[] buffItems = new UIBuffItemSample[3];
    #endregion

    #region PublicMethod
    public void SetData(bool bVictory)
    {
        _bVictory = bVictory;
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
        GKUIEventTriggerListener.Get(m_ctl.BackBtn.gameObject).onClick = OnBack;
    }

    private void Init()
    {
        int id = _bVictory ? 94 : 95;
        m_ctl.TitleText.text = DataController.Instance().GetLocalization(id);

        UpdateLevelPassTime();
        UpdateBuff();
        BuffSelected(0);

        m_ctl.ScoreText.text = _score.ToString();
    }

    /// <summary>
    /// 更新关卡通关时间.
    /// </summary>
    private void UpdateLevelPassTime()
    {
        _score = 0;

        GK.DestroyAllChildren(m_ctl.ResultContent);

        GKDataBase d = MazeSystem.Instance().GetData();

        List<int> passTimelst = d.GetAttributeList((int)EObjectAttr.MazeLevelPassTime).ValInt;
        List<int> bestTimelst = d.GetAttributeList((int)EObjectAttr.MazeLevelBestTime).ValInt;

        int count = MazeSystem.Instance().GetLvDifficult() - 1;
        for(int i = 0; i < count; i++)
        {
            if(passTimelst.Count < i)
            {
                Debug.LogError(string.Format("UpdateLevelPassTime - passTimelst.Count < i"));
                continue;
            }
            int psaaTime = passTimelst[i];

            int bestTime = 0;
            if (bestTimelst.Count < i)
            {
                bestTime = -1;
            }
            else
            {
                bestTime = bestTimelst[i];
            }
            
            var go = GameObject.Instantiate(m_ctl.UIPassTimeItemSample.gameObject);
            go.SetActive(true);
            GK.SetParent(go, m_ctl.ResultContent, false);
            GK.GetOrAddComponent<UIPassTimeItemSample>(go).SetData(i+1, psaaTime, bestTime);

            CalcScore(i+1, psaaTime);
        }

        m_ctl.PassTimeScrollView.normalizedPosition = new Vector2(0, 0);
    }

    /// <summary>
    /// 更新随机BUFF.
    /// </summary>
    private void UpdateBuff()
    {
        GK.DestroyAllChildren(m_ctl.BuffContent);
        if(_bVictory)
        {
            List<MazeBuffType> lst = MazeSystem.Instance().GetRandomBuff();
            for (int i = 0; i < lst.Count; i++)
            {
                var go = GameObject.Instantiate(m_ctl.UIBuffItemSample.gameObject);
                go.SetActive(true);
                GK.SetParent(go, m_ctl.BuffContent, false);
                GK.GetOrAddComponent<UIBuffItemSample>(go).SetData(lst[i], i);
                buffItems[i] = GK.GetOrAddComponent<UIBuffItemSample>(go);
            }
        }
        
    }

    public void BuffSelected(int idx)
    {
        if(_bVictory)
        {
            if ((-1 != _buffIndex) && (_buffIndex < buffItems.GetLength(0)))
                buffItems[_buffIndex].UpdateHighlight(false);
            _buffIndex = idx;
            buffItems[idx].UpdateHighlight(true);
        }
    }

    private void CalcScore(int lv, int passTime)
    {
        _score += (lv * 10 + passTime);
    }

    private void OnBack(GameObject go)
    {
        if(_bVictory)
        {
            if (0 <= _buffIndex && _buffIndex < buffItems.Length)
                MazeSystem.Instance().ActiveBuff(buffItems[_buffIndex].GetBuffType());
            LevelController.Instance().InitData();
        }
        else
        {
            UILoading.Open().Next(LoadingType.Lobby);
        }
        Close();
    }
    #endregion
}
