using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHp : MonoBehaviour {

    public int initHp = 5;
    public Image hpBar;
    public Text txtHp;
    int hp;

    void Start()
    {
        hp = initHp;
    }
    void Update()
    {
        txtHp.text = hp.ToString("N0");
        hpBar.fillAmount = (float)hp / (float)initHp;
        if (hpBar.fillAmount >= 0.6f)
        {
            hpBar.color = Color.green;
        }
        else if (hpBar.fillAmount >= 0.2f)
        {
            hpBar.color = Color.yellow;
        }
        else
        {
            hpBar.color = Color.red;
        }
    }
    public void DamageByEnemy()
    {
        hp--;
        if (hp <= 0)
        {

        }
    }
}
