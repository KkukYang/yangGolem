using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLine : MonoBehaviour {


	private GameObject preHit = null;

	void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, Mathf.Infinity,1<<LayerMask.NameToLayer("Monster"))) {
			if (hit.collider.gameObject != preHit && preHit != null) {
				preHit.GetComponent<MonsterOutLine> ().outLine.SetActive (false);
			}
			if (hit.collider.CompareTag ("Animal")) {
				hit.collider.GetComponent<MonsterOutLine> ().outLine.SetActive (true);
				preHit = hit.collider.gameObject;
				//Debug.Log (preHit.name);
			}
		}

	}
}
