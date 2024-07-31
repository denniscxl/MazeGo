using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GKBase;
using GKData;
using AStar;
using Unity.VisualScripting;


public class MazeSystem : GKSingleton<MazeSystem>
{
    #region PublicField
    public const int MAP_WIDTH = 21;
    public const int MAP_HEIGHT = 21;
    public const int LEVEL_DIFFICULTY_INCREMENT = 6;
    public const int LEVEL_TIME_INCREMENT = 30;
    public const int BASE_COIN = 30;
    public const int NEST_COUNT = 4;

    public delegate void GameBeginEvent();
    public GameBeginEvent OnGameBeginEvent = null;

    public delegate void TDTimeOverEvent();
    public TDTimeOverEvent OnTDTimeOverEvent = null;

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
            if (null != UIMazes_Main.instance)
                UIMazes_Main.instance.SetSelectedHighLight(_curSelectTile);
            else
                Debug.LogError("Set highlight faile, but main panel always deleted.");
        }
    }

    public AStarRedBlackSearch _aStar = null;                   // 寻路句柄.
    public Vector2Int endPoint = new Vector2Int(0, 0);
    #endregion

    #region PrivateField
    private GKDataBase _data = new GKDataBase();

    private float _curlevelTime = 0;                            // 当前关卡时间.
    private int _levelBeginTime = 0;                            // 当前关卡起始时间.
    private float _curTDTime = 0;                               // 当前塔防模式时间, 塔防模式的时间是迷宫模式的过关时间. 过关时间内进行怪物生成.

    private int _lv = 1;                                        // 关卡等级.
    private MazeGameplayStep _step = MazeGameplayStep.Maze;     // 关卡阶段.

    private Dictionary<int, Npc> _monsterDict = new Dictionary<int, Npc>();         // 关卡内怪物列表.
    private Dictionary<int, MazeNest> _nestDict = new Dictionary<int, MazeNest>();  // 关卡内巢穴列表.
    private Dictionary<int, Town> _townDict = new Dictionary<int, Town>();          // 关卡内防御塔列表.

    private int _guid = 0;
    #endregion

    #region PublicMethod
    public void Init()
    {
        
    }

    public void Update()
    {
        if(_step == MazeGameplayStep.Maze)
        {
            if (_curlevelTime > 0 && !MyGame.Instance.isPause)
            {
                _curlevelTime -= Time.deltaTime;
                if (_curlevelTime <= 0)
                {
                    _curlevelTime = 0;
                    GameOver(); 
                }
            }
        }
        else if(_step == MazeGameplayStep.TownDefense)
        {
            if (_curTDTime > 0 && !MyGame.Instance.isPause)
            {
                _curTDTime -= Time.deltaTime;
                if (_curTDTime <= 0)
                {
                    _curTDTime = 0;
                    if (null != OnTDTimeOverEvent)
                    {
                        OnTDTimeOverEvent();
                    }
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
    /// 获取当前游戏阶段.
    /// </summary>
    /// <returns> 游戏阶段枚举 </returns>
    public MazeGameplayStep GetGameStep()
    { 
        return _step; 
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
        return LEVEL_TIME_INCREMENT + GetIncrementTime();
    }

    /// <summary>
    /// 获取最终关卡难度增量.
    /// </summary>
    /// <returns> 难度增量 </returns>
    public int GetTotalIncrementLvSize()
    {
        return LEVEL_DIFFICULTY_INCREMENT + GetpDecrementMapSize();
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
        return GetCurMapTileHeight() * GetCurMapTileWidth();
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
        endPoint = GenerateDeepSearchMaze.Instance().GenerateMaze(GetCurMapTileWidth(), GetCurMapTileHeight(), GetNestCount(), out mapData);
        mapListData = Array2List(mapData);
        _aStar = new AStarRedBlackSearch(mapListData, GetCurMapTileWidth());
        MyGame.Instance.isPause = false;
    }

    /// <summary>
    /// 生成地块周围数据绑定.
    /// </summary>
    public void GenerateAroundTile()
    {
        for (int i = 0; i < GetCurMapTileHeight(); i++)
        {
            for (int j = 0; j < GetCurMapTileWidth(); j++)
            {
                // 上.
                mapData[i, j].tileSample.aroundTils[0] = (i != GetCurMapTileHeight() - 1) ? mapData[i+1, j].tileSample : null;
                // 右.
                mapData[i, j].tileSample.aroundTils[1] = (j != GetCurMapTileWidth() - 1) ? mapData[i, j+1].tileSample : null;
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
        if(_step == MazeGameplayStep.Maze)
            return (int)_curlevelTime;
        else if (_step == MazeGameplayStep.TownDefense)
            return (int)_curTDTime;

        return 0;
    }

    public void UpdateLevelTime()
    {
        if (1 == MazeSystem.Instance().GetLvDifficult())
        {
            InitNewGame();
        }
        else
            _curlevelTime += GetTotalIncrementTime();

        _levelBeginTime = (int)_curlevelTime;
    }

    /// <summary>
    /// 新一局游戏开始时初始化.
    /// </summary>
    private void InitNewGame()
    {
        _step = MazeGameplayStep.Maze;
        _curlevelTime = MAX_LEVEL_TIME;
        ResetBuffState();
        PlayerController.Instance().Coin = BASE_COIN;
    }

    /// <summary>
    /// 游戏进入下个阶段.
    /// </summary>
    public void NextStep()
    {
        MyGame.Instance.isPause = true;
        _step = MazeGameplayStep.TownDefense;
        _curTDTime = _levelBeginTime - (int)_curlevelTime;
        UpdateLevelPassTime(_lv, (int)_curTDTime);
        UIMazes_StepChange.Open().SetData((int)_curTDTime);
        GenerateNest();
    }

    /// <summary>
    /// 游戏成功下一关.
    /// </summary>
    public void NextLevel()
    {
        MyGame.Instance.isPause = true;
        _step = MazeGameplayStep.Maze;
        _curTDTime = -1;
        _guid = 0;
        _monsterDict.Clear();
        _nestDict.Clear();
        _townDict.Clear();
        AddLvDifficult();
        mazeSizeAddition += GetTotalIncrementLvSize();
        UIResult_Maze.Open().SetData(true);
        UIMazes_Main.Close();
    }

    /// <summary>
    /// 游戏失败.
    /// </summary>
    public void GameOver()
    {
        MyGame.Instance.isPause = true;
        ResetBuffState();
        _curlevelTime = -1;
        _curTDTime = -1;
        _guid = 0;
        _monsterDict.Clear();
        _nestDict.Clear();
        _townDict.Clear();
        ResetLvDifficult();
        mazeSizeAddition = 0;

        if (null != OnGameOverEvent)
        {
            OnGameOverEvent();
        }
    }

    /// <summary>
    /// 以高亮地块为中心, Size为范围构建小范围地图. 查找目标点是否能在规定步长内到达.
    /// </summary>
    /// <param name="targetY"> 终点行坐标 </param>
    /// <param name="targetX"> 终点列坐标 </param>
    /// /// <param name="len"> 步长极限 </param>
    public List<Vector2Int> FindPath(int sourceY, int sourceX, int targetY, int targetX, int len)
    {
        List<Vector2Int> lst = new List<Vector2Int>();

        // AStar 寻路.
        List<int> path = _aStar.FindPath(
            Node2Index(new Vector2Int(sourceX, sourceY), GetCurMapTileWidth()), 
            Node2Index(new Vector2Int(targetX, targetY), GetCurMapTileWidth()));

        if (null != path && path.Count > len)
            path.Clear();

        // 局部坐标与世界左边转换.
        List<Vector2Int> outputLst = new List<Vector2Int>();
        if (null != path)
        {
            foreach (var k in path)
            {
                outputLst.Add(Index2Node(k, GetCurMapTileWidth(), GetMapTileSize()));
            }
        }
        return outputLst;
    }

    /// <summary>
    /// 获取目标范围内的怪物对象列表.
    /// </summary>
    /// <param name="pos"> 目标坐标 </param>
    /// <param name="range"> 范围 </param>
    /// <returns></returns>
    public List<Npc> GetMonsterByRange(Vector3 pos, float range)
    {
        List<Npc> lst =  new List<Npc>();
        foreach(var m in _monsterDict.Values)
        {
            if(Vector3.Distance(m.transform.position, pos) < range)
            {
                lst.Add(m);
            }
        }
        return lst;
    }

    /// <summary>
    /// Buff 激活.
    /// </summary>
    /// <param name="t"> Buff 类型 </param>
    public void ActiveBuff(MazeBuffType t)
    {
        MazeSystem.Instance().SetBuffState(t, 1);
        switch(t)
        {
            case MazeBuffType.TimeAdditionI:
            case MazeBuffType.TimeAdditionII:
            case MazeBuffType.TimeAdditionIII:
                AddTimeByBuffType(t);
                break;
            case MazeBuffType.MapReduceI:
            case MazeBuffType.MapReduceII:
            case MazeBuffType.MapReduceIII:
                ReduceMapSize(t);
                break;
        }
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
        _data.SetAttribute((int)EObjectAttr.MazeBuffArrow + (int)t, isOn, true);
    }
    
    /// <summary>
    /// 获得当前未获得Buff中最多随机三个.
    /// </summary>
    /// <returns> Buff列表 </returns>
    public List<MazeBuffType> GetRandomBuff()
    {
        List<MazeBuffType> lst = new List<MazeBuffType>();
        List<MazeBuffType> outputLst = new List<MazeBuffType>();

        if(GetData().GetAttribute((int)EObjectAttr.MazeBuffArrow).ValInt == 0)
            lst.Add(MazeBuffType.Arrow);

        int lv = GetIncrementLevel(MazeBuffType.TimeIncrementI);
        if (lv < 3)
            lst.Add((MazeBuffType.TimeIncrementI + lv));

        lv = GetIncrementLevel(MazeBuffType.TimeAdditionI);
        if (lv < 3)
            lst.Add((MazeBuffType.TimeAdditionI + lv));

        lv = GetIncrementLevel(MazeBuffType.MapDecrementI);
        if (lv < 3)
            lst.Add((MazeBuffType.MapDecrementI + lv));

        lv = GetIncrementLevel(MazeBuffType.MapReduceI);
        if (lv < 3)
            lst.Add((MazeBuffType.MapReduceI + lv));

        lv = GetIncrementLevel(MazeBuffType.NestReduceI);
        if (lv < 3)
            lst.Add((MazeBuffType.NestReduceI + lv));

        if (0 < lst.Count)
        {
            GK.ShuffleByList<MazeBuffType>(ref lst);
            int count = lst.Count> 3 ? 3 : lst.Count;
            for (int i = 0; i < count; i++)
            {
                outputLst.Add((MazeBuffType)lst[i]);
            }
        }

        return outputLst;
    }

    /// <summary>
    /// 返回当前Buff等级.
    /// 部分BUFF可以升级. 获取当前该BUFF等级.
    /// </summary>
    /// <returns> 返回改BUFF等级.</returns>
    public int GetIncrementLevel(MazeBuffType buffType)
    {
        int offest = buffType - MazeBuffType.Arrow;

        if (GetData().GetAttribute((int)EObjectAttr.MazeBuffArrow + offest).ValInt == 1)
        {
            if (GetData().GetAttribute((int)EObjectAttr.MazeBuffArrow + offest + 1).ValInt == 1)
            {
                if (GetData().GetAttribute((int)EObjectAttr.MazeBuffArrow + offest + 2).ValInt == 1)
                {
                    return 3;
                }
                return 2;
            }
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// 产生怪物.
    /// </summary>
    /// <param name="id"> 怪物ID </param>
    /// <param name="y"> 行索引 </param>
    /// <param name="x"> 纵索引 </param>
    public void SpawnMonsters(int id, int y, int x)
    {
        GameObject obj = GK.TryLoadGameObject( string.Format("Prefabs/Maze/Monsters/{0}", id));
        if(null != obj)
        {
            Npc npc = obj.GetOrAddComponent<Npc>();
            if(null != npc)
            {
                int guid = GenerateGuid();
                npc.SetData(guid, id, mapData[y, x].tileSample);
                _monsterDict.Add(guid, npc);
            }
        }
    }

    /// <summary>
    /// 产生防御塔.
    /// </summary>
    /// <param name="id"> 防御塔类型 </param>
    /// <param name="y"> 行索引 </param>
    /// <param name="x"> 纵索引 </param>
    public void SpawnTown(int id, int y, int x)
    {
        GameObject obj = GK.TryLoadGameObject(string.Format("Prefabs/Maze/Towns/{0}", id));
        if (null != obj)
        {
            Town town = obj.GetOrAddComponent<Town>();
            if (null != town)
            {
                int guid = GenerateGuid();
                town.SetData(guid, id, mapData[y, x].tileSample);
                _townDict.Add(guid, town);
            }
        }
    }

    private int GenerateGuid()
    {
        return _guid++;
    }

    /// <summary>
    /// 当怪物被击杀检测是否关卡胜利.
    /// 胜利条件为当生成怪物倒计时结束，并且当前怪物列表为空.
    /// </summary>
    public void MonsterKilled(int guid)
    {
        if(_monsterDict.ContainsKey(guid))
        {
            GK.Destroy(_monsterDict[guid]);
            _monsterDict.Remove(guid);
        }
            
        if(0 == _monsterDict.Count && 0 == _curTDTime)
        {
            NextLevel();
        }
    }
    #endregion
    
    #region PrivateMethod
    private List<bool> Array2List(MazeData[,] array)
    {
        List<bool> result = new List<bool>();
        bool bmove = false;
        if (null == array)
            return result;
        for (int i = 0; i < array.GetLength(0); i++)
        {
            for (int j = 0; j < array.GetLength(1); j++)
            {
                // false 可行走. 
                MazeTileType t = array[i, j].type;
                bmove = ((t == MazeTileType.Wall) || (t == MazeTileType.Nest));
                result.Add(bmove);
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
    private int Node2Index(Vector2Int n, int width)
    {
        if (null == n) return -1;
        return n.y * width + n.x;
    }

    private Vector2Int Index2Node(int index, int width, int count)
    {
        if(index < 0 || index > count-1)
            return Vector2Int.zero;

        return new Vector2Int(index% width, index / width);
    }

    /// <summary>
    /// 获取关卡时间增量.
    /// </summary>
    /// <returns> 时间增量 </returns>
    private int GetIncrementTime()
    {
        return GetIncrementLevel(MazeBuffType.TimeIncrementI);
    }

    private void AddTimeByBuffType(MazeBuffType t)
    {
        switch (t)
        {
            case MazeBuffType.TimeAdditionI:
                _curlevelTime += 5;
                break;
            case MazeBuffType.TimeAdditionII:
                _curlevelTime += 10;
                break;
            case MazeBuffType.TimeAdditionIII:
                _curlevelTime += 15;
                break;
        }

    }

    private void ReduceMapSize(MazeBuffType t)
    {
        switch(t)
        {
            case  MazeBuffType.MapReduceI:
                mazeSizeAddition -= 4;
                break;
            case MazeBuffType.MapReduceII:
                mazeSizeAddition -= 6;
                break;
            case MazeBuffType.MapReduceIII:
                mazeSizeAddition -= 8;
                break;
        }

    }

    /// <summary>
    /// 获取关卡尺寸增量.
    /// </summary>
    /// <returns> 地图尺寸增量 </returns>
    private int GetpDecrementMapSize()
    {
        return - GetIncrementLevel(MazeBuffType.MapDecrementI) * 2;
    }

    /// <summary>
    /// 获取当前怪物巢穴增量.
    /// </summary>
    /// <returns></returns>
    private int GetNestCount()
    {
        return NEST_COUNT + GetNestReduce();
    }

    /// <summary>
    /// 获取怪物巢穴数量.
    /// </summary>
    /// <returns></returns>
    private int GetNestReduce()
    {
        return -GetIncrementLevel(MazeBuffType.NestReduceI);
    }

    /// <summary>
    /// 生成巢穴.
    /// </summary>
    private void GenerateNest()
    {
        for (int i = 0; i < GetCurMapTileHeight(); i++)
        {
            for (int j = 0; j < GetCurMapTileWidth(); j++)
            {
                if (mapData[i, j].type == MazeTileType.Nest)
                {
                    GameObject obj = GK.TryLoadGameObject("Prefabs/Maze/Nest");
                    if (null != obj)
                    {
                        MazeNest nest = obj.GetOrAddComponent<MazeNest>();
                        if (null != nest)
                        {
                            int guid = GenerateGuid();
                            nest.SetData(guid, mapData[i, j].tileSample);
                            _nestDict.Add(guid, nest);
                        }
                    }
                }
            }
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

/// <summary>
/// 每局游戏阶段.
/// </summary>
public enum MazeGameplayStep
{
    Maze = 0,           // 迷宫竞速模式.
    TownDefense,        // 塔防模式.
}

public enum MazeTileType
{ 
    Empty = 0,  // 空白区域.
    Wall = 1,   // 墙体.
    Start = 2,  // 起点.
    End = 3,    // 终点.
    Nest = 4,   // 怪物巢穴 (第二阶段塔防怪物出生点).
    Test = 5,   // 测试类型.
}

public enum MazeBuffType
{
    Arrow = 0,          // 终点箭头.
    TimeIncrementI,     // 回合结束时间增量 - 1.
    TimeIncrementII,    // 回合结束时间增量 - 2.
    TimeIncrementIII,   // 回合结束时间增量 - 3.
    TimeAdditionI,      // 一次性时间增量 - 5.
    TimeAdditionII,     // 一次性时间增量 - 10.
    TimeAdditionIII,    // 一次性时间增量 - 15.
    MapDecrementI,      // 回合结束地图尺寸减量 - 2.  地块必须为单数.
    MapDecrementII,     // 回合结束地图尺寸减量 - 4.
    MapDecrementIII,    // 回合结束地图尺寸减量 - 6.
    MapReduceI,         // 一次性地图尺寸减少量 - 4.
    MapReduceII,        // 一次性地图尺寸减少量 - 6.
    MapReduceIII,       // 一次性地图尺寸减少量 - 8.
    NestReduceI,        // 一次性怪物巢穴减少量 - 1.
    NestReduceII,       // 一次性怪物巢穴减少量 - 2.
    NestReduceIII,      // 一次性怪物巢穴减少量 - 3.
}
