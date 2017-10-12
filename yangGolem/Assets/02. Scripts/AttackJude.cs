using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJude : MonoBehaviour {

	//public GameObject bomb;

	void OnTriggerEnter(Collider coll)
	{
		if (coll.GetComponent<MonsterBehaviour>() != null ) {
            //GameObject clone = Instantiate (bomb) as GameObject;
            GameObject clone = ResourceManager.instance.CreateEffectObj("ExplosionForAir", Vector3.zero, 0.5f);

            clone.transform.position = (coll.transform.position + transform.position) * 0.5f;
            clone.SetActive(true);

            coll.GetComponent<MonsterBehaviour>().hp -= 1.0f;
            coll.GetComponent<MonsterBehaviour>().monsterState = MonsterBehaviour.MONSTERSTATE.Damage;
        }
	}

}
