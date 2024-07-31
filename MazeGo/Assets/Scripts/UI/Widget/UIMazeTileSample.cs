using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using GKUI;
using GKBase;
using AStar;
using System.Threading.Tasks;

public class UIMazeTileSample : UIBase
{
    #region Serializable
    [System.Serializable]
    public class Controls
    {
        public Button Bg;
    }
    #endregion

    #region PublicField
    public UIMazeTileSample [] aroundTils = new UIMazeTileSample[4]; // 周围上右下左节点.
    public int row = 0;
    public int col = 0;
    public bool isPass = false;
    public bool isBuild = false;
    #endregion

    #region PrivateField
    [System.NonSerialized]
    private Controls m_ctl;
    #endregion

    #region PublicMethod
    /// <summary>
    /// 查看目标对象是否在次对象周围.
    /// </summary>
    /// <param name="tile">目标对象</param>
    /// <returns></returns>
    public bool IsAround(UIMazeTileSample tile)
    {
        foreach(var t in aroundTils)
        {
            if (null != t && t == tile)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 设置地块图片.
    /// </summary>
    public void SetTileIcon(MazeTileType t)
    {
        switch (t)
        {
            case MazeTileType.Wall:
                m_ctl.Bg.GetComponent<Image>().color = Color.gray;
                break;
            case MazeTileType.Start:
                m_ctl.Bg.GetComponent<Image>().color = Color.yellow;
                SetHighLight();
                break;
            case MazeTileType.End:
                m_ctl.Bg.GetComponent<Image>().color = Color.yellow;
                break;
            case MazeTileType.Nest:
                m_ctl.Bg.GetComponent<Image>().color = new Color(255f / 255f, 165f / 255f, 0f / 255f); ;
                break;
            case MazeTileType.Test:
                m_ctl.Bg.GetComponent<Image>().color = Color.blue;
                break;
            default:
                break;
        }
    }

    public void SetHighLight() 
    {
        // 初始化高亮时, UI控件尚未初始化完毕.
        if (null == m_ctl)
            return;

        UIMazes_Main.instance.highLight.SetParent(m_ctl.Bg.transform);
        UIMazes_Main.instance.highLight.localRotation = Quaternion.identity;
        UIMazes_Main.instance.highLight.localScale = Vector3.one;
        UIMazes_Main.instance.highLight.offsetMin = new Vector2(0.0f, 0.0f);
        UIMazes_Main.instance.highLight.offsetMax = new Vector2(0.0f, 0.0f);
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
        GKUIEventTriggerListener.Get(m_ctl.Bg.gameObject).onEnter = OnEnter;
        GKUIEventTriggerListener.Get(m_ctl.Bg.gameObject).onClick = OnClick;
    }

    private void Init()
    {
        SetTileIcon(MazeSystem.Instance().mapData[row, col].type);
    }

    private void SetTileColor(Color c)
    {
        m_ctl.Bg.GetComponent<Image>().color = c;
    }

    private void Move2Tile(int x, int y)
    {
        MazeTileType tileType = MazeSystem.Instance().mapData[y, x].type;

        // 起点终点, 巢穴地块颜色不变.
        if (tileType != MazeTileType.Start && tileType != MazeTileType.End && tileType != MazeTileType.Nest)
        {
            MazeSystem.Instance().mapData[y, x].tileSample.SetTileColor(Color.red);
            MazeSystem.Instance().mapData[y, x].tileSample.isPass = true;
        }
            
        MazeSystem.Instance().curSelectTile = MazeSystem.Instance().mapData[y, x].tileSample;
        FogOfWar.Instance().Update();
        // 判断是否游戏目标达成. 
        if (MazeSystem.Instance().mapData[y, x].type == MazeTileType.End)
        {
            MazeSystem.Instance().NextStep();
        }
    }

    private void OnEnter(GameObject go)
    {
        //Debug.Log("OnEnter");
        MazeTileType tileType = MazeSystem.Instance().mapData[row, col].type;

        if (MazeSystem.Instance().GetGameStep() == MazeGameplayStep.Maze)
        {
            // 墙体不可行走.
            if (tileType == MazeTileType.Wall || tileType == MazeTileType.Nest)
                return;

            if (IsAround(MazeSystem.Instance().curSelectTile))
            {
                Move2Tile(col, row);
            }
            else
            {
                // 操作优化体验. 点在非周围点, 短距离内可通过寻路到达.
                if (null == MazeSystem.Instance().curSelectTile)
                    return;

                List<Vector2Int> lst = MazeSystem.Instance().FindPath(MazeSystem.Instance().curSelectTile.row, MazeSystem.Instance().curSelectTile.col, row, col, 5);
                for (int i = 0; i < lst.Count; i++)
                {
                    Move2Tile(lst[i].x, lst[i].y);
                }
            }
        }
        else if(MazeSystem.Instance().GetGameStep() == MazeGameplayStep.TownDefense)
        {
            if (tileType == MazeTileType.Wall)
            {
                MazeSystem.Instance().curSelectTile = this;
                FogOfWar.Instance().Update();
            }
        }
    }

    private void OnClick(GameObject go)
    {
        Debug.Log("OnClick");
        ShowBuildTownPanel(false);

        if (MazeSystem.Instance().GetGameStep() == MazeGameplayStep.TownDefense)
        {
            MazeTileType tileType = MazeSystem.Instance().mapData[row, col].type;

            if (tileType != MazeTileType.Wall)
                return;

            // 防御塔仅能在经过路径边建造且该地块尚未被建造.
           if(IsPassPathByAround() && !IsBuildByAround())
            {
                ShowBuildTownPanel(true);
            }
        }
    }

    private bool IsPassPathByAround()
    {
        foreach (var t in aroundTils)
        {
            if (null != t && t.isPass)
                return true;
        }
        return false;
    }

    private bool IsBuildByAround()
    {
        foreach (var t in aroundTils)
        {
            if (null != t && t.isBuild)
                return true;
        }
        return false;
    }

    private void ShowBuildTownPanel(bool bShow)
    {
        if(bShow)
        {
            UIMazes_TownBuild.Open().SetData(col, row);
        }
        else
        {
            UIMazes_TownBuild.Close();
        }
    }
    #endregion
}
