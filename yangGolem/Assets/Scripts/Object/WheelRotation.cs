using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelRotation : MonoBehaviour {


    public float speed = 1;
    float v;
	void Update () {
        if (TotalData.onCar != true)
        {
            return;
        }
        v = Input.GetAxis("Vertical");
        transform.Rotate(0,speed * v * Time.deltaTime * 180, 0);
	}
}
