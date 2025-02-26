using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBullet : MonoBehaviour
{
    SpriteRenderer m_comSpriteRenerer;
    public GameObject HitEffectPrefab;

    private void Awake()
    {
        m_comSpriteRenerer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, 1.0f);
    }
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Destroy(collision.gameObject);
            PlayerControl insScript = collision.gameObject.GetComponent<PlayerControl>();
            if (false == insScript.IsDash())
            {
                insScript.hp -= 1;
                insScript.isHit = true;
                insScript.isHitRight = (collision.gameObject.transform.position.x < this.gameObject.transform.position.x);
                Hit();
            }
            Destroy(gameObject);
        }
    }
    void Hit()
    {
        Vector3 vPos = transform.position;
        GameObject obj = Instantiate(HitEffectPrefab, vPos, transform.rotation) as GameObject;

        SpriteRenderer comSpriteRenerer = obj.GetComponent<SpriteRenderer>();
        comSpriteRenerer.flipX = m_comSpriteRenerer.flipX;
    }
}
