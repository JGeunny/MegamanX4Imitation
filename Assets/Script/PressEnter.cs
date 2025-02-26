using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressEnter : MonoBehaviour
{
    private AudioSource m_comAudio;
    private Image m_comImage;

    // Use this for initialization
    void Start()
    {
        m_comAudio = GetComponent<AudioSource>();
        m_comImage = GetComponent<Image>();
        StartCoroutine("BlinkImage");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    IEnumerator BlinkImage() //코루틴을 이용한 깜박임
    {
        bool bShowImage = true;
        while (true)
        {
            if (bShowImage)
                m_comImage.color = new Color(1, 1, 1, 1);
            else
                m_comImage.color = new Color(1, 1, 1, 0);

            bShowImage = !bShowImage;
            yield return new WaitForSeconds(0.5f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
            ChangeScene();
    }
    private void ChangeScene()
    {
        m_comAudio.Play();
        SceneManager.LoadScene("Stage01");
    }

}
