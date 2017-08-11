using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCtrl : MonoBehaviour {

    public bool frontWheel = false;
    public float motorAngle = 10;
    float h;

	void Update () {
        if (TotalData.onCar != true)
        {
            return;
        }
        h = Input.GetAxis("Horizontal");
        if (frontWheel == true)
        {
            transform.localRotation = Quaternion.Euler(0, motorAngle * h, 0); ;
        }
        else
        {
            transform.localRotation = Quaternion.Euler(0, -motorAngle * h, 0); ;
        }

	}
}
