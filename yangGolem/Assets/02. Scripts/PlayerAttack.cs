using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

	private static PlayerAttack n_instance = null;
	public static PlayerAttack instance {
		get {
			if (null == n_instance) {
				n_instance = FindObjectOfType (typeof(PlayerAttack)) as PlayerAttack;
				if (null == n_instance) {

				}
			}
			return n_instance;
		}
	}

	public bool onAttack = false;
	public int count = 0;

	void Start () {
		onAttack = false;
		count = 0;
	}
	
	void Update () {
		if (PlayerHp.instance.die == true)
			return;
		
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, Mathf.Infinity)) {

				onAttack = true;
				if (count == 0) {
					PlayerAnim.instance.OnPlayerAttack1 ();
					count = 1;
				} else if (count == 1) {
					PlayerAnim.instance.attack2True = true;
					count = 2;
				} else if (count == 2) {
					PlayerAnim.instance.attack3True = true;
				} 

			}
		}

	}
}

