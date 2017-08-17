using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public EnumHeroState heroState = EnumHeroState.Null;

    public Animator heroAnimator;
    CharacterController _characterController;

    public float gravity = -9.8f;
    public float yVelocity = 0;
    public float moveSpeed = 5;
    public float jumpSpeed = 5;

    Vector3 rot;

    public bool isDie = false;
    public bool isGround = false;

    public Transform weaponeSlotHand;
    public Transform weaponeSlotBack;

    //public float TestVal;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }


    void OnEnable()
    {

        StartCoroutine("InitState");
    }


    IEnumerator InitState()
    {
        heroState = EnumHeroState.Idle;

        yield return null;

        NextState();
    }


    protected void NextState()
    {
        string methodName = heroState.ToString() + "State";

        System.Reflection.MethodInfo info = GetType().GetMethod(methodName
            , System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        StartCoroutine((IEnumerator)info.Invoke(this, null));
    }


    IEnumerator NullState()
    {
        while (this.heroState == EnumHeroState.Null)
        {
            yield return null;
        }

        NextState();
    }

    IEnumerator IdleState()
    {
        moveSpeed = 5.0f;

        while (this.heroState == EnumHeroState.Idle)
        {
            if (!heroAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                heroAnimator.Play("Idle");
            }
            yield return null;
        }

        NextState();
    }

    IEnumerator AttackState()
    {
        heroAnimator.SetBool("bAttack", true);

        heroAnimator.GetComponent<AnimationEvent>().add = new AnimationEvent.Add(EventAttackCombo);
        heroAnimator.GetComponent<AnimationEvent>().end = new AnimationEvent.End(EventAttackEnd);

        moveSpeed *= 0.1f;

        while (this.heroState == EnumHeroState.Attack)
        {

            if (Input.GetMouseButtonDown(0))
            {
                heroAnimator.SetBool("bAttack", true);
            }

            yield return null;
        }

        NextState();
    }

    IEnumerator RunState()
    {
        moveSpeed = 5.0f;

        while (this.heroState == EnumHeroState.Run)
        {
            if (!heroAnimator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            {
                heroAnimator.Play("Run");
            }
            yield return null;
        }

        NextState();
    }

    IEnumerator RollState()
    {
        while (this.heroState == EnumHeroState.Roll)
        {
            yield return null;
        }

        NextState();
    }

    IEnumerator JumpState()
    {
        while (this.heroState == EnumHeroState.Jump)
        {
            yield return null;
        }

        NextState();
    }

    public void EventAttackCombo()
    {
        heroAnimator.SetBool("bAttack", false);
    }

    public void EventAttackEnd(GameObject obj)//obj 안씀.
    {
        if(heroAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack3"))
        {
            heroAnimator.SetBool("bAttack", false);
            heroState = EnumHeroState.Idle;
        }
        else
        {
            if (!heroAnimator.GetBool("bAttack"))
            {
                heroState = EnumHeroState.Idle;
            }
        }
    }

    void Update()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDir;
        isGround = _characterController.isGrounded;
        if (_characterController.isGrounded == true)
        {
            yVelocity = 0.0f;

            if (heroState != EnumHeroState.Attack)
            {
                if ((v > -0.1f && v < 0.1f)
                    && (h < 0.1f && h > -0.1f))
                {
                    heroState = EnumHeroState.Idle;
                }
                else
                {
                    heroState = EnumHeroState.Run;
                }


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //Debug.Log("Jump");
                    yVelocity = jumpSpeed;
                }
            }

            //나중에 게임상태도 신경써야 안겹침. 전투상태 건설상태 등등..
            if (Input.GetMouseButtonDown(0) 
                && heroState != EnumHeroState.Jump
                && (heroState == EnumHeroState.Idle || heroState == EnumHeroState.Run || heroState == EnumHeroState.Attack))
            {
                heroState = EnumHeroState.Attack;
                //heroAnimator.SetBool("bAttack", true);
            }

        }

        moveDir = MoveController(v, h) * moveSpeed;
        moveDir.y = yVelocity;
        yVelocity += gravity * Time.deltaTime;
        _characterController.Move(moveDir * Time.deltaTime);

    }

    #region MoveController(float, float)
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
    #endregion
}

public enum EnumHeroState
{
    Null,
    Idle,
    Attack, //3type
    Run,
    Roll,
    Jump,

}