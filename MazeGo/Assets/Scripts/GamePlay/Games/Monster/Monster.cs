using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Npc
{
    #region PublicField
    #endregion

    #region PrivateField
    #endregion

    #region PublicMethod
    #endregion

    #region PrivateMethod
    protected void Start()
    {
        base.Start();
        InitPath();
    }

    protected void Update()
    {
        Move();
    }
    #endregion
}
