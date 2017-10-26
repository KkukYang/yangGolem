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
    public PLAYERSTATE playerState = PLAYERSTATE.IDLE;
    private PLAYERSTATE prePlayerState = PLAYERSTATE.NONE;

    protected CharacterController characterController;

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
    public float yVelocity = 0;
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



    IEnumerator Start()
    {
        anim = GetComponent<Animator>();
        playerState = PLAYERSTATE.IDLE;
        characterController = GetComponent<CharacterController>();

        hp = MaxHp;
        die = false;
        //hpBar.fillAmount = hpBarFade.fillAmount;

        onAttack = false;
        count = 0;

        while(!TileInfoManager.instance.isDoneLoadingMap)
        {
            yield return null;
        }

        StartCoroutine("TileCheckUnderHero");
        StartCoroutine("PlayerControl");
    }


    IEnumerator TileCheckUnderHero()
    {
        while (true)
        {
            yield return CoroutineManager.instance.GetWaitForSeconds(0.2f);

            RaycastHit hitTileCheck;
            int mask = 1 << LayerMask.NameToLayer("Cube");
            //Debug.DrawRay(this.transform.position,
            //    transform.TransformDirection(-Vector3.up * testRayLenth),
            //    Color.red);

            if (Physics.Raycast(this.transform.position,
                transform.TransformDirection(-Vector3.up),
                out hitTileCheck, testRayLenth, mask))
            {
                //if (hitTileCheck.transform.parent.GetComponent<GeographyCube>() != null)
                //{
                //    curCubeUnderPlayer = hitTileCheck.transform.parent.GetComponent<GeographyCube>();

                //    //Debug.Log (cube.positionID + " " + cube.layerID);
                //    curBottomPositionID = curCubeUnderPlayer.positionID;
                //    curBottomLayerID = curCubeUnderPlayer.layerID;
                //}

                //curCubeUnderPlayer = hitTileCheck.transform.parent.GetComponent<GeographyCube>();
                //curBottomPositionID = curCubeUnderPlayer.positionID;
                //curBottomLayerID = curCubeUnderPlayer.layerID;

                ////_obj.name = String.Format("{0}/{1}/{2}/{3}", _type, _positionID, _layer, _isExistOnCube);
                try
                {
                    curBottomPositionID = int.Parse(hitTileCheck.transform.parent.name.Split('/')[1]);
                    curBottomLayerID = int.Parse(hitTileCheck.transform.parent.name.Split('/')[2]);
                }
                catch
                {
                    Debug.Log("exception : " + hitTileCheck.transform.name);
                }

            }
        }
    }


    IEnumerator PlayerControl()
    {
        while (true)
        {
            yield return null;

            if (!die)
            {
                if (hp <= 0)
                {
                    die = true;
                    playerState = PLAYERSTATE.DEATH;
                }
            }
            else
            {
                anim.Play("Death");
                anim.SetBool("death", true);
                break;
            }

            RaycastHit groundHit;
            //Debug.DrawRay(transform.position, -Vector3.up, Color.blue);
            if (Physics.Raycast(transform.position+new Vector3(0,1,0), -Vector3.up, out groundHit, 1, 1 << LayerMask.NameToLayer("Cube")))
            {
                if (groundHit.collider.transform.parent.CompareTag("Cube"))
                {
                    yVelocity = 0.0f;
                }
            }

            if (Physics.Raycast(transform.position+new Vector3(0,1,0), -Vector3.up, out groundHit, 2, (1 << LayerMask.NameToLayer("Cube"))|(1<<LayerMask.NameToLayer("FieldObject"))))
            {
                if (groundHit.collider.transform.parent.CompareTag("Cube"))
                {
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


            if (Input.GetMouseButtonDown(0) && playerState != PLAYERSTATE.ROLL)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Cube")))
                {
                    AttackDirection(hit.point.z - transform.position.z, hit.point.x - transform.position.x);
                    onAttack = true;
                    if (count == 0)
                    {
                        anim.SetBool("attack", true);
                        playerState = PLAYERSTATE.ATTACK;
                        count = 1;
                    }
                    else if (count == 1)
                    {
                        attack2True = true;
                        count = 2;
                    }
                    else if (count == 2)
                    {
                        attack3True = true;
                        if(attack2True == true)
                        {
                            count = 2;
                        }
                    }
                    

                    continue;

                }
            }

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            moveDir = Vector3.zero;

            if (playerState != PLAYERSTATE.ATTACK
                && playerState != PLAYERSTATE.ROLL)
            {
                moveDir = MoveController(v, h);
            }

            moveDir.y = yVelocity;
            yVelocity += gravity * Time.deltaTime;

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


            characterController.Move(moveDir * moveSpeed * Time.deltaTime);


            if (prePlayerState == playerState)
            {
                //continue;
            }

            switch (playerState)
            {
                case PLAYERSTATE.IDLE:
                    {
                        attackCollider[0].SetActive(false);
                        attackCollider[1].SetActive(false);
                        attackCollider[2].SetActive(false);
                        //if(prePlayerState ==PLAYERSTATE.RUN)
                            anim.CrossFade("Idle", 0.1f);
                        //else
                        //    anim.Play ("Idle");
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
                        attackCollider[0].SetActive(false);
                        attackCollider[1].SetActive(false);
                        attackCollider[2].SetActive(false);
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
                        //anim.SetBool("attack", true);
                        if(attack2True == true && attack3True == true)
                        {
                            if (anim.GetBool("idle") == true)
                            {
                                playerState = PLAYERSTATE.IDLE;
                                anim.CrossFade("Idle", 0.1f);
                                onAttack = false;
                            }
                            else if (anim.GetBool("run") == true)
                            {
                                playerState = PLAYERSTATE.RUN;
                                anim.Play("Run");
                                onAttack = false;
                            }
                        }
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

            prePlayerState = playerState;

        }
    }


    void Update()
    {

        //		hpBar.fillAmount = hp / MaxHp;
        //		if (hpBar.fillAmount < hpBarFade.fillAmount) {
        //			hpBarFade.fillAmount -= Time.deltaTime/10; 
        //		}

    }


    public void OnDamage(int damage)
    {
        hp -= damage;
        anim.SetTrigger("damage");
        playerState = PLAYERSTATE.DAMAGE;

    }


    public void AttackCollier()
    {
        //attackCollider[0].SetActive(true);
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
        else{
            playerState = PLAYERSTATE.IDLE;
        }
    }


    public void OffAttack(int num)
    {
        if (num == 0)
        {
            if (attack2True == true)
            {
                anim.SetBool("attack", true);
                attack2True = false;
            }
            else
            {
                playerState = PLAYERSTATE.IDLE;
                anim.SetBool("attack", false);
                onAttack = false;
                count = 0;
                attack2True = false;
                attack3True = false;
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
                attack2True = false;
                attack3True = false;
            }
            else
            {
                playerState = PLAYERSTATE.IDLE;
                anim.SetBool("attack", false);
                onAttack = false;
                count = 0;
                attack2True = false;
                attack3True = false;
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
            playerState = PLAYERSTATE.IDLE;
            anim.SetBool("attack", false);
            count = 0;
            onAttack = false;
            attack2True = false;
            attack3True = false;
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


    // public void OnAttackMove(int num)
    // {
    //     StartCoroutine(CoAttackMove(num));
    // }


    // IEnumerator CoAttackMove(int num)
    // {
    //     int count = 0;

    //     while (count < num)
    //     {
    //         count++;
    //         characterController.Move(transform.forward * attackMoveSpeed * Time.deltaTime);
    //         yield return new WaitForFixedUpdate();
    //     }
    //     yield return null;
    // }
    // public void RollPlay()
    // {
    //     StopCoroutine("CoRollMove");
    //     StartCoroutine("CoRollMove");
    // }


    // IEnumerator CoRollMove()
    // {
    //     playerState = PLAYERSTATE.ROLL;
    //     int count = 0;
    //     float speed = 1.5f;
    //     while (count < totalCount)
    //     {
    //         count++;
    //         characterController.Move(transform.forward * Time.deltaTime * speed * rollSpeed + transform.up * yVelocity * Time.deltaTime);
    //         if (count > accCount)
    //         {
    //             speed -= 0.1f;
    //             if (speed <= 0)
    //                 speed = 0;
    //         }
            
    //         yield return new WaitForFixedUpdate();
    //     }
    //     yield return null;
    //     OffRoll();
    // }


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
    public CharacterController GetCharacterController()
    {
        return characterController;
    }
}
