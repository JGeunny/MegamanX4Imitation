using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState_Jump : StateObj
{
    protected Animator m_comAnimator = null;
    public PlayerState_Jump(Animator _comAnimator)
    {
        Id = 4;
        m_comAnimator = _comAnimator;
    }
    public override void OperateEnter()
    {
        PlayerControl playerScript = obj.GetComponent<PlayerControl>();
        playerScript.PlaySound_Jump();

        IsExcute = true;
        m_comAnimator.SetBool("isJump", true);
    }
    public override void OperateExit()
    {
        IsExcute = false;
        m_comAnimator.SetBool("isJump", false);
    }
    public override void OperateUpdate()
    {

    }
}
