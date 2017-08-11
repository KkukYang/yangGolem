using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarCtrl : MonoBehaviour {

    public Transform car;
    public float sensitivity = 90;
    public float speed = 1;
    public bool onCar = false;
    public GameObject canvus;
    public Transform golemPos;
    public Transform playerPos = null;
    GameObject golem;
    public Button btnMaker;

    void Start()
    {
        btnMaker.onClick.AddListener(() => GameObject.Find("Cam").GetComponent<CamCtrl>().CarMaker(gameObject));
        golem = GameObject.FindWithTag("Golem");
    }
	void Update () {
        if (onCar != true)
        {
            canvus.SetActive(true);
            return;
        }
        else
        {
            golem.transform.position = golemPos.position;
            canvus.SetActive(false);
        }
        if (TotalData.onCar != true)
        {
            onCar = false;
            return;
        }
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDir;
        moveDir = Vector3.forward * v * speed ;
        if (v == 0) h = 0;
        else if (v > 0)
        {
            car.transform.Rotate((Vector3.up * h) * sensitivity * Time.deltaTime);
        }
        else
        {
            car.transform.Rotate(-(Vector3.up * h) * sensitivity * Time.deltaTime);
        }
        car.transform.Translate(moveDir * Time.deltaTime);
	}
    public Vector3 OnCarFunc()
    {
        onCar = true;
        return playerPos.position;
    }
}
