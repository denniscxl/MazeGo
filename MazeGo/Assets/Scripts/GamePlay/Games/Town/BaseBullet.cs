using GKBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameData;

public class BaseBullet : MonoBehaviour
{
    #region PublicField
    #endregion

    #region PrivateFielda
    private MazeTownData _data;
    private Town _town = null;
    private GameObject _target;
    private float _curTime = 0;
    #endregion

    #region PublicMethod
    public void Init(Town town, MazeTownData data, GameObject target)
    {
        _town = town;
        _data = data;
        _target = target;
        _curTime = 0;
    }
    #endregion

    #region PrivateMethod
    private void Start()
    {
        UIMazes_Main.instance.SetNpcRoot(gameObject, _town.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (null == _data)
            return;

        if(_curTime < _data.time)
        {
            _curTime += Time.deltaTime;
            Move();
        }
        else
        {
            GK.Destroy(gameObject);
        }
    }

    protected virtual void Move()
    {
        if (null == _target)
            return;

        Vector3 target = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, target, _data.bulletSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            Damage();
        }
    }

    protected virtual void Damage()
    {
        if(1 == _data.targetType)
        {
            if(null != _target)
            {
                _target.GetComponent<Npc>().Damage(_data.damage);
            }
        }
        else if(2 == _data.targetType)
        {
            List<Npc> lst = MazeSystem.Instance().GetMonsterByRange(transform.position, _data.bulletRange);
            foreach (var n in lst)
            {
                n.Damage(_data.damage);
            }
        }
        GK.Destroy(gameObject);
    }
    #endregion

}
