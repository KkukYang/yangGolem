using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TotalDayCtrl : MonoBehaviour {

    public Image imgTime;
    public Text txtTime;
    public GameObject totalLight;
    float ttime;
    public float choiceTime = 30;
    Color thisColor;

    void Start()
    {
        
        thisColor = imgTime.color;
        ttime = choiceTime*2;
        txtTime.text = ttime.ToString("N0") ;
        totalLight.SetActive(true);
    }
    void Update () {
        ttime -= Time.deltaTime;
        txtTime.text = ttime.ToString("N0");
        if (TotalData.onNight == false)
        {
            imgTime.fillAmount = ttime / (choiceTime*2);
            imgTime.color = thisColor;
            if (ttime <= 0)
            {
                TotalData.onNight = true;
                ttime = choiceTime;
                totalLight.SetActive(false);
            }
        }
        else
        {
            imgTime.fillAmount = ttime / choiceTime;
            imgTime.color = Color.yellow;
            if (ttime <= 0)
            {
                TotalData.onNight = false;
                ttime = choiceTime*2;
                totalLight.SetActive(true);
            }
        }
	}
}
