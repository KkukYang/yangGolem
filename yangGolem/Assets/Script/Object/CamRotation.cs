using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour {

    public float camSizeSpeed = 1;
    public float maxSight = 80;
    public float minSight = 10;
    int rotNum = 0;
    private Camera cam;
    void Start()
    {
        cam = GetComponentInChildren<CamCtrl>().cam;
    }
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            rotNum -= 45;
            transform.localRotation = Quaternion.Euler(0, rotNum, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            rotNum += 45;
            transform.localRotation = Quaternion.Euler(0, rotNum, 0);
        }
        cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * camSizeSpeed * Time.deltaTime;
        if (cam.fieldOfView >= maxSight)
        {
            cam.fieldOfView = maxSight;
        }
        else if (cam.fieldOfView <= minSight)
        {
            cam.fieldOfView = minSight;
        }
	}
}
