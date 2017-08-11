using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMgr : MonoBehaviour {

    public Image imgCarHp;
    public Text txtCarHp;
    public float initHp=1;
    float hp;
    private PlayerElement pe;
    bool carGo = true;
    void Start()
    {
        hp = initHp;
        pe = GameObject.FindWithTag("Player").GetComponent<PlayerElement>();
    }
	void Update () {
        if (TotalData.onCar != true)
        {
            hp += Time.deltaTime;
            if (hp >= initHp)
            {
                carGo = true;
                hp = initHp;
            }
        }
        else
        {
            if (carGo == false)
            {
                pe.OnCarTrue();
                return;
            }
            if (hp <= 0)
            {
                pe.OnCarTrue();
                carGo = false;
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                hp -= Time.deltaTime;
            }
        }
        txtCarHp.text = hp.ToString("N1");
        imgCarHp.fillAmount = hp / initHp;
	}
}
