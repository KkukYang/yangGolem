using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeDetectionCameraControl : MonoBehaviour
{
    Camera mainCamera;
    public GameObject player;
    public bool bPlayerTransparent;
    public Material[] arrPlayerMaterial;

    private void Awake()
    {
        mainCamera = this.transform.parent.GetComponent<Camera>();
        this.GetComponent<Camera>().enabled = false;
    }

    void Start()
    {
        player = this.transform.parent.GetComponent<CameraControlManager>().player;
        StartCoroutine(PlayerTransparent());

    }


    IEnumerator PlayerTransparent()
    {
        while(true)
        {
            RaycastHit hit;

            if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit))
            {
                //Debug.Log(hit.transform.name);
                if (hit.transform.name != "player")// || hit.transform.name != "golem")
                {
                    this.GetComponent<Camera>().enabled = true;
                    if(!bPlayerTransparent)
                    {
                        foreach(Material mat in arrPlayerMaterial)
                        {
                            mat.color = Color.gray;
                        }
                        bPlayerTransparent = true;
                    }
                }
                else
                {
                    this.GetComponent<Camera>().enabled = false;
                    if (bPlayerTransparent)
                    {
                        foreach (Material mat in arrPlayerMaterial)
                        {
                            mat.color = Color.white;
                        }
                        bPlayerTransparent = false;
                    }
                }
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    void Update()
    {


    }

    private void LateUpdate()
    {
        this.GetComponent<Camera>().fieldOfView = mainCamera.fieldOfView;
    }
}
