using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamForward : MonoBehaviour {

	private Transform cam;

	void Start()
	{
		cam = GameObject.FindWithTag ("MainCamera").transform;
	}

	void LateUpdate () {
		transform.LookAt (cam);
	}
}
