using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Bullet : MonoBehaviour
{
    SpriteRenderer m_comSpriteRenerer;
    public GameObject HitEffectPrefab;

    private void Awake()
    {
        m_comSpriteRenerer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 2.0f);
    }
    void Update()
    {
    }
    void Hit()
    {
        Vector3 vPos = transform.position;
        GameObject obj = Instantiate(HitEffectPrefab, vPos, transform.rotation) as GameObject;

        SpriteRenderer comSpriteRenerer = obj.GetComponent<SpriteRenderer>();
        comSpriteRenerer.flipX = m_comSpriteRenerer.flipX;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Monster")
        {
            // Destroy(collision.gameObject);
            MonsterAI insScript = collision.gameObject.GetComponent<MonsterAI>();
            insScript.isHit = true;
            insScript.isHitRight = (collision.gameObject.transform.position.x < this.gameObject.transform.position.x);
            insScript.hp -= 1;
            
            Hit();
            Destroy(gameObject);
        }
    }
    //SpriteRenderer m_comSpriteRenerer;
    //bool m_bDirRight = true;
    //int m_iTeamFlag = 0;
    //float m_fSpeed = 30;
    //Vector2 m_vPos  = Vector2.zero;
    //public void SetData(bool _bDirRight, int _iTeamFlag, Vector2 vPos, float _fSpeed)
    //{
    //    m_bDirRight = _bDirRight;
    //    m_iTeamFlag = _iTeamFlag;
    //    m_vPos = vPos;
    //    m_fSpeed = _fSpeed;
    //    transform.position = new Vector3(m_vPos.x, m_vPos.y, transform.position.z);
    //}
    //private void Awake()
    //{

    //}
    //// Start is called before the first frame update
    //void Start()
    //{
    //    Destroy(gameObject, 2.0f);
    //    m_comSpriteRenerer = GetComponent<SpriteRenderer>();
    //    m_comSpriteRenerer.flipX = !m_bDirRight;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    int iDir = m_bDirRight ? 1 : -1;
    //    this.transform.Translate(new Vector3(iDir * m_fSpeed * Time.deltaTime, 0, 0));
    //}
}
