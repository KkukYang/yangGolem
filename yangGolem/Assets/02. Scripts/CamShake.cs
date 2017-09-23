using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour {

	bool shake = false;
	void Update () {
		if (Input.GetKeyDown (KeyCode.P) && shake == false) {
			StartCoroutine (ShakeIt ());
		}
	}
	IEnumerator ShakeIt()
	{
		float ttime = 0;
		Vector3 pos = transform.position;
		shake = true;
		while (true) {
			ttime += Time.deltaTime;
			if (ttime >= 1) 
			{
				transform.position = pos;
				shake = false;
				yield break;
			}
			transform.position += new Vector3 (Random.Range (-0.5f, 0.5f), Random.Range (-0.5f, 0.5f), 0);
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}
}
