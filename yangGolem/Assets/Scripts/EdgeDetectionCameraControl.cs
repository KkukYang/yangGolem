using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetectionCameraControl : MonoBehaviour
{
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = this.transform.parent.GetComponent<Camera>();
        this.GetComponent<Camera>().enabled = false;
    }

    void Start()
    {

    }

    void Update()
    {
        //Ray ray;
        RaycastHit hit;
        int mask = 1 << LayerMask.NameToLayer("Grid") | 1 << LayerMask.NameToLayer("Floor");
        //mask = ~mask; //반전일 경우.

        if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit))
        {
            //Debug.Log(hit.transform.name);
            if(hit.transform.name != "player")
            {
                this.GetComponent<Camera>().enabled = true;
            }
            else
            {
                this.GetComponent<Camera>().enabled = false;
            }
        }


    }

    private void LateUpdate()
    {
        this.GetComponent<Camera>().orthographicSize = mainCamera.orthographicSize;
    }
}
