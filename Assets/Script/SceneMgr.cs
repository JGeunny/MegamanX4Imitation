using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMgr : MonoBehaviour
{
    private static SceneMgr m_clsInstance = null;

    public static SceneMgr GetInstance()
    {
        if (m_clsInstance == null)
        {
            m_clsInstance = GameObject.FindObjectOfType<SceneMgr>();
        }
        return m_clsInstance;
    }
    public enum ESelectScene
    {
        Title,
        Stage01,
        End
    }

    Dictionary<ESelectScene, string> m_dicSceneInfo =
            new Dictionary<ESelectScene, string>();
    private void Ready_SceneMgr()
    {
        m_dicSceneInfo.Add(ESelectScene.Title, "Title");
        m_dicSceneInfo.Add(ESelectScene.Stage01, "Stage01");
        m_dicSceneInfo.Add(ESelectScene.End, "Ending");
    }

    private void Awake()
    {
        GetInstance();
        Ready_SceneMgr();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ChangeScene(ESelectScene eSelectScene)
    {
        if (false == m_dicSceneInfo.ContainsKey(eSelectScene))
            return false;
        SceneManager.LoadScene(m_dicSceneInfo[eSelectScene]);
        return true;
    }
}
