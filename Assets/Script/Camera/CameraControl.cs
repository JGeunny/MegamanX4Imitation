using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject target;  //A라는 GameObject변수 선언
    Transform trans;
    void Start()
    {
        trans = target.transform;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(trans.position.x, 0, transform.position.z);
    }
}
