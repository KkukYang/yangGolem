using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCtrl : MonoBehaviour {

    Animator _animator;
    public GameObject item;
    int heart = 5;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    void Damage()
    {
        _animator.SetTrigger("damage");
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.CompareTag("Sword"))
        {
            
            heart--;
            if (heart <= 0)
            {
                
                StartCoroutine(Die());
            }
            Damage();
        }
    }
    IEnumerator Die()
    {
        _animator.SetBool("die", true);
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(3);
        GameObject _item = Instantiate(item) as GameObject;
        _item.transform.position = transform.position + new Vector3(0, 0.2f, 0);
        gameObject.SetActive(false);
    }
}
