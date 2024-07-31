using GKBase;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameData;
using static UnityEditor.PlayerSettings;

/// <summary>
/// 防御塔基类.
/// </summary>
public class Town : Npc
{
    #region PublicField
    #endregion

    #region PrivateField
    private MazeTownData _data;
    private float _curTime;
    private bool _isTimeOver = false;
    #endregion

    #region PublicMethod

    #endregion

    #region PrivateMethod
    protected void Start()
    {
        base.Start();
        MazeSystem.Instance().OnTDTimeOverEvent += DTTimeOver;

    }

    protected void Update()
    {
        if (null == _data || _isTimeOver)
            return;

        if(_curTime > _data.atkSpeed)
        //if (_curTime > 0.1f)
        {
            _curTime = 0;
            Shoot();
        }
        else
        {
            _curTime += Time.deltaTime;
        }
    }

    override protected void Init()
    {
        _data = DataController.Data.GetMazeTownData(_id);
    }

    virtual protected void Shoot()
    {
        Debug.Log("Shoot");

        GameObject obj = GK.TryLoadGameObject(string.Format("Prefabs/Maze/Bullets/{0}", _data.bulletType));
        if (null != obj)
        {
            GameObject t = FindTarget();
            if(null != t)
            {
                BaseBullet bullet = obj.GetOrAddComponent<BaseBullet>();
                if (null != bullet)
                {
                    bullet.Init(this, _data, t);
                }
            }
        }
    }

    virtual protected GameObject FindTarget()
    {
        if (null == _data)
            return null;

        List<Npc> lst = MazeSystem.Instance().GetMonsterByRange(transform.position, _data.range);
        if (0 == lst.Count)
            return null;

        float minDistance = 0;
        int minIdx = 0;
        for (int i = 0; i < lst.Count; i++)
        {
            float d = Vector3.Distance(transform.position, lst[i].transform.position);
            if (d < minDistance)
            {
                minDistance = d;
                minIdx = i;
            }

        }
        return lst[minIdx].gameObject;
    }

    private void DTTimeOver()
    {
        _isTimeOver = true;
    }
    #endregion
}
