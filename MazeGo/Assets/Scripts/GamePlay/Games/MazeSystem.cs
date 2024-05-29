using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GKBase;
using GKData;
using AStar;


public class MazeSystem : GKSingleton<MazeSystem>
{
    #region PublicField
    public const int MAP_WIDTH = 21;
    public const int MAP_HEIGHT = 21;
    public const int LEVEL_DIFFICULTY_INCREMENT = 4;
    public const int LEVEL_TIME_INCREMENT = 30;

    public delegate void GameBeginEvent();
    public GameBeginEvent OnGameBeginEvent = null;
    public delegate void GameOverEvent();
    public GameOverEvent OnGameOverEvent = null;

    public int mazeSizeAddition = 0;
    public const int MAX_LEVEL_TIME = 60;

    // 地图数据.
    public MazeData[,] mapData = null;              // 二维数组形式地图数据.
    public List<bool> mapListData = null;           // 链表形式的地图数据.
    private UIMazeTileSample _curSelectTile = null;
    public UIMazeTileSample curSelectTile
    {
        get { return _curSelectTile; }
        set 
        { 
            _curSelectTile = value;
            UIMazes_Main.instance.SetSelectedHighLight(_curSelectTile);
        }
    }

    public AStarRedBlackSearch _aStar = null;       // 寻路句柄.
    public Node endPoint = new Node(0, 0);
    #endregion

    #region PrivateField
    private GKDataBase _data = new GKDataBase();

    private float _curlevelTime = 0;    // 当前关卡时间.
    private int _lv = 1;                // 关卡等级.
    private int _levelBeginTime = 0;    // 当前关卡起始时间.
    #endregion

    #region PublicMethod
    public void Init()
    {
        
    }
    
    public void Update()
    {
        if(_curlevelTime > 0 && !MyGame.Instance.isPause)
        {
            _curlevelTime -= Time.deltaTime;
            if (_curlevelTime <= 0)
            {
                if (null != OnGameOverEvent)
                {
                    GameOver();
                    OnGameOverEvent();
                }
            }
        }
    }

    /// <summary>
    /// 获取存储数据.
    /// </summary>
    /// <returns> 返回数据源 </returns>
    public GKDataBase GetData()
    {
        return _data;
    }

    /// <summary>
    /// 设置存储数据.
    /// </summary>
    /// <param name="d"> 数据源 </param>
    public void SetData(GKDataBase d)
    {
        _data = d;
    }

    /// <summary>
    /// 更新玩家过关时间.
    /// </summary>
    /// <param name="lv"> 关卡难度 </param>
    /// <param name="useTime"> 使用时间 </param>
    public void UpdateLevelPassTime(int lv, int useTime)
    {
        // 更新过关时间.
        List<int> lst = _data.GetAttributeList((int)EObjectAttr.MazeLevelPassTime).ValInt;
        if (null != lst && lv <= lst.Count)
        {
            lst[lv - 1] = useTime;
        }
        else
        {
            lst.Add(useTime);
        }
        _data.SetAttributeList((int)EObjectAttr.MazeLevelPassTime, lst, true);

        // 更新最佳时间.
        List<int> maxlst = _data.GetAttributeList((int)EObjectAttr.MazeLevelBestTime).ValInt;
        if (null != maxlst && lv <= maxlst.Count)
        {
            if (0 > maxlst[lv - 1])
                maxlst[lv - 1] = 999;

            if (useTime < maxlst[lv - 1])
            {
                maxlst[lv-1] = useTime;
            }
        }
        else
        {
            maxlst.Add(useTime);
        }
        _data.SetAttributeList((int)EObjectAttr.MazeLevelBestTime, maxlst, true);

        DataController.Instance().SaveMazeData();
    }

    /// <summary>
    /// 获取最终关卡时间增量.
    /// </summary>
    /// <returns> 时间增量 </returns>
    public int GetTotalIncrementTime()
    {
        return LEVEL_TIME_INCREMENT;
    }

    /// <summary>
    /// 获取最终关卡难度增量.
    /// </summary>
    /// <returns> 难度增量 </returns>
    public int GetTotalIncrementLvSize()
    {
        return LEVEL_DIFFICULTY_INCREMENT;
    }

    /// <summary>
    /// 获取当前关卡等级.
    /// </summary>
    /// <returns> 关卡等级 </returns>
    public int GetLvDifficult()
    {
       return _lv;
    }

    public void AddLvDifficult()
    {
        _lv++;
    }

    public void ResetLvDifficult()
    {
        _lv = 1;
    }

    /// <summary>
    /// 获取关卡地块总数.
    /// </summary>
    /// <returns> 地块总数 </returns>
    public int GetMapTileSize()
    {
        return (MAP_HEIGHT + mazeSizeAddition) * (MAP_WIDTH + mazeSizeAddition);
    }

    public int GetCurMapTileWidth()
    {
        return MAP_WIDTH + mazeSizeAddition;
    }

    public int GetCurMapTileHeight()
    {
        return MAP_HEIGHT + mazeSizeAddition;
    }

    /// <summary>
    /// 迷宫构建入口函数. 
    /// 具体迷宫构建方法参考GenerateDeepSearchMaze.cs.
    /// </summary>
    public void GenerateMaze()
    {
        endPoint = GenerateDeepSearchMaze.Instance().GenerateMaze(MAP_WIDTH + mazeSizeAddition, MAP_HEIGHT + mazeSizeAddition, out mapData);
        mapListData = Array2List(mapData);
        _aStar = new AStarRedBlackSearch(mapListData, MAP_WIDTH + mazeSizeAddition);
        if (null != OnGameBeginEvent)
        {
            OnGameBeginEvent();
        }
        MyGame.Instance.isPause = false;
    }

