using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GKBase;
using GKData;
using GKFOW;

public class LevelController : MonoBehaviour
{
    #region PublicField
    
    #endregion

    #region PrivateField
    private static LevelController _instance = null;
    [SerializeField]
    private GameType _gameType = GameType.Maze;
    #endregion

    #region PublicMethod
    public static LevelController Instance() { return _instance; }

    public void InitData()
    {
        switch (_gameType)
        {
            case GameType.Maze:
                MazeSystem.Instance().GenerateMaze();
                UIMazes_Main.Open();
                break;
            default:
                break;
        }
    }

    public void Exit()
    {
        switch (_gameType)
        {
            case GameType.Maze:
                MazeSystem.Instance().GameOver();
                UIMazes_Main.Close();
                break;
            default:
                break;
        }
    }
    #endregion

    #region PrivateMethod
    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    protected void Start()
    {
        // 初始化游戏内阵营数据.
        InitData();
    }

    // 关卡结束后释放链接, 避免内存泄漏.
    private void OnDestroy()
    {
    }

    // 每局游戏开始时初始化游戏数据.
    

    #endregion
}

/// <summary>
/// 游戏类型.
/// </summary>
public enum GameType
{
    Maze = 0,   // 迷宫.
}
