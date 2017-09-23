using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private static Player n_instance = null;
    public static Player instance
    {
        get
        {
            if (null == n_instance)
            {
                n_instance = FindObjectOfType(typeof(Player)) as Player;
                if (null == n_instance)
                {

                }
            }
            return n_instance;
        }
    }
    public enum PLAYERSTATE
    {
        NONE = -1,
        IDLE = 0,
        RUN = 1,
        ATTACK,
        ROLL,
        JUMP,
        DAMAGE,
        DEATH
    }

    private Animator anim;
    public PLAYERSTATE playerState;
    private CharacterController characterController;

    public float MaxHp;
    public float hp;
    public bool die;
    //	public UISprite hpBar;
    //	public UISprite hpBarFade;

    public bool onAttack = false;
    public int count = 0;

    public bool attack2True = false;
    public bool attack3True = false;
    int num = 0;

    public float moveSpeed = 3.5f;
    public float gravity = -9.8f;
    float yVelocity = 0;
    Vector3 moveDir;
    Vector3 rot;

    public float attackMoveSpeed = 1;

    public int totalCount = 20;
    public int accCount = 10;
    public float rollSpeed = 7.5f;
    public float jumpSpeed = 5;

    public GameObject[] attackCollider = new GameObject[3];

    public int curBottomPositionID = 0;    //실시간으로 적용되는 애들.
    public int curBottomLayerID = 0;       //실시간으로 적용되는 애들.

    public float testRayLenth = 1.5f;

    public GeographyCube curCubeUnderPlayer;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerState = PLAYERSTATE.IDLE;
        characterController = GetComponent<CharacterController>();

        hp = MaxHp;
        die = false;
        //hpBar.fillAmount = hpBarFade.fillAmount;

        onAttack = false;
        count = 0;
    }

    void Update()
    {
        //		hpBar.fillAmount = hp / MaxHp;
        //		if (hpBar.fillAmount < hpBarFade.fillAmount) {
        //			hpBarFade.fillAmount -= Time.deltaTime/10; 
        //		}
        if (die == false)
        {

            if (hp <= 0)
            {
                die = true;
                playerState = PLAYERSTATE.DEATH;
            }
        }
        if (die == true)
        {
            anim.Play("Death");
            anim.SetBool("death", true);
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0) && playerState != PLAYERSTATE.ROLL)
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Cube")))
            {

                AttackDirection(hit.point.z - transform.position.z, hit.point.x - transform.position.x);
                onAttack = true;
                if (count == 0)
                {
                    playerState = PLAYERSTATE.ATTACK;
                    count = 1;
                }
                else if (count == 1)
                {
                    playerState = PLAYERSTATE.ATTACK;
                    attack2True = true;
                    count = 2;
                }
                else if (count == 2)
                {
                    playerState = PLAYERSTATE.ATTACK;
                    attack3True = true;
                }
                return;
            }
        }

        RaycastHit groundHit;
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");


        Debug.DrawRay(transform.position, transform.TransformDirection(-transform.up) * 10, Color.green);
        if (Physics.Raycast(transform.position, transform.TransformDirection(-transform.up), out groundHit, 0.2f, 1 << LayerMask.NameToLayer("Cube")))
        {
            if (groundHit.collider.CompareTag("Cube"))
            {
                yVelocity = 0.0f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    yVelocity = jumpSpeed;
                }
                if (Input.GetKeyDown(KeyCode.LeftShift) && playerState != PLAYERSTATE.ROLL)
                {
                    playerState = PLAYERSTATE.ROLL;
                }
            }
        }

        {
            RaycastHit hitTileCheck;
            int mask = 1 << LayerMask.NameToLayer("Cube");
            Debug.DrawRay(this.transform.position,
                transform.TransformDirection(-Vector3.up * testRayLenth),
                Color.red);

            if (Physics.Raycast(this.transform.position,
                transform.TransformDirection(-Vector3.up),
                out hitTileCheck, testRayLenth, mask))
            {
                if (hitTileCheck.transform.parent.GetComponent<GeographyCube>() != null)
                {
                    curCubeUnderPlayer = hitTileCheck.transform.parent.GetComponent<GeographyCube>();

                    //Debug.Log (cube.positionID + " " + cube.layerID);
                    curBottomPositionID = curCubeUnderPlayer.positionID;
                    curBottomLayerID = curCubeUnderPlayer.layerID;
                }
            }
        }

        moveDir = Vector3.zero;


        if (v != 0 || h != 0)
        {
            if (playerState != PLAYERSTATE.ATTACK
                 && playerState != PLAYERSTATE.ROLL)
            {
                playerState = PLAYERSTATE.RUN;
            }
        }
        else
        {
            if (playerState != PLAYERSTATE.ATTACK
                 && playerState != PLAYERSTATE.DAMAGE
                 && playerState != PLAYERSTATE.ROLL)
            {
                playerState = PLAYERSTATE.IDLE;
            }
        }

        if (playerState != PLAYERSTATE.ATTACK
            && playerState != PLAYERSTATE.ROLL)
        {
            moveDir = MoveController(v, h);
        }
        moveDir.y = yVelocity;
        yVelocity += gravity * Time.deltaTime;
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);


        switch (playerState)
        {
            case PLAYERSTATE.IDLE:
                {
                    anim.CrossFade("Idle", 0.1f);
                    //anim.Play ("Idle");
                    anim.SetBool("idle", true);
                    anim.SetBool("run", false);
                    anim.SetBool("attack", false);
                    onAttack = false;
                    count = 0;
                    attack2True = false;
                    attack3True = false;
                }
                break;
            case PLAYERSTATE.RUN:
                {
                    anim.Play("Run");
                    anim.SetBool("run", true);
                    anim.SetBool("idle", false);
                    anim.SetBool("attack", false);
                    onAttack = false;
                    count = 0;
                    attack2True = false;
                    attack3True = false;
                }
                break;
            case PLAYERSTATE.ATTACK:
                {
                    anim.Play("Attack");
                    anim.SetBool("roll", false);
                    anim.SetBool("attack", true);
                }
                break;
            case PLAYERSTATE.ROLL:
                {
                    anim.Play("Roll");
                    anim.SetBool("roll", true);
                    anim.SetBool("attack", false);
                    onAttack = false;
                    count = 0;
                    attack2True = false;
                    attack3True = false;
                }
                break;
            case PLAYERSTATE.JUMP:
                {

                }
                break;
            case PLAYERSTATE.DAMAGE:
                {
                    anim.Play("Damage");
                }
                break;
            case PLAYERSTATE.DEATH:
                {
                    anim.Play("Death");
                    anim.SetBool("death", true);
                }
                break;
        }
    }
    public void OnDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("damage");
        playerState = PLAYERSTATE.DAMAGE;

    }
    public void AttackCollier()
    {
        attackCollider[0].SetActive(true);
    }
    public void OffRoll()
    {
        anim.SetBool("roll", false);
        if (anim.GetBool("idle") == true)
        {
            playerState = PLAYERSTATE.IDLE;
        }
        else if (anim.GetBool("run") == true)
        {
            playerState = PLAYERSTATE.RUN;
        }
    }
    public void OffAttack(int num)
    {
        if (num == 0)
        {
            if (attack2True == true)
            {
                anim.SetBool("attack", true);
                attackCollider[0].SetActive(false);
                attackCollider[1].SetActive(true);
            }
            else
            {
                onAttack = false;
                count = 0;
                attack2True = false;
                attack3True = false;
                attackCollider[0].SetActive(false);
                attackCollider[1].SetActive(false);
                attackCollider[2].SetActive(false);
                if (anim.GetBool("idle") == true)
                {
                    playerState = PLAYERSTATE.IDLE;
                }
                else if (anim.GetBool("run") == true)
                {
                    playerState = PLAYERSTATE.RUN;
                }
            }
        }
        else if (num == 1)
        {
            if (attack3True == true)
            {
                anim.SetBool("attack", true);
                attackCollider[1].SetActive(false);
                attackCollider[2].SetActive(true);
            }
            else
            {
                onAttack = false;
                count = 0;
                attack2True = false;
                attack3True = false;
                attackCollider[0].SetActive(false);
                attackCollider[1].SetActive(false);
                attackCollider[2].SetActive(false);
                if (anim.GetBool("idle") == true)
                {
                    playerState = PLAYERSTATE.IDLE;
                }
                else if (anim.GetBool("run") == true)
                {
                    playerState = PLAYERSTATE.RUN;
                }
            }
        }
        else
        {
            count = 0;
            onAttack = false;
            attack2True = false;
            attack3True = false;
            attackCollider[0].SetActive(false);
            attackCollider[1].SetActive(false);
            attackCollider[2].SetActive(false);
            if (anim.GetBool("idle") == true)
            {
                playerState = PLAYERSTATE.IDLE;
                onAttack = false;
            }
            else if (anim.GetBool("run") == true)
            {
                playerState = PLAYERSTATE.RUN;
                onAttack = false;
            }
        }
    }

    private Vector3 MoveController(float v, float h)
    {
        Vector3 vec = Vector3.zero;

        if (v > 0)
        {
            if (h > 0)
            {
                vec = (Vector3.forward + Vector3.right).normalized;
                rot.y = 45;
            }
            else if (h < 0)
            {
                vec = (Vector3.forward - Vector3.right).normalized;
                rot.y = -45;
            }
            else
            {
                vec = Vector3.forward;
                rot.y = 0;
            }
        }
        else if (v < 0)
        {
            if (h > 0)
            {
                vec = (-Vector3.forward + Vector3.right).normalized;
                rot.y = 135;
            }
            else if (h < 0)
            {
                vec = (-Vector3.forward - Vector3.right).normalized;
                rot.y = -135;
            }
            else
            {
                vec = -Vector3.forward;
                rot.y = 180;
            }
        }
        else
        {
            if (h > 0)
            {
                vec = Vector3.right;
                rot.y = 90;
            }
            else if (h < 0)
            {
                vec = -Vector3.right;
                rot.y = -90;
            }
            else
            {
                //rot.y = 0;
            }
        }
        transform.rotation = Quaternion.Euler(rot);
        return vec;
    }
    public void OnAttackMove(int num)
    {
        StartCoroutine(CoAttackMove(num));
    }
    IEnumerator CoAttackMove(int num)
    {
        int count = 0;

        while (count < num)
        {
            count++;
            characterController.Move(transform.forward * attackMoveSpeed * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }
    public void RollPlay()
    {
        StopCoroutine("CoRollMove");
        StartCoroutine("CoRollMove");
    }

    IEnumerator CoRollMove()
    {
        playerState = PLAYERSTATE.ROLL;
        int count = 0;
        float speed = 1.5f;
        while (count < totalCount)
        {
            count++;
            characterController.Move(transform.forward * Time.deltaTime * speed * rollSpeed + transform.up * yVelocity * Time.deltaTime);
            if (count > accCount)
            {
                speed -= 0.1f;
                if (speed <= 0)
                    speed = 0;
            }
            yield return new WaitForFixedUpdate();
        }
        yield return null;
        OffRoll();
    }

    private void AttackDirection(float v, float h)
    {

        float dis = 0.75f;
        if (v > dis)
        {
            if (h > dis)
            {
                rot.y = 45;
            }
            else if (h < -dis)
            {
                rot.y = -45;
            }
            else
            {
                rot.y = 0;
            }
        }
        else if (v < -dis)
        {
            if (h > dis)
            {
                rot.y = 135;
            }
            else if (h < -dis)
            {
                rot.y = -135;
            }
            else
            {
                rot.y = 180;
            }
        }
        else
        {
            if (h > dis)
            {
                rot.y = 90;
            }
            else if (h < -dis)
            {
                rot.y = -90;
            }
            else
            {
                //rot.y = 0;
            }
        }
        transform.rotation = Quaternion.Euler(rot);

    }
}
