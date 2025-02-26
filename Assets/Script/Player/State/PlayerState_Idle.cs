using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Idle : StateObj
{
    protected Animator m_comAnimator = null;
    public PlayerState_Idle(Animator _comAnimator)
    {
        Id = 1;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        IsExcute = true;
        m_comAnimator.SetBool("isDash", false);
        m_comAnimator.SetBool("isMoving", false);
    }
    public override void OperateExit()
    {
        IsExcute = false;
    }
    public override void OperateUpdate()
    {
    }
}
