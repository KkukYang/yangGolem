using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	private Animator anim;
	public int hp = 3;
	private int maxHp;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
		maxHp = hp;
	}
	void Enable()
	{
		anim.SetBool("die",false);
		anim.Play("Idle");
		hp = maxHp;
	}
	public void Damage()
	{
		hp--;
		anim.SetTrigger("damage");
		if(hp<=0)
		{
			Debug.Log("Cut Tree");
			anim.SetBool("die",true);
			//gameObject.transform.parent.gameObject.SetActive(false);
		}
	}
	public void OnDie()
	{
		gameObject.transform.parent.gameObject.SetActive(false);
	}
}
