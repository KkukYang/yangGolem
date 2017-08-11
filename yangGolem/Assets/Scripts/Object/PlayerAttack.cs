using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public Transform pos;
    public bool onAtk = false;
    public Vector3 enemyPos;
    Collider _collider;
    void Start()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }
	void Update () {
        if (TotalData.onCar == true)
        {
            return;
        }

        Vector3 vec1 = (enemyPos - transform.position);
        Vector3 vec2 = (pos.position - transform.position).normalized;
        if (onAtk == true)
        {
            if (vec1.magnitude > 5f)
            {
                onAtk = false;
                return;
            }
            _collider.enabled = true;
            vec1.Normalize();
            transform.Translate(vec1 * Time.deltaTime * 10);
        }
        else if (onAtk == false)
        {
            if (Mathf.Abs(transform.position.x - pos.position.x) > 0.25f)
            { transform.Translate(vec2 * Time.deltaTime * 10); }
            else
            {
                transform.position = pos.position;
            }
        }

	}
    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.CompareTag("Animal"))
        {
            if (onAtk == true)
            {
                if (coll.GetComponent<HouseCtrl>().heart <= 0)
                {
                    coll.gameObject.SetActive(false);
                }
                _collider.enabled = false;
                onAtk = false;
            }
        }
    }
}