    /// <summary>
    /// 生成地块周围数据绑定.
    /// </summary>
    public void GenerateAroundTile()
    {
        for (int i = 0; i < MAP_HEIGHT + mazeSizeAddition; i++)
        {
            for (int j = 0; j < MAP_WIDTH + mazeSizeAddition; j++)
            {
                // 上.
                mapData[i, j].tileSample.aroundTils[0] = (i != MAP_HEIGHT + mazeSizeAddition - 1) ? mapData[i+1, j].tileSample : null;
                // 右.
                mapData[i, j].tileSample.aroundTils[1] = (j != MAP_WIDTH + mazeSizeAddition - 1) ? mapData[i, j+1].tileSample : null;
                // 下.
                mapData[i, j].tileSample.aroundTils[2] = (i != 0) ? mapData[i-1, j].tileSample : null;
                // 左.
                mapData[i, j].tileSample.aroundTils[3] = (j != 0) ? mapData[i, j-1].tileSample : null;
            }
        }
    }

    /// <summary>
    /// 获取当前关卡时间.
    /// </summary>
    /// <returns></returns>
    public int GetCurLevelTime()
    {
        return (int)_curlevelTime;
    }

    public void UpdateLevelTime()
    {
        if (1 == MazeSystem.Instance().GetLvDifficult())
        {
            _curlevelTime = MAX_LEVEL_TIME;
            
        }
        else
            _curlevelTime += GetTotalIncrementTime();

        _levelBeginTime = (int)_curlevelTime;
    }

    public void NextLevel()
    {
        UpdateLevelPassTime(_lv, _levelBeginTime - (int)_curlevelTime);
        MyGame.Instance.isPause = true;
        AddLvDifficult();
        mazeSizeAddition += GetTotalIncrementLvSize();
    }

    public void GameOver()
    {
        ResetBuffState();
        UpdateLevelPassTime(_lv, _levelBeginTime);
        _curlevelTime = -1;
        ResetLvDifficult();
        mazeSizeAddition = 0;
    }

    /// <summary>
    /// 以高亮地块为中心, Size为范围构建小范围地图. 查找目标点是否能在规定步长内到达.
    /// </summary>
    /// <param name="targetY"> 终点行坐标 </param>
    /// <param name="targetX"> 终点列坐标 </param>
    /// /// <param name="len"> 步长极限 </param>
    public List<Node> FindPath(int targetY, int targetX, int len)
    {
        List<Node> lst = new List<Node>();

        if (null == curSelectTile)
            return lst;


        // AStar 寻路.
        Dictionary<int, int> path = _aStar.FindPath(
            Node2Index(new Node(curSelectTile.col, curSelectTile.row), MAP_WIDTH + mazeSizeAddition), 
            Node2Index(new Node(targetX, targetY), MAP_WIDTH + mazeSizeAddition));

        if (null != path && path.Count > len)
            path.Clear();

        // 局部坐标与世界左边转换.
        List<Node> outputLst = new List<Node>();
        if (null != path)
        {
            foreach (var k in path.Keys)
            {
                outputLst.Add(Index2Node(k, MAP_WIDTH + mazeSizeAddition, GetMapTileSize()));
            }
        }
        return outputLst;
    }
    #endregion

    #region PrivateMethod
    private List<bool> Array2List(MazeData[,] array)
    {
        List<bool> result = new List<bool>();
        if (null == array)
            return result;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                result.Add(!(array[i, j].type != MazeTileType.Wall));
            }
        }
        return result;
    }

    /// <summary>
    /// 节点转数组索引.
    /// </summary>
    /// <param name="n"> 节点对象 </param>
    /// <param name="width"> 地图宽度 </param>
    /// <returns></returns>
    private int Node2Index(Node n, int width)
    {
        if (null == n) return -1;
        return n.y * width + n.x;
    }

    private Node Index2Node(int index, int width, int count)
    {
        if(index < 0 || index > count-1)
            return null;

        return new Node(index% width, index / width);
    }

    /// <summary>
    /// 重置迷宫游戏的状态.
    /// </summary>
    public void ResetBuffState()
    {
        // 新一局游戏开始时初始化Buff状态.
        for (int i = 0; i < GK.EnumCount<MazeBuffType>(); i++)
        {
            SetBuffState((MazeBuffType)i, 0);
        }
    }

    /// <summary>
    /// 设置Buff状态.
    /// </summary>
    /// <param name="t"> buff类型 0: off 1: on </param>
    /// <param name="isOn"> 是否打开 </param>
    public void SetBuffState(MazeBuffType t, int isOn)
    {
        switch(t)
        {
            case MazeBuffType.Arrow:
                _data.SetAttribute((int)EObjectAttr.MazeBuffArrow, isOn, true);
                break;
        }
    }
    #endregion
}

public class MazeData
{
    public int x, y;
    public MazeTileType type = MazeTileType.Empty;
    public UIMazeTileSample tileSample = null;
}

public enum MazeTileType
{ 
    Empty = 0,  // 空白区域.
    Wall = 1,   // 墙体.
    Start = 2,  // 起点.
    End = 3,    // 终点.
    Test = 4,   // 测试类型.
}

public enum MazeBuffType
{
    Arrow = 0,  // 终点箭头.
}