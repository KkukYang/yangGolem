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
    public float orthographicSize;
    public float min;
    public float max;

    public bool isInit = false;

    void Awake()
    {
        isInit = false;
        thisCamera = GetComponent<Camera>();
        orthographicSize = thisCamera.orthographicSize = 5.0f;

        this.transform.parent = player.transform;
        this.transform.localPosition = new Vector3(0.0f, 9.42f, -16.31f);
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
                orthographicSize += 0.5f;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                orthographicSize -= 0.5f;
            }

            if (orthographicSize < min)
            {
                orthographicSize = min;
            }
            else if (orthographicSize > max)
            {
                orthographicSize = max;
            }

            thisCamera.orthographicSize = Mathf.Clamp(orthographicSize, 1, 10);
        }
    }

    private void LateUpdate()
    {
        if (isInit)
        {
            increment = player.transform.position - initPlayerPos;
            this.transform.position = new Vector3(initCameraPos.x + increment.x
                , initCameraPos.y
                , initCameraPos.z + increment.z);
        }

    }
}
