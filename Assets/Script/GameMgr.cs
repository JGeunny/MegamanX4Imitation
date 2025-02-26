using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    private AudioSource m_comAudio;
    bool bSoundTrigger = true;

    GameObject player;
    public GameObject MonsterPrefab;
    // Start is called before the first frame update

    private static SceneMgr m_clsInstance = null;

    public float m_fLimitTime;
    public Text textTimer;

    public static SceneMgr GetInstance()
    {
        if (m_clsInstance == null)
        {
            m_clsInstance = GameObject.FindObjectOfType<SceneMgr>();
        }
        return m_clsInstance;
    }

    void Start()
    {
        m_comAudio = GetComponent<AudioSource>();
        player = GameObject.Find("Player");
        m_fLimitTime = 120.0f;
        StartCoroutine(CreateMonster());
    }

    // Update is called once per frame
    void Update()
    {
        m_fLimitTime -= Time.deltaTime;
        if (m_fLimitTime < 0)
        {
            m_fLimitTime = 0.0f;
            ChangeScene();
        }

        
        int iTime = (int)Mathf.Round(m_fLimitTime);

        if(30 == iTime && true == bSoundTrigger)
        {
            bSoundTrigger = false;
            m_comAudio.Play();
        }
        textTimer.text = "" + iTime;

        if (Input.GetKey(KeyCode.Return))
            ChangeScene();
    }

    IEnumerator CreateMonster() //코루틴을 이용한 깜박임
    {
        while(true)
        {
            int iCreateCount = (int)(5.0f - (m_fLimitTime / 30));
            if (iCreateCount > 3) iCreateCount = 3;
            for (int i = 0; i < iCreateCount; ++i)
            {
                Vector3 vCreatePos = player.transform.position;
                vCreatePos.x += (Random.Range(0, 2) == 0 ? -1 : 1) * Random.Range(2.0f, 3.5f);
                vCreatePos.y = 0.0f;

                Instantiate(MonsterPrefab, vCreatePos, transform.rotation);
            }
             yield return new WaitForSeconds(2.0f);
        }

    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Ending");
    }
}
