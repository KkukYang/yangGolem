using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAI : MonoBehaviour {

	private static MonsterAI n_instance = null;
	public static MonsterAI instance
	{
		get{
			if (null == n_instance) {
				n_instance = FindObjectOfType (typeof(MonsterAI)) as MonsterAI;
				if (null == n_instance) {

				}
			}
			return n_instance;
		}
	}

	public int emenyNum = 0;
	public bool possibleAttack = false;
	public int monstarNum = 3;

	void Update()
	{
		if (emenyNum >= monstarNum) {
			possibleAttack = false;
		} else {
			possibleAttack = true;
		}
	}
}
