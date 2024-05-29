using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GKStateMachine;
using GKUI;

class GKCameraUICenterState : GKStateMachineStateBase<MachineStateID> {

    private CameraController _controller;

    public GKCameraUICenterState() : base(MachineStateID.Follow)
    {

    }

    override public void Enter()
    {
        _controller = CameraController.Instance();
    }

    override public void Exit()
    {
        if(null != _controller.GetFocus())
           _controller.SetTargetPos(_controller.GetFocus().position.x, _controller.GetFocus().position.z);
    }

    override public MachineStateID Update()
    {
        if (null == UIController.instance.m_camera || null == _controller.GetFocus())
            return ID;

        Transform t = UIController.instance.m_camera.transform;
        Vector3 pos =  new Vector3(_controller.GetFocus().position.x, _controller.GetFocus().position.y, t.position.z) ;
        UIController.instance.m_camera.transform.position = Vector3.Lerp(t.position, pos, Time.deltaTime * _controller.GetMoveSpeed());
        
        return ID;
    }

}
