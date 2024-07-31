using GKBase;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 塔防模式下, 怪物巢穴.
/// </summary>

public class MazeNest : MonoBehaviour
{
    #region PublicField
    #endregion

    #region PrivateField
    [SerializeField]
    private int _id = -1;        // 怪物ID.
    private int _guid = -1;
    private UIMazeTileSample _tile;
    private bool _isTimeOver = false;

    private float _curSpawnTime = 99;
    #endregion

    #region PublicMethod
    public void SetData(int guid, UIMazeTileSample tile)
    {
        _guid = guid;
        _tile = tile;
        _id = 0;
    }
    #endregion

    #region PrivateMethod
    // Start is called before the first frame update
    void Start()
    {
        if (_tile != null)
        {
            GK.SetParent(gameObject, _tile.gameObject, false);
        }
        MazeSystem.Instance().OnTDTimeOverEvent += DTTimeOver;
    }

    // Update is called once per frame
    void Update()
    {
        // 临时生成时间和生成对象. 后续逻辑晚些补充.
        if(MazeSystem.Instance().GetGameStep() == MazeGameplayStep.TownDefense 
            && !MyGame.Instance.isPause && !_isTimeOver && null != _tile)
        {
            if(_curSpawnTime < 5)
            {
                _curSpawnTime += Time.deltaTime;
            }
            else
            {
                _curSpawnTime = 0;
                MazeSystem.Instance().SpawnMonsters(_id, _tile.row, _tile.col);
            }
        }
    }

    private void DTTimeOver()
    {
        _isTimeOver = true;
    }
    #endregion


}
