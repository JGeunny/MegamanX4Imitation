using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState_Idle : StateObj
{
    protected Animator m_comAnimator = null;
    public MonsterState_Idle(Animator _comAnimator)
    {
        Id = 1;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        IsExcute = true;
    }
    public override void OperateExit()
    {
        IsExcute = false;
    }
    public override void OperateUpdate()
    {
    }
}
