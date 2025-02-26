using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public AudioSource m_comAudio;
    public AudioClip audioClipHit;
    public AudioClip audioClipDash;
    public AudioClip audioClipJump;

    SpriteRenderer m_comSpriteRenerer;
    Animator m_comAnimator;
    Rigidbody2D m_comRigidbody;
    float m_fJumpForce = 600;
    bool m_bGround = false;

    int m_iSpeed = 3; //스피드 2 : 무빙, 5 : 대쉬
    float m_fMoveX = 0, m_fMoveY = 0;

    public GameObject bulletPrefab;
    public GameObject dashEffectPrefab;

    public int hp { get; set; }
    public int maxHp { get; set; }

    public bool isHit { get; set; }
    public bool isHitRight { get; set; }
    public enum EPlayerState
    {
        Idle,
        Moving,
        Dash,
        Jump,
        Attack,
        End
    }

    private StateMachine stateMachine;
    private Dictionary<EPlayerState, StateObj> dicState = new Dictionary<EPlayerState, StateObj>();

    public bool IsDash()
    {
        return stateMachine.CurrentState.Id == dicState[EPlayerState.Dash].Id;
    }

    private void Awake()
    {
        m_comAudio = GetComponent<AudioSource>();
        m_comSpriteRenerer = GetComponent<SpriteRenderer>();
        m_comAnimator = GetComponent<Animator>();
        m_comRigidbody = GetComponent<Rigidbody2D>();

        StateObj Idle = new PlayerState_Idle(m_comAnimator);
        StateObj Moving = new PlayerState_Moving(m_comAnimator);
        StateObj Dash = new PlayerState_Dash(m_comAnimator);
        StateObj Jump = new PlayerState_Jump(m_comAnimator);
        StateObj Attack = new PlayerState_Attack(m_comAnimator);

        Idle.obj = this.gameObject;
        Moving.obj = this.gameObject;
        Dash.obj = this.gameObject;
        Jump.obj = this.gameObject;
        Attack.obj = this.gameObject;

        dicState.Add(EPlayerState.Idle, Idle);
        dicState.Add(EPlayerState.Moving, Moving);
        dicState.Add(EPlayerState.Dash, Dash);
        dicState.Add(EPlayerState.Jump, Jump);
        dicState.Add(EPlayerState.Attack, Attack);

        stateMachine = new StateMachine(Jump);
    }
    // Start is called before the first frame update
    void Start()
    {
        maxHp = 50;
        hp = maxHp;

        StartCoroutine(DashEffect());
    }

    // Update is called once per frame
    void Update()
    {
        if(true == isHit)
        {
            StartCoroutine(HitEffect());
            PlaySound_Hit();
        }
        MoveControl();
        if (false == stateMachine.CurrentState.IsExcute)
        {
            stateMachine.SetState(dicState[EPlayerState.Idle]);
        }
        stateMachine.DoOperateUpdate();
    }

    void MoveControl()
    {
        m_fMoveX = 0;
        m_fMoveY = 0;

        int iFlipFlag = m_comSpriteRenerer.flipX ? -1 : 1;

        if (stateMachine.CurrentState.Id != dicState[EPlayerState.Attack].Id)
        {
            if (Input.GetKey(KeyCode.F))
            {
                //if (stateMachine.CurrentState.Id == dicState[EPlayerState.Moving].Id)
                stateMachine.SetState(dicState[EPlayerState.Dash]);
            }

            if (stateMachine.CurrentState.Id != dicState[EPlayerState.Dash].Id)
            {
                Jump();

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_comSpriteRenerer.flipX = false;
                    m_fMoveX = m_iSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_comSpriteRenerer.flipX = true;
                    m_fMoveX = -m_iSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    m_fMoveY = m_iSpeed * Time.deltaTime;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    m_fMoveY = -m_iSpeed * Time.deltaTime;
                }

                if (stateMachine.CurrentState.Id != dicState[EPlayerState.Jump].Id)
                {
                    if (m_fMoveX == 0 && m_fMoveY == 0)
                    {
                        stateMachine.SetState(dicState[EPlayerState.Idle]);
                    }
                    else
                    {
                        stateMachine.SetState(dicState[EPlayerState.Moving]);
                    }
                }
                else
                {
                    if (true == m_bGround)
                        stateMachine.CurrentState.IsExcute = false;
                }
            }
            else
            {
                m_fMoveX = iFlipFlag * 3 * m_iSpeed * Time.deltaTime;
            }

            this.transform.Translate(new Vector3(m_fMoveX, m_fMoveY, 0));
        }

        if (stateMachine.CurrentState.Id == dicState[EPlayerState.Idle].Id)
        {
            if (Input.GetKey(KeyCode.A))
            {
                Vector3 vPos = transform.position;
                vPos.x += 0.5f * iFlipFlag;
                vPos.y += 0.25f;
                GameObject obj = Instantiate(bulletPrefab, vPos, transform.rotation) as GameObject;

                Rigidbody2D comRigidbody = obj.GetComponent<Rigidbody2D>();
                comRigidbody.AddForce(new Vector2(iFlipFlag * 700, 0));

                SpriteRenderer comSpriteRenerer = obj.GetComponent<SpriteRenderer>();
                comSpriteRenerer.flipX = m_comSpriteRenerer.flipX;

                stateMachine.SetState(dicState[EPlayerState.Attack]);
            }
        }
    }
    void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && true == m_bGround)
        {
            m_bGround = false;
            stateMachine.SetState(dicState[EPlayerState.Jump]);
            
            m_comRigidbody.velocity = Vector2.zero;
            m_comRigidbody.AddForce(new Vector2(0, m_fJumpForce));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        m_bGround = true;
    }

    IEnumerator HitEffect() //코루틴을 이용한 깜박임
    {
        isHit = false;
        
        bool bShowImage = true;
        float fShackSpeed = 20.0f * 0.01f;        
        for (int i = 0; i < 3; ++i)
        {
            int iFlip = (isHitRight ? -1 : 1);
            if (bShowImage)
            {
                m_comSpriteRenerer.color = new Color(1, 1, 1, 1);
                this.transform.Translate(Vector3.up * fShackSpeed);
                this.transform.Translate(iFlip * Vector3.right * fShackSpeed);
            }
            else
            {
                m_comSpriteRenerer.color = new Color(1, 0, 0, 1);
                this.transform.Translate(-Vector3.up * fShackSpeed);
                this.transform.Translate(iFlip * Vector3.right * fShackSpeed);
            }

            bShowImage = !bShowImage;
            yield return new WaitForSeconds(0.05f);
        }
        StartCoroutine(HitEffect2());
    }

    IEnumerator HitEffect2() //코루틴을 이용한 깜박임
    {
        bool bShowImage = true;

        m_comSpriteRenerer.color = new Color(1, 1, 1, 0.50f);
        for (int i = 0; i < 3; ++i)
        {
            if (bShowImage)
            {
                m_comSpriteRenerer.color = new Color(1, 1, 1, 1);
                //this.transform.Translate(Vector3.right * fShackSpeed * Time.deltaTime);
            }
            else
            {
                m_comSpriteRenerer.color = new Color(1, 1, 1, 0.50f);
                //this.transform.Translate(-Vector3.right * fShackSpeed * Time.deltaTime);
            }

            bShowImage = !bShowImage;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator DashEffect() //코루틴을 이용한 깜박임
    {
        while (true)
        {
            if (stateMachine.CurrentState.Id == dicState[EPlayerState.Dash].Id)
            {
                Vector3 vPos = transform.position;
                //vPos.x += 0.5f * m_comSpriteRenerer.flipX;
                vPos.y -= 0.5f;
                GameObject obj = Instantiate(dashEffectPrefab, vPos, transform.rotation) as GameObject;

                SpriteRenderer comSpriteRenerer = obj.GetComponent<SpriteRenderer>();
                comSpriteRenerer.flipX = m_comSpriteRenerer.flipX;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    public void PlaySound_Hit()
    {
        m_comAudio.clip = audioClipHit;
        m_comAudio.Play();
    }
    public void PlaySound_Dash()
    {
        m_comAudio.clip = audioClipDash;
        m_comAudio.Play();
    }
    public void PlaySound_Jump()
    {
        m_comAudio.clip = audioClipJump;
        m_comAudio.Play();
    }
}
