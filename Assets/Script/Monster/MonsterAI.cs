using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public AudioSource m_comAudioHit;

    SpriteRenderer m_comSpriteRenerer;
    Animator m_comAnimator;
    Rigidbody2D m_comRigidbody;

    int m_iSpeed = 2; //스피드 2 : 무빙
    float m_fMoveX = 0, m_fMoveY = 0;
    public int hp{ get; set; }

    GameObject targetObj = null;
    public GameObject bulletPrefab;
    bool m_bFireReady = false;
    bool m_bFindTarget = false;

    public GameObject DeadEffectPrefab;
    public bool isHit { get; set; }
    public bool isHitRight { get; set; }

    public enum EMonsterState
    {
        Idle,
        Moving,
        Attack,
        End
    }
    public enum EMonsterHavior
    {
        Search,
        Chasing,
        Attack,
        Retreat,
        End
    }

    EMonsterHavior m_eHavior = EMonsterHavior.Search;
    private StateMachine stateMachine;
    private Dictionary<EMonsterState, StateObj> dicState = new Dictionary<EMonsterState, StateObj>();

    private void Awake()
    {
        m_comAudioHit = GetComponent<AudioSource>();
        m_comSpriteRenerer = GetComponent<SpriteRenderer>();
        m_comAnimator = GetComponent<Animator>();
        m_comRigidbody = GetComponent<Rigidbody2D>();

        StateObj Idle = new MonsterState_Idle(m_comAnimator);
        StateObj Moving = new MonsterState_Moving(m_comAnimator);
        StateObj Attack = new MonsterState_Attack(m_comAnimator);

        dicState.Add(EMonsterState.Idle, Idle);
        dicState.Add(EMonsterState.Moving, Moving);
        dicState.Add(EMonsterState.Attack, Attack);

        stateMachine = new StateMachine(Idle);
    }

    // Start is called before the first frame update
    void Start()
    {
        targetObj = GameObject.Find("Player");
        StartCoroutine("ChangeState");
        hp = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (true == isHit)
        {
            StartCoroutine(HitEffect());
            m_comAudioHit.Play();
        }
        MoveControl();
        if (false == stateMachine.CurrentState.IsExcute)
        {
            stateMachine.SetState(dicState[EMonsterState.Idle]);
        }
        stateMachine.DoOperateUpdate();

        if(hp <= 0)
        {
            Instantiate(DeadEffectPrefab, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

    void MoveControl()
    {
        m_fMoveX = 0;
        m_fMoveY = 0;

        int iFlipFlag = m_comSpriteRenerer.flipX ? -1 : 1;
        float fDist = Vector3.Distance(this.transform.position, targetObj.transform.position);

        if(fDist > 4.0f)
        {
            if (false == m_bFindTarget)
            {
                //수색
                m_eHavior = EMonsterHavior.Search;
            }
            else
            {
                //추격
                m_eHavior = EMonsterHavior.Chasing;
            }
        }
        else if (fDist > 3.0f)
        {
            m_bFindTarget = true;
            //공격
            m_eHavior = EMonsterHavior.Attack;            
        }
        else
        {
            //퇴각
            m_eHavior = EMonsterHavior.Retreat;
        }

        if (stateMachine.CurrentState.Id == dicState[EMonsterState.Moving].Id)
        {
            m_fMoveX = iFlipFlag * m_iSpeed * Time.deltaTime;
        }
        this.transform.Translate(new Vector3(m_fMoveX, m_fMoveY, 0));

        if (stateMachine.CurrentState.Id != dicState[EMonsterState.Attack].Id)
        {
            if (true == m_bFireReady)
            {
                m_bFireReady = false;
                Vector3 vPos = transform.position;
                vPos.x += 0.65f * iFlipFlag;
                vPos.y += 0.4f;
                GameObject obj = Instantiate(bulletPrefab, vPos, transform.rotation) as GameObject;

                Rigidbody2D comRigidbody = obj.GetComponent<Rigidbody2D>();
                comRigidbody.AddForce(new Vector2(iFlipFlag * 250, 0));

                SpriteRenderer comSpriteRenerer = obj.GetComponent<SpriteRenderer>();
                comSpriteRenerer.flipX = m_comSpriteRenerer.flipX;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerControl insScript = collision.gameObject.GetComponent<PlayerControl>();
            if (true == insScript.IsDash())
            {
                isHit = true;
                hp -= 3;
            }
        }
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            if (stateMachine.CurrentState.Id == dicState[EMonsterState.Attack].Id)
            {
                yield return new WaitForSeconds(0.2f);
                continue;
            }
            float fDir = this.transform.position.x - targetObj.transform.position.x;
            bool bFlipDir = (fDir >= 0);

            switch (m_eHavior)
            {
                case EMonsterHavior.Search:
                    int rand = Random.Range(0, 2);
                    stateMachine.SetState(dicState[(EMonsterState)rand]);
                    if (stateMachine.CurrentState.Id == dicState[EMonsterState.Moving].Id)
                    {
                        int randDir = Random.Range(0, 2);
                        m_comSpriteRenerer.flipX = (randDir == 1) ? true : false;
                    }
                    break;
                case EMonsterHavior.Chasing:
                    stateMachine.SetState(dicState[EMonsterState.Moving]);
                    m_comSpriteRenerer.flipX = bFlipDir;
                    break;
                case EMonsterHavior.Attack:
                    stateMachine.SetState(dicState[EMonsterState.Attack]);
                    m_comSpriteRenerer.flipX = bFlipDir;
                    m_bFireReady = true;
                    break;
                case EMonsterHavior.Retreat:
                    stateMachine.SetState(dicState[EMonsterState.Moving]);
                    m_comSpriteRenerer.flipX = !bFlipDir;
                    break;
                case EMonsterHavior.End:
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
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
    }
}

