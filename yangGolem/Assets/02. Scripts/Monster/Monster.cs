using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {

	enum MONSTERANIM
	{   
		NONE = -1,
		IDLE = 0,
		MOVE = 1,
		ATTACK,
		DAMAGE,
		DEATH
	};

	private Animation anim;
	private MONSTERANIM monstarAnim;
	public NavMeshAgent nav;
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
	public int hp = 3;

	bool monTrue;
	int monIndex = 100;

	public GameObject playerCatch;


	public bool die = false;

	void Awake () {
		ch = GetComponent<CharacterController> ();
		target = GameObject.Find("Player").transform;
		anim = GetComponent<Animation>();
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
		monTrue = false;
		playerCatch.SetActive (false);
	}
	void AnimIdle()
	{
		anim["Idle"].speed = 3.0f;
		anim.CrossFade("Idle");
	}
	void AnimMove()
	{
		anim["Move"].speed = 2.0f;
		anim.CrossFade("Move");
	}
	void AnimAttack()
	{
		anim["Attack"].speed = -0.5f;
		anim["Attack"].time = anim["Attack"].length;
		anim.Play("Attack");
	}
	void AnimDamage()
	{
		anim["Damage"].speed = 4.0f;
		//anim["Damage"].time = anim["Damage"].length;
		anim.Play("Damage");
	}
	void Update () {

		Quaternion rot = transform.rotation;
		rot.x = 0;
		rot.z = 0;
		transform.rotation = rot;

		if (ch.isGrounded == true) {
			yVelocity = 0;
		}else{
			yVelocity += (-9.8f * Time.deltaTime);
			ch.Move(new Vector3(0,yVelocity,0));
		}


		switch(monstarAnim)
		{


		case MONSTERANIM.IDLE:
			{

				//monstarAI
				playerCatch.SetActive (false);
				if (monTrue == true) {
					MonsterAI.instance.emenyNum--;
					monIndex = 100;
					monTrue = false;
				}
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
				//monstarAI
				if (monTrue != true) {
					monIndex = MonsterAI.instance.emenyNum;
					MonsterAI.instance.emenyNum++;
					monTrue = true;
					if (monIndex >= MonsterAI.instance.monstarNum) {
						playerCatch.SetActive (false);
						monstarAnim = MONSTERANIM.IDLE;
						break;
					} else {
						playerCatch.SetActive (true);
					}
				}


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
						nav.destination = target.position * moveSpeed;
						monTrue = true;
					}
				}
			}
			break;
		case MONSTERANIM.ATTACK:
			{
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
					AnimAttack ();
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
				if (monTrue == true) {
					MonsterAI.instance.emenyNum--;
					monIndex = 100;
					monTrue = false;
					playerCatch.SetActive (false);
				}
				StartCoroutine(DeadProcess());
				monstarAnim = MONSTERANIM.NONE;
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
				AnimDamage ();

			}
		}
	}

	IEnumerator DeadProcess()
	{
		die = true;
		CancelInvoke();
		nav.enabled = false;
		ch.enabled = false;

		anim["Death"].speed = 2.0f;
		anim.Play("Death");

		while (anim.isPlaying)
		{
			yield return new WaitForEndOfFrame();
		}
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
