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
    public float zoomSnap;

    public bool isInit = false;

	bool shake = false;

    void Start()
    {
        isInit = false;
        thisCamera = GetComponent<Camera>();
        fov = thisCamera.fieldOfView = 50.0f;

		Transform p = this.transform.parent;

//        this.transform.parent = player.transform;
//        this.transform.localPosition = new Vector3(0.0f, 6.0f, -3.5f);
//		this.transform.parent = p;
		p.parent = player.transform;
		p.localPosition = new Vector3(0.0f, 6.0f, -3.5f);
		p.parent = null;
        initCameraPos = this.transform.position;
        initPlayerPos = player.transform.position;

        min *= TileInfoManager.instance.viewAround * 0.1f;
        max *= TileInfoManager.instance.viewAround * 0.1f;
        zoomSnap *= TileInfoManager.instance.viewAround * 0.1f;
        fov *= TileInfoManager.instance.viewAround * 0.1f;
        isInit = true;
    }

    void Update()
    {
        if (!isInit)
        {
            return;
        }

            if (Input.GetKeyDown (KeyCode.P) && shake == false) {
			StartCoroutine (ShakeIt ());
		}

        if (PopUpManager.instance.listPopUp.Find(popup => popup.name == "PopUpTest") == null)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                fov += zoomSnap;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                fov -= zoomSnap;
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
			this.transform.parent.position = new Vector3(initCameraPos.x + increment.x
                , initCameraPos.y + increment.y
                , initCameraPos.z + increment.z);
        }

    }
	IEnumerator ShakeIt()
	{
		float ttime = 0;
		Vector3 pos = transform.localPosition;
		shake = true;
		while (true) {
			ttime += Time.deltaTime;
			if (ttime >= 1) 
			{
				transform.localPosition = pos;
				shake = false;
				yield break;
			}
			transform.localPosition = pos + transform.up * Random.Range (-0.1f, 0.1f) + transform.right * Random.Range (-0.1f, 0.1f);//new Vector3 (Random.Range (-0.1f, 0.1f),0, Random.Range (-0.1f, 0.1f));
			yield return new WaitForEndOfFrame ();
		}
		yield return null;
	}
}
