using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    Animator m_comAnimator;
    // Start is called before the first frame update
    void Start()
    {
        m_comAnimator = GetComponent<Animator>();
        //StartCoroutine(WaitForAnimation(m_comAnimator));
    }

    // Update is called once per frame
    void Update()
    {
        if (false == (m_comAnimator.GetCurrentAnimatorStateInfo(0).length
            > m_comAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime))
        {
            Destroy(gameObject);
        }
    }
    //IEnumerator WaitForAnimation(Animator animator)
    //{
    //    //while (true == animator.GetCurrentAnimatorStateInfo(0).IsName(name)) {
    //    //while (false == animator.IsInTransition(0))
    //    while (animator.GetCurrentAnimatorStateInfo(0).length
    //        > animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
    //    {
    //        yield return new WaitForEndOfFrame();
    //    }
    //    GameObject.Destroy(animator.gameObject);

    //}
}
