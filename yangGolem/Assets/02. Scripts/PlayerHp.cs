using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHp : MonoBehaviour {

	private static PlayerHp n_instance = null;
	public static PlayerHp instance 
	{
		get
		{
			if (null == n_instance)
			{
				n_instance = FindObjectOfType(typeof(PlayerHp)) as PlayerHp;
				if (null == n_instance)
				{

				}
			}
			return n_instance;
		}
	}

	public float MaxHp;
	public float hp;
	public bool die;
	public UISprite hpBar;
	public UISprite hpBarFade;

	void Start()
	{
		hp = MaxHp;
		die = false;
		hpBar.fillAmount = hpBarFade.fillAmount;
	}

	void Update()
	{
		hpBar.fillAmount = hp / MaxHp;
		if (hpBar.fillAmount < hpBarFade.fillAmount) {
			hpBarFade.fillAmount -= Time.deltaTime/10; 
		}
		if (die == false) {
			
			/*
			if (hpBar.fillAmount >= 0.5f) {
				hpBar.color = Color.green;
			} else if (hpBar.fillAmount >= 0.2f) {
				hpBar.color = Color.yellow;
			} else {
				hpBar.color = Color.red;
			}*/
			if (hp <= 0) {
				die = true;
				GetComponent<PlayerAnim> ().OnDeath ();
			}
		}

	}
}
