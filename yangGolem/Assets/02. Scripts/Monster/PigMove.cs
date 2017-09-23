using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PigMove : MonoBehaviour {

	public enum MONSTERANIM
	{   
		NONE = -1,
		IDLE = 0,
		MOVE = 1,
		ATTACK,
		DAMAGE,
		DEATH,
		GOGI
	};

	private Animator anim;
	public MONSTERANIM monstarAnim;
	private NavMeshAgent nav;
	private Transform target; 
	private CharacterController ch;
	float yVelocity;

	private int ran =0;
	private Vector3 idlePos;
	private float stateTime;
	public float idleTime = 3;
	public float attackTime = 3;
	public float damageTime = 1;
	public float moveSpeed = 1;
	public float attackDistance = 0.75f;
	public float moveDistance = 7;
	public float gogiDistance = 1.25f;
	public int hp = 3;

	public GameObject playerCatch;

	public bool die = false;

	public Transform gogi = null;

	void Awake () {
		ch = GetComponent<CharacterController> ();
		target = PlayerAnim.instance.transform;
		anim = GetComponent<Animator>();
		nav = GetComponent<NavMeshAgent>();

	}
	void OnEnable()
	{
		nav.enabled = true;
		ch.enabled = true;
		die = false;
		stateTime = 0;
		monstarAnim = MONSTERANIM.IDLE;
		AnimIdle();

		playerCatch.SetActive (false);
	}
	void AnimIdle()
	{
		anim.SetBool ("run",false);
	}
	void AnimMove()
	{
		anim.SetBool ("run",true);
	}
	void Update () {
		float gogiDis = 100;
		if (gogi != null) {
			gogiDis = (gogi.position - transform.position).magnitude;

		}
		switch(monstarAnim)
		{
		case MONSTERANIM.IDLE:
			{
				if (gogi != null) {
					if (gogiDis <= 7) {
						monstarAnim = MONSTERANIM.GOGI;
						break;
					}
				}

				anim.speed = 1;
				playerCatch.SetActive (false);
				//anim.Play ("Idle");
				float dis = (target.position - transform.position).magnitude;
				stateTime += Time.deltaTime;
				if(stateTime > idleTime)
				{
					ran = Random.Range(0,3);
					if(ran != 0)
					{
						AnimIdle();
						nav.destination = transform.position;
					}
					else
					{
						AnimMove();
						idlePos = transform.position + new Vector3(Random.Range(-2.0f,2.0f),0,Random.Range(-2.0f,2.0f));
						nav.destination = idlePos;
					}
					stateTime = 0;
				}
				if(idlePos == transform.position)
				{
					AnimIdle();
				}
				if(dis <= moveDistance)
				{
					if (MonsterAI.instance.possibleAttack == true) {
						monstarAnim = MONSTERANIM.MOVE;
					}
				}
			}	
			break;
		case MONSTERANIM.MOVE:
			{
				playerCatch.SetActive (true);
				//anim.Play ("Run");
				float dis = (target.position - transform.position).magnitude;

				if (dis > moveDistance) {
					monstarAnim = MONSTERANIM.IDLE;
					stateTime = idleTime;
					AnimIdle ();
					break;
				} else {
					if (dis <= attackDistance) {
						monstarAnim = MONSTERANIM.ATTACK;
						stateTime = attackTime;

					} else {
						AnimMove ();
						Vector3 escapeTarget = new Vector3 (transform.position.x * 2 - target.position.x, 0, transform.position.z * 2 - target.position.z);
						nav.destination = escapeTarget * moveSpeed;
					}
				}
			}
			break;
		case MONSTERANIM.ATTACK:
			{
				playerCatch.SetActive (true);
				float dis = (target.position - transform.position).magnitude;

				if(dis > attackDistance)
				{
					monstarAnim = MONSTERANIM.MOVE;

					nav.isStopped = false;
					break;
				}
				nav.destination = transform.position;

				stateTime += Time.deltaTime;
				if (stateTime > attackTime) {
					PlayerAnim.instance.OnDamage (1);
					PlayerHp.instance.hp--;
					transform.rotation = Quaternion.LookRotation (target.position - transform.position);		
					nav.isStopped = true;
					//AnimAttack ();
					stateTime = 0;
				}

			}	
			break;
		case MONSTERANIM.DAMAGE:
			{
				nav.isStopped = false;

				float dis = (target.position - transform.position).magnitude;

				stateTime += Time.deltaTime;
				if(stateTime > damageTime)
				{
					if (dis <= moveDistance) {
						monstarAnim = MONSTERANIM.MOVE;
					} else {
						monstarAnim = MONSTERANIM.IDLE;
						AnimIdle ();
					}
				}
				if (hp <= 0) {
					monstarAnim = MONSTERANIM.DEATH;
				}
			}	
			break;
		case MONSTERANIM.DEATH:
			{
				playerCatch.SetActive (false);

				StartCoroutine (DeadProcess ());
				monstarAnim = MONSTERANIM.NONE;
			}
			break;
		case MONSTERANIM.GOGI:
			{
				playerCatch.SetActive (false);
				anim.Play ("Run");

				if (gogiDis > 7) {
					monstarAnim = MONSTERANIM.IDLE;
				}
				else if (gogiDis > gogiDistance) {
					anim.speed = 1;
					AnimMove ();
					nav.destination = gogi.position * moveSpeed;
				} else {
					//AnimIdle ();
					anim.speed = 3;
					nav.destination = transform.position;
					transform.LookAt (gogi.position);
				}

			}
			break;
		}

	}
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.CompareTag ("Sword")) {
			if (hp > 0) {
				hp--;
				monstarAnim = MONSTERANIM.DAMAGE;
				stateTime = 0;
				//AnimDamage ();

			}
		}
	}

	IEnumerator DeadProcess()
	{
		die = true;
		CancelInvoke();
		nav.enabled = false;
		ch.enabled = false;

		yield return new WaitForSeconds(1.5f);
		GameObject item;
		if (Random.Range (0, 2) == 0) {
			item = Instantiate (Resources.Load ("Items/gogi")) as GameObject;
			item.name = "gogi";
		} else {
			item = Instantiate (Resources.Load ("Items/carrot")) as GameObject;
			item.name = "carrot";
		}
		item.transform.position = transform.position+new Vector3(0,1,0);
		item.transform.rotation = Quaternion.Euler (Random.Range (-90.0f, 90.0f), Random.Range (-90.0f, 90.0f), Random.Range (-90.0f, 90.0f));
		gameObject.SetActive(false);
	}

}
