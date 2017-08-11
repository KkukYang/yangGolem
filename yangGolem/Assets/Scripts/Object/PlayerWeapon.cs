using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

    public GameObject[] weapon;
	void Update () {
        if (TotalData.onCar == true) return;
        for (int i = 0; i < weapon.Length; i++)
        {
            if (i == TotalData.weapon)
            {
                weapon[i].SetActive(true);
            }
            else
            {
                weapon[i].SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon[0].transform.position = weapon[TotalData.weapon].transform.position;
            TotalData.weapon = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TotalData.weapon = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TotalData.weapon = 2;
        }
	}
}
