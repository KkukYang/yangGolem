using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJude : MonoBehaviour {

	//public GameObject bomb;

	void OnTriggerEnter(Collider coll)
	{
		if (!coll.CompareTag("Cube") && !coll.CompareTag("Gogi") ) {
            //GameObject clone = Instantiate (bomb) as GameObject;
            GameObject clone = ResourceManager.instance.CreateEffectObj("ExplosionForAir", Vector3.zero, 0.5f);

            clone.transform.position = (coll.transform.position + transform.position) * 0.5f;
            clone.SetActive(true);
		}
	}
}
