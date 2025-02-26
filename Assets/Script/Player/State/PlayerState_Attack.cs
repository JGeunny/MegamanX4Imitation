using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Attack : StateObj
{
    protected Animator m_comAnimator = null;
    public PlayerState_Attack(Animator _comAnimator)
    {
        Id = 5;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        IsExcute = true;
        m_comAnimator.SetBool("isAttack", true);
    }
    public override void OperateExit()
    {
        IsExcute = false;
        m_comAnimator.SetBool("isAttack", false);
    }
    public override void OperateUpdate()
    {
        if (false == (m_comAnimator.GetCurrentAnimatorStateInfo(0).length > m_comAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime))
        {
            IsExcute = false;
        }
    }
}
