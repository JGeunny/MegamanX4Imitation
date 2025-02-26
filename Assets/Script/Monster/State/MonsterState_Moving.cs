using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Moving : StateObj
{
    protected Animator m_comAnimator = null;
    public MonsterState_Moving(Animator _comAnimator)
    {
        Id = 2;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        IsExcute = true;
        m_comAnimator.SetBool("isMoving", true);
    }
    public override void OperateExit()
    {
        IsExcute = false;
        m_comAnimator.SetBool("isMoving", false);
    }
    public override void OperateUpdate()
    {
    }
}
