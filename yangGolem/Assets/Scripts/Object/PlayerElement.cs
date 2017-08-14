using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElement : MonoBehaviour {

    public GameObject golem;
    public bool onEle = false;
    bool onCar = false;
    //public Vector3 enemyPos;
    private PlayerAnim pa;
    CharacterController _characterController;
    private Transform _respawn;
    void Start()
    {
        pa = GetComponent<PlayerAnim>();
        _characterController = GetComponent<CharacterController>();
        _respawn = GameObject.FindWithTag("Respawn").transform;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (TotalData.weapon == 0)
            {
                if (onCar == false)
                {
                    foreach (GameObject car in GameObject.FindGameObjectsWithTag("Car"))
                    {
                        if ((transform.position - car.transform.position).magnitude <= 2)
                        {
                            if (car.GetComponent<CarCtrl>().playerPos == null) { break; }
                            transform.SetParent(car.transform);
                            golem.transform.SetParent(car.transform);

                            transform.position = car.GetComponent<CarCtrl>().OnCarFunc();
                            transform.localRotation = Quaternion.Euler(0, 0, 0);
                            
                            onCar = true;
                            TotalData.onCar = true;
                            _characterController.enabled = false;
                            break;
                        }
                    }
                }
                else if (onCar == true)
                {
                    OnCarTrue();
                }
            }
        }
    }
    public void OnCarTrue()
    {
        transform.SetParent(_respawn);
        golem.transform.SetParent(_respawn);
        golem.transform.rotation = Quaternion.Euler(0, 0, 0);
        TotalData.onCar = false;
        _characterController.enabled = true;
        onCar = false;
    }
    void OnTriggerStay(Collider coll)
    {
        if (coll.gameObject.CompareTag("Tree"))
        {
            if (TotalData.weapon == 1 && onEle == true)
            {
                onEle = false;
                pa.PlayAttackAnim();
            }
            else if (TotalData.weapon == 2 && onEle == true)
            {
                onEle = false;
                pa.PlayBuildAnim();
            }
        }
        /*
        if (coll.gameObject.CompareTag("Car"))
        {
                if (onCar == false)
                {
                    onCar = true;
                    transform.SetParent(GameObject.FindWithTag("Car").transform);
                    TotalData.onCar = true;
                }
        }
         * */
    }
}
