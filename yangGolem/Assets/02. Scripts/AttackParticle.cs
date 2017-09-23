using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackParticle : MonoBehaviour {

	public GameObject particleBoom;
	public Transform pos;

	void OnTriggerEnter(Collider coll)
	{
		GameObject boom = Instantiate (particleBoom) as GameObject;
		boom.transform.position = pos.position;
	}
}
