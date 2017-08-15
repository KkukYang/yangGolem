using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlManager : MonoBehaviour
{

    //이동 하고자 하는 경우,  X Z좌표로 움직이면 됨.
    Camera thisCamera = null;
    public GameObject player;
    public Vector3 increment;
    public Vector3 initCameraPos;
    public Vector3 initPlayerPos;

    //[Range(5, 20)]
    public float fov;
    public float min;
    public float max;

    public bool isInit = false;

    void Awake()
    {
        isInit = false;
        thisCamera = GetComponent<Camera>();
        fov = thisCamera.fieldOfView = 50.0f;

        this.transform.parent = player.transform;
        this.transform.localPosition = new Vector3(0.0f, 6.0f, -3.5f);
        this.transform.parent = null;
        initCameraPos = this.transform.position;
        initPlayerPos = player.transform.position;
        isInit = true;
    }

    void Update()
    {

        if (PopUpManager.instance.listPopUp.Find(popup => popup.name == "PopUpTest") == null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                fov += 5.0f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                fov -= 5.0f;
            }

            if (fov < min)
            {
                fov = min;
            }
            else if (fov > max)
            {
                fov = max;
            }

            thisCamera.fieldOfView = Mathf.Clamp(fov, min, max);
        }
    }

    private void LateUpdate()
    {
        if (isInit)
        {
            increment = player.transform.position - initPlayerPos;
            this.transform.position = new Vector3(initCameraPos.x + increment.x
                , initCameraPos.y + increment.y
                , initCameraPos.z + increment.z);
        }

    }
}
