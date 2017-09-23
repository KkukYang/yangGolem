using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour {

	private static PlayerAnim n_instance = null;
	public static PlayerAnim instance 
	{
		get
		{
			if (null == n_instance)
			{
				n_instance = FindObjectOfType(typeof(PlayerAnim)) as PlayerAnim;
				if (null == n_instance)
				{
					
				}
			}
			return n_instance;
		}
	}

	public enum PLAYERSTATE
	{
		None = -1,
		Idle = 0,
		Run = 1,
		Roll,
		Damage,
		Attack,
		Death

	};

	public Animator anim;
	public bool attack2True = false;
	public bool attack3True = false;
	int num = 0;
	public GameObject[] attack = new GameObject[3];
	public PLAYERSTATE playerState = new PLAYERSTATE();


	void Start () {
		playerState = PLAYERSTATE.Idle;
		for (int k = 0; k < attack.Length; k++) {
			attack [k].SetActive (false);
		}
	}
	void Update()
	{
		
		switch (playerState) 
		{
		case PLAYERSTATE.Idle:
			{
				anim.SetBool ("idle", true);
				anim.SetBool ("run", false);
				PlayerAttack.instance.onAttack = false;
				anim.SetBool ("attack", false);
			}
			break;
		case PLAYERSTATE.Run:
			{
				anim.SetBool ("run", true);
				anim.SetBool ("idle", false);
				PlayerAttack.instance.onAttack = false;
				anim.SetBool ("attack", false);
			}
			break;
		case PLAYERSTATE.Roll:
			{
				anim.SetBool ("roll",true);
			}
			break;
		case PLAYERSTATE.Damage:
			{
				//None
			}
			break;
		case PLAYERSTATE.Attack:
			{
				anim.SetBool ("attack", true);
				//anim.Play ("attack");
			}
			break;
		case PLAYERSTATE.Death:
			{
				anim.SetBool("death",true);
				anim.Play ("death");
			}
			break;
		case PLAYERSTATE.None:
			{
				anim.SetBool ("roll",false);
				anim.SetBool ("attack", false);
				attack2True = false;
				attack3True = false;
				PlayerAttack.instance.count = 0;
				PlayerAttack.instance.onAttack = false;
				attack [0].SetActive (false);
			}
			break;
		}
	}
	public void PlayerIdle()
	{
		playerState = PLAYERSTATE.Idle;
	}
	public void PlayerRun()
	{
		playerState = PLAYERSTATE.Run;
	}
	public void OnPlayerAttack1()
	{
		if (playerState != PLAYERSTATE.Roll) {
			playerState = PLAYERSTATE.Attack;
		}
	}
	public void OnPlayerAttack2()
	{
		if (attack2True == true) {
			playerState = PLAYERSTATE.Attack;
		} else {
			playerState = PLAYERSTATE.None;
		}
	}
	public void OnPlayerAttack3()
	{
		if (attack3True == true) {
			playerState = PLAYERSTATE.Attack;
		}else {
			playerState = PLAYERSTATE.None;
		}
	}
	public void OffPlayerAttack()
	{
		playerState = PLAYERSTATE.None;
	}

	public void PlayerRoll()
	{
		playerState = PLAYERSTATE.Roll;
	}

	IEnumerator CoAttack(int num)
	{
		//attack [num].SetActive (true);
		yield return new WaitForFixedUpdate ();
		//attack [num].SetActive (false);
	}
	public void AttackCollier()
	{
		attack [0].SetActive (true);
	}
	public void OnDamage(float damage)
	{
		anim.SetTrigger ("damage");
		playerState = PLAYERSTATE.None;
		//Debug.Log ("Damage : " + damage.ToString ());
	}
	public void OnDeath()
	{
		playerState = PLAYERSTATE.Death;
	}
}
