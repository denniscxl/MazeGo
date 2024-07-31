using GKBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Npc : MonoBehaviour
{
    #region PublicField
    #endregion

    #region PrivateField
    protected int _id;                // npc 类型ID.
    protected int _guid;              // 对象唯一ID.
    protected int _hp;                // 基础血量.
    protected int _curHp;             // 当前血量.
    protected float _speed;           // 基础速度.
    protected float _curSpeed;        // 当前速度.

    protected List<Vector2Int> _pathList = new List<Vector2Int>();    // 移动路径.
    protected UIMazeTileSample _nextTarget = null;
    protected UIMazeTileSample _tile = null;
    #endregion

    #region PublicMethod
    public void SetData(int guid, int id, UIMazeTileSample tile)
    {
        _guid = guid;
        _id = id; 
        _tile = tile;
        _speed = 0.3f;
        Init();
    }

    public void Damage(int hp)
    {
        _curHp += hp;
        if(_curHp <= 0)
        {
            MazeSystem.Instance().MonsterKilled(_guid);
        }
    }
    #endregion

    #region PrivateMethod
    virtual protected void Init()
    {

    }

    protected void Start()
    {
        UIMazes_Main.instance.SetNpcRoot(gameObject, _tile.transform.position);
    }

    protected void InitPath() 
    {
        _pathList = MazeSystem.Instance().FindPath(_tile.row, _tile.col, MazeSystem.Instance().endPoint.y, MazeSystem.Instance().endPoint.x, 9999);
        SetTarget();
    }

    protected void Move()
    {
        Vector3 target = new Vector3(_nextTarget.transform.position.x, _nextTarget.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, target) < 0.01f)
        {
            SetTarget();
        }
    }

    /// <summary>
    /// 设置下一个移动目标.
    /// </summary>
    protected void SetTarget()
    {
        // 到达终点. 
        if (_pathList.Count == 0)
        {
            MazeSystem.Instance().GameOver();
            return;
        }
        _nextTarget = MazeSystem.Instance().mapData[_pathList[0].y, _pathList[0].x].tileSample;
        _pathList.RemoveAt(0);
    }
    #endregion
}
