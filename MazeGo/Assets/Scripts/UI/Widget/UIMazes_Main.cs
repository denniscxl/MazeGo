using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using GKBase;
using GKUI;
using UnityEditorInternal;
using static UnityEditor.PlayerSettings;
using TMPro;

public class UIMazes_Main : SingletonUIBase<UIMazes_Main>
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public GameObject Root;
        public GameObject BG;
        public Image NumberA;
        public Image NumberB;
        public Image NumberC;
        public Text LevelDifficultVal;
        public Text MazeSizeVal;
        public Text LevelIncrementVal;
        public Text TimeIncrementVal;
        public Text RecordVal;
        public GameObject Arrow;
    }
    #endregion

    #region PublicField

    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    [SerializeField]
    private GameObject _tileSample;
    [SerializeField]
    private GameObject _highLightSample;
    [System.NonSerialized]
    public RectTransform highLight;
    private RectTransform _centerTran;
    private RectTransform _rootTran;
    private float _rootMoveSpeed = 2;   // 迷宫高亮居中移动速度.

    [SerializeField]
    private Sprite[] _timeImg;

    private int _curLevelDifficult = 1; // 关卡等级. 数据更新时,  UI可能未初始化成功. 故需要缓存更新.

    private bool _bShowArrow = false;   // 每局游戏开始前读取一次箭头情报.
    #endregion

    #region PublicMethod
    public void SetSelectedHighLight(UIMazeTileSample t)
    {
        t.SetHighLight();
    }
    #endregion

    #region PrivateMethod
    private void Start()
    {
        Serializable();
        InitListener();
        Init();
    }

    private void Update()
    {
        UpdateMazePosition();
        UpdateTime();
        UpdateArrowState();
    }

    private void Serializable()
    {
        GK.FindControls(this.gameObject, ref m_ctl);
    }

    private void InitListener()
    {
        MazeSystem.Instance().OnGameOverEvent -= OnGameOver;
        MazeSystem.Instance().OnGameOverEvent += OnGameOver;
        MazeSystem.Instance().OnGameBeginEvent -= OnGameBegin;
        MazeSystem.Instance().OnGameBeginEvent += OnGameBegin;
    }

    private void Init()
    {
        // 初始化高亮对象.
        highLight = GameObject.Instantiate(_highLightSample).GetComponent<RectTransform>(); ;

        // 初始化关卡地块对象.
        RectTransform rectTran = null;
        for (int i = 0; i < MazeSystem.MAP_HEIGHT + MazeSystem.Instance().mazeSizeAddition; ++i)
        {
            for (int j = 0; j < MazeSystem.MAP_WIDTH + MazeSystem.Instance().mazeSizeAddition; ++j)
            {
                GameObject t = GameObject.Instantiate(_tileSample);

                rectTran = t.GetComponent<RectTransform>();
                rectTran.SetParent(m_ctl.Root.transform);
                rectTran.localRotation = Quaternion.identity;
                rectTran.localScale = Vector3.one;
                rectTran.anchoredPosition = new Vector3(100 * j, 100 * i, 0);

                UIMazeTileSample mazeTile = t.GetComponent<UIMazeTileSample>();
                mazeTile.row = i;
                mazeTile.col = j;
                MazeSystem.Instance().mapData[i, j].tileSample = mazeTile;


                if (MazeSystem.Instance().mapData[i, j].type == MazeTileType.Start)
                    MazeSystem.Instance().curSelectTile = MazeSystem.Instance().mapData[i, j].tileSample;
            }
        }
        MazeSystem.Instance().GenerateAroundTile();
        _centerTran = m_ctl.BG.GetComponent<RectTransform>();
        _rootTran = m_ctl.Root.GetComponent<RectTransform>();

        // 重置关卡时间.
        MazeSystem.Instance().UpdateLevelTime();

        // 更新玩家信息面板.
        UpdateInfo();


    }

    private void UpdateInfo()
    {
        m_ctl.LevelDifficultVal.text = MazeSystem.Instance().GetLvDifficult().ToString();
        m_ctl.MazeSizeVal.text = string.Format("{0}X{0}", MazeSystem.Instance().GetCurMapTileHeight(), MazeSystem.Instance().GetCurMapTileWidth());
        m_ctl.LevelIncrementVal.text = MazeSystem.Instance().GetTotalIncrementLvSize().ToString();
        m_ctl.TimeIncrementVal.text = MazeSystem.Instance().GetTotalIncrementTime().ToString();
    }

    /// <summary>
    /// 更新地图坐标. 居中当前高亮地块.
    /// </summary>
    private void UpdateMazePosition()
    {
        Vector3 offest = _centerTran.position -highLight.position;
        Vector3 targetPos = _rootTran.position + offest;

        _rootTran.position = Vector3.Lerp(_rootTran.position, targetPos, Time.deltaTime * _rootMoveSpeed);
    }

    private void UpdateTime()
    {
        if (null == m_ctl || null == m_ctl.NumberA)
            return;

        int t = MazeSystem.Instance().GetCurLevelTime();
        int a = t / 100;
        int b = t / 10 % 10;
        int c = t % 10;
        
        m_ctl.NumberA.sprite = _timeImg[a];
        m_ctl.NumberB.sprite = _timeImg[b];
        m_ctl.NumberC.sprite = _timeImg[c];
    }

    /// <summary>
    /// 更新BUFF 指向终点箭头方向.
    /// </summary>
    private void UpdateArrowState()
    {
        if(_bShowArrow)
        {
            if(null != MazeSystem.Instance().curSelectTile && null != MazeSystem.Instance().endPoint)
            {
                Vector2 scoure = new Vector2(MazeSystem.Instance().curSelectTile.col, MazeSystem.Instance().curSelectTile.row);
                Vector2 target = new Vector2(MazeSystem.Instance().endPoint.x, MazeSystem.Instance().endPoint.y);

                Vector2 direction = target - scoure;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90;
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                m_ctl.Arrow.transform.rotation = rotation;
            }
        }
    }

    private void OnGameBegin()
    {
        Debug.Log("OnGameBegin");
        _bShowArrow = MazeSystem.Instance().GetData().GetAttribute((int)EObjectAttr.MazeBuffArrow).ValInt == 1;
        m_ctl.Arrow.SetActive(_bShowArrow);
    }

    /// <summary>
    /// 游戏结束回调.
    /// </summary>
    private void OnGameOver()
    {
        Debug.Log("OnGameOver");
        UIResult.Open().SetData(false);
        Close();
    }
    #endregion
}
