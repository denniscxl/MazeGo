using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GKBase;
using System;
using System.Reflection;

/// <summary>
/// 2024/05/25
/// 深度优先迷宫构建工厂类.
/// 构建分为3步: 
/// 1. 随机方向上取一点检测是否为死路, 思路退栈.
/// 2. 破墙并记录.
/// 3. 入栈并循环. 直到备份数据所有非墙节点记录完毕, 退出循环.
/// -----------------------------------------------------------
/// 2024/05/26 迷宫路径过于单一. 主动打破部分墙体.
/// 1. 打破墙体, 增加岔路可能性.
/// -----------------------------------------------------------
/// ------ 迷宫路径过于单一. 拓展魔板概念. 
/// 1.模板遵从规格大小 纵向双数横向单数.
/// </summary>
public class GenerateDeepSearchMaze : GKSingleton<GenerateDeepSearchMaze>
{
    #region PublicField

    #endregion

    #region PrivateField
    private MazeData[,] _mapData = null;
    private bool[,] _backUpMapData = null;
    private Stack<Point> _container = new Stack<Point>();
    private Point _curPoint = null;
    #endregion

    #region PublicMethod
    /// <summary>
    /// 深度遍历地牢生成.
    /// </summary>
    /// <param name="width"> 宽度 </param>
    /// <param name="height"> 高度 </param>
    /// <param name="data"> 地图数据 </param>
    public Node GenerateMaze(int width, int height, out MazeData[,] data)
    {
        _container.Clear();
        GenerateMapData(width, height);
        GenerateClosure();
        GenerateRoguelikeMaze();
        Node n = GenerateStartEndPoint();
        BreakWall();
        data = _mapData;

        return n;
    }
    #endregion

