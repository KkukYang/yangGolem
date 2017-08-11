using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColliderCtrl : MonoBehaviour {

    public GameObject chair;
	
	void Update () {
        if (TotalData.onCarMaker == false)
        {
            chair.SetActive(false);
        }
        else
        {
            chair.SetActive(true);
        }
	}
}
