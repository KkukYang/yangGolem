using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public abstract class MonsterBehaviour : MonoBehaviour {

	public enum MONSTERSTATE
	{
		None = -1,
		Idle = 0,
		Move = 1,
		Attack,
		Damage,
		Death,
		Eat
	}



	protected MonoBehaviour _behaviour;	
	public MONSTERSTATE monsterState = new MONSTERSTATE();
	protected Animation anim;
	protected NavMeshAgent nav;
	protected Transform player;
	protected float maxHp;
    public MonsterInfo monsterInfo = new MonsterInfo();
    public bool isDie = false;
    //public int positionID;

    [HideInInspector]
	public string idleName = "Idle";
	[HideInInspector]
	public string moveName = "Move";
	[HideInInspector]
	public string attackName = "Attack";
	[HideInInspector]
	public string damageName = "Damage";
	[HideInInspector]
	public string deathName = "Death";
	[HideInInspector]
	public string eatName = "Eat";

	public MONSTERTYPE monsterType = new MONSTERTYPE();

	public float idleTime = 1;
	[HideInInspector]
	public float moveTime = 0;
	public float attackTime = 1;
	[HideInInspector]
	public float damageTime = 0;
	public float eatTime = 5;

	public float moveDistance = 5;
	public float attackDistance = 2;
	public float eatDistance = 10;

	public float hp = 1;

	[HideInInspector]
	public Transform gogi = null;

    protected void NextState()
	{
		string methodName = monsterState.ToString() + "State";

		MethodInfo info = GetType().GetMethod(methodName
			, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

		StartCoroutine((IEnumerator) info.Invoke(_behaviour, null));
	}
	public IEnumerator IdleState()
	{
		float time = idleTime;
		bool onMove = false;
		Vector3 pos = _behaviour.transform.position;
		nav.destination = pos;
		while (monsterState == MONSTERSTATE.Idle) {

			if (!anim.IsPlaying (idleName) && onMove == false) {
				anim [idleName].speed = 10;
				anim.CrossFade (idleName,0.1f);
			}

			time += Time.deltaTime;
			if (time > idleTime) {
				time = 0;
				float ran = Random.Range (0.0f, 100.0f);
				if (ran > 66.0f) {
					onMove = true;
					anim [moveName].speed = 10;
					anim.CrossFade (moveName, 0.1f);
					if (null != nav) {
						nav.stoppingDistance = 0;
						pos = _behaviour.transform.position + new Vector3 (Random.Range (-3, 3), 0, Random.Range (-3, 3));
						nav.destination = pos;
					}
				} else {
					if (null != nav) {
						nav.destination = _behaviour.transform.position;
					}
				}
			}
			if (null != nav) {
				if (Mathf.Floor((pos-_behaviour.transform.position).magnitude) <= 0.10f) {
					nav.destination = _behaviour.transform.position;
					onMove = false;
				}
			}

			yield return null;
		}
		NextState ();
		yield return null;
	}
	public IEnumerator MoveState()
	{
		float time = moveTime;
		while (monsterState == MONSTERSTATE.Move) {

			if (!anim.IsPlaying (moveName)) {
				anim [moveName].speed = 8;
				anim.Play (moveName);
			}

			time += Time.deltaTime;
			if (time > moveTime) {
				time = 0;
				if (null != player) {
					if (null != nav) {
						if (monsterType == MONSTERTYPE.Pig || monsterType == MONSTERTYPE.Chicken) {
							nav.stoppingDistance = 0;
							nav.destination = _behaviour.transform.position*2-player.position;
						} else {
							nav.stoppingDistance = attackDistance;
							nav.destination = player.position;
							if ((player.position - _behaviour.transform.position).magnitude <= attackDistance) {
								monsterState = MONSTERSTATE.Attack;
							}
						}
					}
				}
			}
			yield return null;
		}
		NextState ();
		yield return null;
	}
	public IEnumerator AttackState()
	{
		float time = attackTime;
		while (monsterState == MONSTERSTATE.Attack) {

			if (anim.IsPlaying (attackName)) {
				Vector3 playerPos = player.position;
				playerPos.y = _behaviour.transform.position.y;
				_behaviour.transform.LookAt (playerPos);
			} else {
				anim [moveName].speed = 8;
				anim.Play (moveName);
				nav.stoppingDistance = attackDistance;
				nav.destination = player.position;
			}

			time += Time.deltaTime;
			if (time > attackTime) {
				if (null != player) {
					float distance = (player.position - _behaviour.transform.position).magnitude;
					if (monsterType == MONSTERTYPE.Pig && monsterType == MONSTERTYPE.Slime) {
						if (distance <= attackDistance) {
							Player.instance.OnDamage(1);
							time = 0;
							if (!anim.IsPlaying (attackName)) {
								anim [attackName].speed = 8;
								anim.Play (attackName);
							}
						}
						else {
							if (!anim.IsPlaying (moveName)) {
								anim [moveName].speed = 8;
								anim.Play (moveName);
								nav.stoppingDistance = attackDistance;
								nav.destination = player.position;
							}
						}
					}
				}
			}
			yield return null;
		}
		NextState ();
		yield return null;
	}
	public IEnumerator DamageState()
	{
		float time = damageTime;
		if (!anim.IsPlaying (damageName)) {
			if (null != player) {
				//hp -= player.instance.AttackDamage;
			}
			anim [damageName].speed = 8;
			anim.Play (damageName);
			if (null != nav) {
				nav.destination = _behaviour.transform.position;
			}
		}
		while (monsterState == MONSTERSTATE.Damage) {
			if (!anim.IsPlaying (damageName)) {
				if (hp <= 0) {
					monsterState = MONSTERSTATE.Death;
				} else {
					monsterState = MONSTERSTATE.Idle;
				}
				break;
			}
			time += Time.deltaTime;
			if (time > damageTime) {

			}
			yield return null;
		}
		NextState ();
		yield return null;
	}

	public IEnumerator DeathState()
	{
		if (!anim.IsPlaying (deathName)) {
			anim [deathName].speed = 8;
			anim.Play (deathName);
			if (null != nav) {
				nav.destination = _behaviour.transform.position;
			}
		}


        yield return CoroutineManager.instance.GetWaitForSeconds(1.5f);
        this.transform.DOScale(0.1f, 1.0f).SetEase(Ease.InCubic);
        yield return CoroutineManager.instance.GetWaitForSeconds(1.0f);
        ResourceManager.instance.CreateEffectObj("Eff_ItemGen", this.transform.position, 1.0f).SetActive(true);
        Die();  //아이템 드랍등.
        yield return CoroutineManager.instance.GetWaitForSeconds(0.5f);
        this.transform.parent = ResourceManager.instance.monsterBox.transform;
        this.gameObject.SetActive(false);

        //while (monsterState == MONSTERSTATE.Death) {

        //	yield return null;
        //}
        //NextState ();
        //yield return null;
    }

    protected virtual void Die()
    {
        Debug.Log("monster behaviour die()");
    }

	public IEnumerator EatState()
	{
		float time = eatTime;
		while (monsterState == MONSTERSTATE.Eat) {
			if (null == gogi) {
				monsterState = MONSTERSTATE.Idle;
				break;
			}
			if (anim.IsPlaying (eatName)) {
				Vector3 gogiPos = gogi.position;
				gogiPos.y = _behaviour.transform.position.y;
				_behaviour.transform.LookAt (gogiPos);
			} else {
				anim [moveName].speed = 8;
				anim.Play (moveName);
				nav.stoppingDistance = eatDistance;
				nav.destination = gogi.position;
			}
				
			if (null != gogi) {
				float distance = (gogi.position - _behaviour.transform.position).magnitude;
				if (monsterType == MONSTERTYPE.Pig) {
					if (distance <= eatDistance) {
						if (!anim.IsPlaying (eatName)) {
							anim [eatName].speed = 8;
							anim.Play (eatName);
							time = 0;
						}

						time += Time.deltaTime;
						if (time > eatTime) {
							gogi.gameObject.SetActive (false);
							gogi = null;
							time = 0;
						}	
					} else {
						if (!anim.IsPlaying (moveName)) {
							anim [moveName].speed = 8;
							anim.Play (moveName);
							nav.stoppingDistance = eatDistance;
							nav.destination = gogi.position;
						}
					}
				}
			}
			
			yield return null;
		}
		NextState ();
		yield return null;
	}
}
