using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateObj : MonoBehaviour
{
    public GameObject obj;
    public int Id { get; protected set; }
    public bool IsExcute { get; set; }

    public abstract void OperateEnter();
    public abstract void OperateExit();
    public abstract void OperateUpdate();
}