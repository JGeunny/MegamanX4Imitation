using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Dash : StateObj
{
    protected Animator m_comAnimator = null;
    public PlayerState_Dash(Animator _comAnimator)
    {
        Id = 3;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        PlayerControl playerScript = obj.GetComponent<PlayerControl>();
        playerScript.PlaySound_Hit();

        IsExcute = true;
        m_comAnimator.SetBool("isDash", true);
    }
    public override void OperateExit()
    {
        IsExcute = false;
        m_comAnimator.SetBool("isDash", false);
    }
    public override void OperateUpdate()
    {
        if(false == (m_comAnimator.GetCurrentAnimatorStateInfo(0).length > m_comAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime))
        {
            IsExcute = false;
        }
    }
}
