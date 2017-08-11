using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Zombie : MonoBehaviour
{
    enum ENEMYSTATE
    {
        NONE = -1,
        IDLE = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEAD
    }

    ENEMYSTATE enemyState = ENEMYSTATE.IDLE;

    float stateTime = 0.0f;
    public float idleStateMaxTime = 2.0f;
    public float attackStateMaxTime = 2.0f;
    public float DamageStateMaxTime = 1.0f;
    public Animation anim;
    Transform target = null;
    public CharacterController characterController;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float attackRange = 2.5f;

    public int healthPoint = 5;
    int originHp;
    //public Image imgHp;

    //public GameObject explosionParticle = null;
    //public GameObject deadObject = null;

    Vector3 pos;

    void PlayIdleFromDamage()
    {
        anim.CrossFade("Idle");
    }

    void Awake()
    {
        pos = transform.position;
        originHp = healthPoint;
        InitZombie();
    }
    void OnEnable()
    {
        transform.position = pos;
        InitZombie();
    }
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void InitZombie()
    {
        if(characterController != null)
            characterController.enabled = true;
        healthPoint = originHp;
        //imgHp.fillAmount = (float)healthPoint / (float)originHp;
        //imgHp.color = Color.Lerp(Color.red, Color.green, healthPoint / 5.0f);

        enemyState = ENEMYSTATE.IDLE;
        PlayIdleAnim();
    }
    void PlayIdleAnim()
    {
        anim["Idle"].speed = 3.0f;
        anim.Play("Idle");
    }
    void Update()
    {
        switch (enemyState)
        {
            case ENEMYSTATE.IDLE:
                {
                    stateTime += Time.deltaTime;
                    if(stateTime > idleStateMaxTime)
                    {
                        stateTime = 0.0f;
                        enemyState = ENEMYSTATE.MOVE;
                    }
                }
                break;
            case ENEMYSTATE.MOVE:
                {
                    //target = GameObject.FindGameObjectWithTag("Player").transform;
                    anim["Move"].speed = 2.0f;
                    anim.CrossFade("Move");

                    float disrance = (target.position - transform.position).magnitude;
                    if (disrance < attackRange)
                    {
                        enemyState = ENEMYSTATE.ATTACK;
                        stateTime = attackStateMaxTime;
                    }
                    else
                    {
                        Vector3 dir = target.position - transform.position;
                        dir.y = 0.0f;
                        dir.Normalize();
                        characterController.SimpleMove(dir * moveSpeed); 

                        transform.rotation = Quaternion.Lerp(transform.rotation,
                                                         Quaternion.LookRotation(dir), 
                                                        rotationSpeed * Time.deltaTime);
                    }
                }
                break;
            case ENEMYSTATE.ATTACK:
                {
                    float disrance = (target.position - transform.position).magnitude;
                    stateTime += Time.deltaTime;
                    if (stateTime > attackStateMaxTime)
                    {
                        Vector3 dir = target.position - transform.position;
                        dir.y = 0.0f;
                        dir.Normalize();
                        transform.rotation = Quaternion.Lerp(transform.rotation,
                                                         Quaternion.LookRotation(dir),
                                                        rotationSpeed * Time.deltaTime);
                        GameObject.FindWithTag("Player").GetComponent<PlayerHp>().DamageByEnemy();
                        stateTime = 0.0f;
                        anim["Attack"].speed = -0.5f;
                        anim["Attack"].time = anim["Attack"].length;
                        anim.Play("Attack");
                    }
                    if (disrance < attackRange)
                    {
                        enemyState = ENEMYSTATE.IDLE   ;
                    }
                }
                break;
            case ENEMYSTATE.DAMAGE:
                {
                    
                    healthPoint -= 1;
                    //imgHp.fillAmount = (float)healthPoint / (float)originHp;

                    //imgHp.color = Color.Lerp(Color.red, Color.green, healthPoint/5.0f);

                    
                    //if (healthPoint <= 1) imgHp.C
                    //else if(healthPoint <= 3) imgHp.color = Color.yellow;
                    AnimationState animState = anim.PlayQueued("Damage", QueueMode.PlayNow);//예약의 개념이므로0

                    animState.speed = 2.0f; // 따로 받아서 변경..

                    float animLength = anim["Damage"].length / animState.speed;
                    CancelInvoke();
                    Invoke("PlayIdleFromDamage", animLength);

                    stateTime = 0.0f;
                    enemyState = ENEMYSTATE.IDLE;

                    if(healthPoint <= 0)
                    {
                        enemyState = ENEMYSTATE.DEAD;
                    }
                }
                break;
            case ENEMYSTATE.DEAD:
                { 
                    StartCoroutine(DeadProcess());
                    enemyState = ENEMYSTATE.NONE;
                }
                break;
        }
    }
    void OnCollisionEnter(Collision coll)
    {
        if (enemyState == ENEMYSTATE.DEAD || enemyState == ENEMYSTATE.NONE)
            {
                return;
            }
    }
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Golem"))
        {
            enemyState = ENEMYSTATE.DAMAGE;
        }
    }

    IEnumerator DeadProcess()
    {
        CancelInvoke();
        characterController.enabled = false;

        anim["Death"].speed = 2.0f;
        anim.Play("Death");

        while (anim.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1.0f);

        //GameObject explosionObj = Instantiate(explosionParticle) as GameObject;
        //Vector3 explosionObjPos = transform.position;
        //explosionObjPos.y = 1.0f;
        //explosionObj.transform.position = explosionObjPos;

        yield return new WaitForSeconds(0.5f);

        //GameObject deadObj = Instantiate(deadObject) as GameObject;
        //Vector3 deadObjPos = transform.position;
        //deadObjPos.y = 2.0f;
        //deadObj.transform.position = deadObjPos;

        //deadObj.transform.rotation = Random.rotation;

        gameObject.SetActive(false);
    }
}
