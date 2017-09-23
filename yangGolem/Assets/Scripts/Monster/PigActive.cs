using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigActive : MonoBehaviour {

	private Pig pig;
	GameObject gogi = null;
	public bool isGogi = false;
	void Start()
	{
		pig = transform.parent.GetComponent<Pig> ();
	}

	void OnTriggerStay(Collider coll)
	{
		if (isGogi == false) {
			if (coll.CompareTag ("Gogi")) {
				isGogi = true;
				gogi = coll.gameObject;
				pig.gogi = gogi.transform;
			}
		}
	}
}
