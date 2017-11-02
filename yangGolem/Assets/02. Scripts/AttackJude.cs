using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJude : MonoBehaviour {

	//public GameObject bomb;
    private GameObject cam;
    Vector3 tempPos;
    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera");
        tempPos = cam.transform.localPosition;
    }
	void OnTriggerEnter(Collider coll)
	{
		if (coll.GetComponent<MonsterBehaviour>() != null ) {

            GameObject clone = ResourceManager.instance.CreateEffectObj("ExplosionForAir", Vector3.zero, 0.5f);
            Vector3 pos = coll.transform.position;
            pos.y =0;
            pos.y = transform.position.y;
            clone.transform.position = pos;//(coll.transform.position + transform.position) * 0.5f;
            clone.SetActive(true);

            coll.GetComponent<MonsterBehaviour>().hp -= Player.instance.ap;
            coll.GetComponent<MonsterBehaviour>().monsterState = MonsterBehaviour.MONSTERSTATE.Damage;

            AttackMgr.instance.AttackAction();
        }
        if(coll.CompareTag("Tree"))
        {
            // GameObject clone = ResourceManager.instance.CreateEffectObj("ExplosionForAir", Vector3.zero, 0.5f);
            // Vector3 pos = coll.transform.position;
            // pos.y =0;
            // pos.y = transform.position.y;
            // clone.transform.position = pos;//(coll.transform.position + transform.position) * 0.5f;
            // clone.SetActive(true);
            
            coll.GetComponent<Tree>().Damage();
        }
	}
   
}
