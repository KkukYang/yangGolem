using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

    public Collider swordCollider;
    enum PLAYERSTATE
    {
        WAIT = 0,
        MOVE,
        ATTACK,
        DAMAGE,
        DEAD,
        BUILD
    }
    PLAYERSTATE playerState = PLAYERSTATE.WAIT;
    private Animation anim;

    public int buildCount = 0;

	void Start () {
        anim = GetComponent<Animation>();
        anim.Play("Wait");
	}

    void Update()
    {
        switch (playerState)
        {
            case PLAYERSTATE.WAIT:
                {
                    PlayWaitAnim();
                }
                break;
            case PLAYERSTATE.MOVE:
                {
                    PlayMoveAnim();
                }
                break;
            case PLAYERSTATE.ATTACK:
                {
                    PlayAttackAnim();
                }
                break;
            case PLAYERSTATE.DAMAGE:
                {
                }
                break;
            case PLAYERSTATE.DEAD:
                {
                }
                break;
            case PLAYERSTATE.BUILD:
                {
                    PlayBuildAnim();
                }
                break;
        }
    }
    public void PlayWaitAnim()
    {
        playerState = PLAYERSTATE.WAIT;
        anim.CrossFade("Wait");
        buildCount = 0;
    }
    public void PlayMoveAnim()
    {
        playerState = PLAYERSTATE.MOVE;
        anim.Play("Walk");
    }
    public void PlayAttackAnim()
    {
        playerState = PLAYERSTATE.ATTACK;
        anim["Build"].speed = 4;
        anim.Play("Attack");
    }
    public void PlayBuildAnim()
    {
        playerState = PLAYERSTATE.BUILD;
        anim["Build"].speed = 1;
        anim.Play("Build");
    }
    public void AttackEnd()
    {
        if (swordCollider.enabled == true)
        {
            swordCollider.enabled = false;
        }
        else
        {
            swordCollider.enabled = true;
        }
    }
    public void BuildEnd()
    {
        buildCount++;
    }
}