    #region PrivateMethod
    private void GenerateMapData(int width, int height)
    {
        _mapData = new MazeData[height, width];
        _backUpMapData = new bool[height, width];
        for (int i = 0; i < height ; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _mapData[i, j] = new MazeData();
                _backUpMapData[i, j] = false;
            }
        }
    }

    private void GenerateClosure()
    {
        for (int i = 0; i < _mapData.GetLength(0); i++)
        {
            for (int j = 0; j < _mapData.GetLength(1); j++)
            {
                if ((0 == i) || ((_mapData.GetLength(0) - 1) == i) || (0 == j) || ((_mapData.GetLength(1) - 1) == j))
                {
                    _mapData[i, j].type = MazeTileType.Wall;
                    _backUpMapData[i, j] = true;
                }
                else
                {
                    if((1 == (i%2)) && (1 == (j % 2)))
                    {
                        _mapData[i, j].type = MazeTileType.Empty;
                        _backUpMapData[i, j] = false;
                    }
                    else
                    {
                        _mapData[i, j].type = MazeTileType.Wall;
                        _backUpMapData[i, j] = true;
                    }
                }
                _mapData[i, j].x = j;
                _mapData[i, j].y = i;
            }
        }
    }

    private void GenerateRoguelikeMaze()
    {
        int direction = 0;

        _curPoint = new Point(1, 1);
        _container.Push(_curPoint);
        _backUpMapData[1, 1] = true;

        while (true)
        {
            //结束循环条件：遍历完了.
            int count = 0;
            for (int i = 0; i < _backUpMapData.GetLength(0); i++)
            {
                for (int j = 0; j < _backUpMapData.GetLength(1); j++)
                {
                    if (_backUpMapData[i,j] == false)
                        count++;
                }
            }
            if (count == 0) 
                break;

            direction = GetRandomDirection();

            //退栈条件是走到死胡同，死胡同是上下左右都走过了.
            while(-1 == direction)
            {
                _container.Pop();
                if(_container.Count == 0)
                {
                    Debug.LogError("GenerateRoguelikeMaze Faile - 栈中元素为空, 弹出异常.");
                    break;
                }
                _curPoint = _container.Peek();
                direction = GetRandomDirection();
            }

            if (direction == 0)
            {
                //向上走并且上面未被遍历或者边界墙.
                _mapData[_curPoint.y + 1, _curPoint.x].type = MazeTileType.Empty;
                //记录走过.
                _backUpMapData[_curPoint.y + 2, _curPoint.x] = true;
                Point p = new Point(_curPoint.y + 2, _curPoint.x);
                _container.Push(p);
                _curPoint = p;
            }
            else if (direction == 1)
            {
                _mapData[_curPoint.y, _curPoint.x - 1].type = MazeTileType.Empty;
                _backUpMapData[_curPoint.y, _curPoint.x - 2] = true;
                Point p = new Point(_curPoint.y, _curPoint.x - 2);
                _container.Push(p);
                _curPoint = p;
            }
            else if (direction == 2)
            {
                _mapData[_curPoint.y - 1, _curPoint.x].type = MazeTileType.Empty;
                _backUpMapData[_curPoint.y - 2, _curPoint.x] = true;
                Point p = new Point(_curPoint.y - 2, _curPoint.x);
                _container.Push(p);
                _curPoint = p;
            }
            else if (direction == 3)
            {
                _mapData[_curPoint.y, _curPoint.x + 1].type = MazeTileType.Empty;
                _backUpMapData[_curPoint.y, _curPoint.x + 2] = true;
                Point p = new Point(_curPoint.y, _curPoint.x + 2);
                _container.Push(p);
                _curPoint = p;
            }
        }
    }

    /// <summary>
    /// 走的方向是随机的 0123上左下右.
    /// </summary>
    /// <returns> 方向, -1为无路可走 </returns>
    private int GetRandomDirection()
    {
        bool[] dirArray = { false, false, false, false };
        int count = 0;
        int index = -1;
        for (int i = 0; i < 4; i++)
        {
            dirArray[i] = IsTileTypeByDirection(i, false, _curPoint);
            if(dirArray[i]) 
            { 
                count++; 
            }
        }

        if (count > 0)
            index = UnityEngine.Random.Range(0, count);
        else
            return -1;

        for (int i = 0; i < 4; i++)
        {
            if (dirArray[i])
                index--;
            if (index < 0)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// 生成起点终点. 取所有外围节点中距离最远的两个点.
    /// 目前凸包生成函数存在异常. 但正方形最远距离就是对角线.
    /// 25/05/30: 增加随机起始与终点. 分别为四个角落. 
    /// 0-左下|1-左上|2-右上|3-右下. 对于重复节点进行跳过处理.
    /// </summary>
    private Node GenerateStartEndPoint()
    {
        int r1 = UnityEngine.Random.Range(0, 4);
        
        int r2 = UnityEngine.Random.Range(0, 4);
        while (r1 == r2)
        {
            r2 = UnityEngine.Random.Range(0, 4);
        }

        SetStarEndPoint(r1, MazeTileType.Start);
        return SetStarEndPoint(r2, MazeTileType.End);
    }

    private Node SetStarEndPoint(int dir, MazeTileType t)
    {
        Node n = new Node();
        switch(dir) 
        { 
            case 0:
                n.x = 1; 
                n.y = 1;
                break;
            case 1:
                n.x = 1; 
                n.y = MazeSystem.MAP_HEIGHT + MazeSystem.Instance().mazeSizeAddition - 2;
                break;
            case 2:
                n.x = MazeSystem.MAP_WIDTH + MazeSystem.Instance().mazeSizeAddition - 2; 
                n.y = MazeSystem.MAP_HEIGHT + MazeSystem.Instance().mazeSizeAddition - 2;
                break;
            case 3:
                n.x = MazeSystem.MAP_WIDTH + MazeSystem.Instance().mazeSizeAddition - 2;
                n.y = 1;
                break;
        }
        _mapData[n.y, n.x].type = t;
        return n;
    }

    /// <summary>
    /// 判断当前方向上是否存在目标类型.
    /// </summary>
    /// <param name="d"> 方向, 0123上左下右 </param>
    /// <returns></returns>
    private bool IsTileTypeByDirection(int d, bool b, Point p)
    {
        switch (d) 
        {
            case 0:
                if(p.y + 2 < _backUpMapData.GetLength(0))
                {
                    return _backUpMapData[p.y + 2, p.x] == b;
                }
                return false;
            case 1:
                if (p.x - 2 >= 0)
                {
                    return _backUpMapData[p.y, p.x - 2] == b;
                }
                return false;
            case 2:
                if (p.y - 2 >= 0)
                {
                    return _backUpMapData[p.y - 2, p.x] == b;
                }
                return false;
            case 3:
                if (p.x + 2 < _backUpMapData.GetLength(1))
                {
                    return _backUpMapData[p.y, p.x + 2] == b;
                }
                return false;
        }
        return false;
    }

    /// <summary>
    /// 随机打破部分墙体. 增强迷宫多样性.
    /// </summary>
    private void BreakWall()
    {
        List<Point> _lst = new List<Point>();

        // 推入所有仅两边联通的墙体.
        for (int i = 2; i < _mapData.GetLength(0) - 2; i++)
        {
            for (int j = 2; j < _mapData.GetLength(1) - 2; j++)
            {
                if (_mapData[i, j].type == MazeTileType.Wall)
                {
                    if (_mapData[i + 1, j].type == MazeTileType.Empty && _mapData[i - 1, j].type == MazeTileType.Empty
                        && _mapData[i, j + 1].type == MazeTileType.Wall && _mapData[i, j - 1].type == MazeTileType.Wall)
                    {
                        _lst.Add(new Point(i, j));
                    }
                    else if (_mapData[i, j + 1].type == MazeTileType.Empty && _mapData[i, j - 1].type == MazeTileType.Empty
                        && _mapData[i + 1, j].type == MazeTileType.Wall && _mapData[i - 1, j].type == MazeTileType.Wall)
                    {
                        _lst.Add(new Point(i, j));
                    }   
                }  
            }
        }

        int maxBreak = _lst.Count / 10;
        int count = UnityEngine.Random.Range(0, maxBreak);
        GK.ShuffleByList(ref _lst);

        for(int i = 0; i < count; i++ )
        {
            _mapData[_lst[i].y, _lst[i].x].type = MazeTileType.Empty;
        }
    }
    #endregion
}

public class Node
{
    public Node() { }

    public Node(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x, y;
}