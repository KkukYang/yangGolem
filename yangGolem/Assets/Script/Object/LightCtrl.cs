using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCtrl : MonoBehaviour {

    public GameObject light;

    void Update()
    {
        if (TotalData.onNight == true)
        {
            light.SetActive(true);
        }
        else
        {
            light.SetActive(false);
        }
    }
	
}
