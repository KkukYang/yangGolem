using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    public float sensitivity = 700.0f;

	void Update () {
        float h = Input.GetAxis("Horizontal");

        transform.localEulerAngles += (Vector3.up * h) * sensitivity * Time.deltaTime;
	}
}
