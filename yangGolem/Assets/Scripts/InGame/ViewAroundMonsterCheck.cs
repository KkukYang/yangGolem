using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ViewAroundMonsterCheck : MonoBehaviour
{


    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            other.GetComponent<NavMeshAgent>().enabled = true;
            other.GetComponent<MonsterBehaviour>().StartNextState();
            other.GetComponent<Rigidbody>().useGravity = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            other.GetComponent<NavMeshAgent>().enabled = false;
            other.GetComponent<MonsterBehaviour>().StartCoroutine(other.GetComponent<MonsterBehaviour>().StopNextState());
            other.GetComponent<Rigidbody>().useGravity = true;

        }

    }
}
