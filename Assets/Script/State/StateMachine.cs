using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public StateObj CurrentState{ get; private set; }

    public StateMachine(StateObj _defaultStateObj)
    {
        CurrentState = _defaultStateObj;
        CurrentState.OperateEnter();
    }

    public void SetState(StateObj _stateObj)
    {
        //if (CurrentState == _stateObj)
        //if(true == OutOfMemoryException.Equals(CurrentState, _stateObj))
        if (CurrentState.Id == _stateObj.Id)
        {
            Debug.Log("이미 사용중인 상태");
            return;
        }

        CurrentState.OperateExit();
        CurrentState = _stateObj;
        CurrentState.OperateEnter();
    }

    public void DoOperateUpdate()
    {
        CurrentState.OperateUpdate();
    }
}
