using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GKBase;
using System;

namespace AStar
{
    /// <summary>
    /// 2024/05/27
    /// AStar算法工具类.
    /// 采用SortDictionary的排序(红黑树).
    /// A* 最短路径搜索
    /// </summary>
    public class AStarRedBlackSearch
    {
        private List<bool> Map;
        private int Width;
        private int Height;
        private bool IsValidPosition(int start)
        {
            return start >= 0 && start <= Map.Count;
        }
        public AStarRedBlackSearch(List<bool> map, int Width)
        {
            if (map == null) throw new ArgumentNullException();

            Map = map;
            this.Width = Width;
            this.Height = Map.Count / Width;
        }

        private SortedDictionary<int, int> Open;
        private SortedDictionary<int, int> Close;

        public List<int> FindPath(int start, int end)
        {
            if (!IsValidPosition(start) || !IsValidPosition(end)) throw new ArgumentOutOfRangeException();
            this.Start = start; this.End = end;
            Open = new SortedDictionary<int, int>();

            Close = new SortedDictionary<int, int>();
            GScore = new Dictionary<int, int>();
            FScore = new Dictionary<int, int>();
            ComeFrom = new Dictionary<int, int>();

            // 将开始节点入队列
            Open.Add(start, GetFScore(start));

            int x = start;
            while (Open.Count > 0)
            {
                x = GetLowestF();
                if (x == End)
                {
                    // Trace From
                    return ReconstructPath(ComeFrom, x); ;
                }

                Open.Remove(x);
                Close.Add(x, GetFScore(x));

                foreach (int y in GetNodesAround(x))
                {
                    if (Close.ContainsKey(y))
                    {
                        continue;
                    }

                    int newGValue = GetCost(x) + GetDistance(x, y);
                    bool newIsBetter = false;

                    if (!Open.ContainsKey(y))
                    {
                        Open.Add(y, GetFScore(y));
                        newIsBetter = true;
                    }
                    else if (newGValue < GScore[y])
                        newIsBetter = true;


                    if (newIsBetter)
                    {
                        if (ComeFrom.ContainsKey(y))
                            ComeFrom[y] = x;
                        else
                            ComeFrom.Add(y, x);

                        GScore[y] = newGValue;
                        FScore[y] = GScore[y] + GetHeuristic(y);
                    }

                }
            }

            return null;
        }

        private List<int> ReconstructPath(Dictionary<int, int> cameFrom, int current)
        {
            var path = new List<int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        private int Start;
        private int End;


        private IList<int> GetNodesAround(int pos)
        {
            List<int> list = new List<int>(4);
            int x = pos % Width; int y = pos / Width;
            // Left
            if (x > 0 && !Map[x - 1 + y * Width]) list.Add(x - 1 + y * Width);
            // Up
            if (y > 0 && !Map[x + (y - 1) * Width]) list.Add(x + (y - 1) * Width);
            // Right
            if (x < Width - 1 && !Map[x + 1 + y * Width]) list.Add(x + 1 + y * Width);
            // Down
            if (y < Height - 1 && !Map[x + (y + 1) * Width]) list.Add(x + (y + 1) * Width);

            return list;
        }

        private int GetCost(int current)
        {
            // UNDONE
            int xDistance = (int)Math.Abs(Start % Width - current % Width);
            int yDistance = (int)Math.Abs(Start / Width - current / Width);

            if (!GScore.ContainsKey(current))
                GScore.Add(current, (xDistance + yDistance) * 10);

            return GScore[current];
        }

        private int GetLowestF()
        {
            int lowest = -1;
            int temp = 0;
            foreach (var item in Open)
            {
                if (lowest == -1 || temp > item.Value)
                {
                    lowest = item.Key;
                    temp = item.Value;
                }
            }

            return lowest;
        }

        private int GetFScore(int pos)
        {
            if (!FScore.ContainsKey(pos))
                FScore.Add(pos, GetCost(pos) + GetHeuristic(pos));

            return FScore[pos];
        }

        private Dictionary<int, int> GScore;
        private Dictionary<int, int> FScore;
        private Dictionary<int, int> ComeFrom;

        // 得到预估的距离
        private int GetHeuristic(int current)
        {
            return GetDistance(current, End);
        }

        private int GetDistance(int x, int y)
        {
            int xDistance = (int)Math.Abs(y % Width - x % Width);
            int yDistance = (int)Math.Abs(y / Width - x / Width);

            return 10 * (xDistance + yDistance);

        }
    }
}
