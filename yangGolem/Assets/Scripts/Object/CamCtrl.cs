using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamCtrl : MonoBehaviour {

    float _y, _z;
    public Transform player;
    public PlayerAttack pa;
    public PlayerElement pe;
    float ttime = 0;
    Transform carPos;
    public Camera cam;
    float camSize;
    private GameObject _penalCarMaker;
    private CarMaker _carMaker;
    private CarCtrl _carCtrl;
    private ItemMgr _itemMgr;
    void Start()
    {
        _y = transform.position.y;
        _z = transform.position.z;

        camSize = cam.fieldOfView;
        _penalCarMaker = GameObject.Find("PanelCarMaker");
        _penalCarMaker.SetActive(false);

        _carMaker = GameObject.Find("CarMakerMgr").GetComponent<CarMaker>();

        _itemMgr = GameObject.Find("ItemMgr").GetComponent<ItemMgr>();
    }
    void Update()
    {
        ttime -= Time.deltaTime;
        if (ttime <= 0)
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (TotalData.onCarMaker == false)
            {

                if (Input.GetMouseButton(0))
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag("Item1"))
                        {
                            bool _on = _itemMgr.OnItem("UIItem1");
                            if(_on == true)
                                Destroy(hit.collider.gameObject);
                        }
                        if (hit.collider.CompareTag("Item2"))
                        {
                            bool _on = _itemMgr.OnItem("UIItem2");
                            if (_on == true)
                                Destroy(hit.collider.gameObject);
                        }
                        if (TotalData.weapon == 0)
                        {
                            if (hit.collider.CompareTag("Animal"))
                            {
                                pa.onAtk = true;
                                pa.enemyPos = hit.transform.position;
                                ttime = 1;
                            }
                        }
                        else if (TotalData.weapon == 1)
                        {
                            if (hit.collider.CompareTag("Tree"))
                            {
                                pe.onEle = true;
                                //pe.enemyPos = hit.transform.position;
                                ttime = 1;
                            }

                        }
                    }

                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag("CarBody"))
                        {
                            _carMaker.CarBodyFunc(carPos, hit.collider.gameObject.transform.position,_carCtrl);
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                    {
                        if (hit.collider.CompareTag("CarBody"))
                        {
                            //_carMaker.CarBodyFunc(hit.collider.gameObject.transform, hit.collider.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }
    void LateUpdate()
    {
        if (TotalData.onCarMaker == false)
        {
            transform.position = player.position;

        }//transform.position = player.position + new Vector3(0, 4f,-1.5f);//-3.85f);
        else
        {
            transform.position = carPos.position;
        }
    }
    public void CarMaker(GameObject pos)
    {
        if (TotalData.onCarMaker == false)
        {
            //Time.timeScale = 0;
            TotalData.onCarMaker = true;
            carPos = pos.transform;
            cam.fieldOfView = 10;
            _penalCarMaker.SetActive(true);
            _carCtrl = pos.GetComponent<CarCtrl>();
        }
        else
        {
            //Time.timeScale = 1;
            TotalData.onCarMaker = false;
            cam.fieldOfView = camSize;
            _penalCarMaker.SetActive(false);
            _carCtrl = null;
        }
    }
}
