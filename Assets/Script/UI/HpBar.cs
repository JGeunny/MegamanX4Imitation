using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Slider hpBarControl;
    SpriteRenderer spriteRenderer;
    GameObject objPlayer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        objPlayer = GameObject.Find("Player");
        StartCoroutine(PlayerHp());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl insScript = objPlayer.GetComponent<PlayerControl>();
        hpBarControl.value = insScript.hp / (float)insScript.maxHp;
    }

    IEnumerator PlayerHp()
    {
        while(true)
        {
            //PlayerControl insScript = objPlayer.GetComponent<PlayerControl>();
            //hpBarControl.value = insScript.hp / (float)insScript.maxHp;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
