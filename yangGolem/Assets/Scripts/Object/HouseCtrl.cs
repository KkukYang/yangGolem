using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCtrl : MonoBehaviour {

    public GameObject item;
    public int heart = 5;
    int tempHeart;
    void Awake()
    {
        tempHeart = heart;
    }
    void OnEnable()
    {
        heart = tempHeart;
    }
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("Golem"))
        {
            heart--;
            if(heart <=0)
            {
                GameObject _item = Instantiate(item) as GameObject;
                _item.transform.position = transform.position + new Vector3(0, 0.2f, 0);
                //gameObject.SetActive(false);
            }
        }
    }
}
