using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpTest : MonoBehaviour
{
    public GameObject selectObject = null;
    public GameObject thisCamera;
    public UIButton exitButton;

    public Vector3 curTouchPos;
    public Vector3 preTouchPos;
    public Vector3 rotVector;

    public float orthographicSize;
    public float min;
    public float max;

    private void Awake()
    {
        EventDelegate eventButton = new EventDelegate(this, "ExitButtonEvent");
        EventDelegate.Add(exitButton.onClick, eventButton);
    }

    private void OnEnable()
    {
        PopUpManager.instance.StartPopUp(this.gameObject);

        selectObject.transform.parent = thisCamera.transform.Find("Object");
        selectObject.transform.localScale = Vector3.one;
        selectObject.transform.localPosition = Vector3.zero;
        selectObject.transform.eulerAngles = Vector3.zero;

        orthographicSize = thisCamera.GetComponent<Camera>().orthographicSize = 0.005f;

        Invoke("SetTurnOnCamera", 0.5f);
    }

    private void OnDisable()
    {
        thisCamera.SetActive(false);
    }

    void SetTurnOnCamera()
    {
        thisCamera.SetActive(true);
    }

    void ExitButtonEvent()
    {
        ////selectObject 이름안에 정보가 들어 있다. 첫번째x크기, 두번째y크기
        //string[] arrStr = selectObject.name.Split('_');

        //{   //나중에 오브젝트 풀링 쓰자 ResourceManager에서 관리 할 것.
        //    foreach (GameObject obj in CheckLocationByClick.instance.listSelectTile)
        //    {
        //        Destroy(obj);
        //    }

        //    CheckLocationByClick.instance.listSelectTile.Clear();
        //}

        //for (int i = 0; i < int.Parse(arrStr[1]) * int.Parse(arrStr[2]); i++)
        //{
        //    GameObject obj = Instantiate(Resources.Load("Prefabs/SelectTile")) as GameObject;
        //    obj.name = "selectTile";
        //    obj.transform.parent = CheckLocationByClick.instance.selectTileGroup.transform;
        //    CheckLocationByClick.instance.listSelectTile.Add(obj);
        //}

        ////SelectTile들 좌표 설정. 맨 첫번째를 기준으로.
        //for (int idx = 0, i = 0; i < int.Parse(arrStr[1]); i++)
        //{
        //    for (int j = 0; j < int.Parse(arrStr[2]); j++)
        //    {
        //        CheckLocationByClick.instance.listSelectTile[idx].GetComponent<SelectTile>().localPoint = new SelectPoint(j, i);

        //        CheckLocationByClick.instance.listSelectTile[idx].transform.eulerAngles = new Vector3(90.0f, 45.0f, 0.0f);
        //        CheckLocationByClick.instance.listSelectTile[idx].transform.localPosition = new Vector3(j * 0.5f, 0.0f, i * -0.5f);
        //        CheckLocationByClick.instance.listSelectTile[idx].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        //        CheckLocationByClick.instance.listSelectTile[idx].GetComponent<SelectTile>().rootSelectTile = CheckLocationByClick.instance.listSelectTile[0];

        //        //프로젝트뷰에 있을때 비활성니까.
        //        CheckLocationByClick.instance.listSelectTile[idx].SetActive(true);
        //        idx++;
        //    }
        //}


        //InGameManager.instance.selectedObjectToPlace = Instantiate(selectObject);
        //InGameManager.instance.selectedObjectToPlace.transform.parent = CheckLocationByClick.instance.selectTileGroup.transform.parent;
        ////센터에 맞춰야함.
        //InGameManager.instance.selectedObjectToPlace.transform.localPosition = new Vector3((float.Parse(arrStr[1]) - 1.0f) * 0.25f
        //    , 0.0f
        //    , (float.Parse(arrStr[2]) - 1.0f) * -0.25f);

        ////To test.
        ////InGameManager.instance.selectedObjectToPlace.transform.localPosition = new Vector3((1.0f - 1.0f) * 0.25f
        ////    , 0.0f
        ////    , (2.0f - 1.0f) * -0.25f);

        //InGameManager.instance.selectedObjectToPlace.transform.eulerAngles = Vector3.zero;
        //InGameManager.instance.selectedObjectToPlace.name = selectObject.name;
        //CheckLocationByClick.instance.isConstruction = true;

        //Destroy(selectObject);

        thisCamera.SetActive(false);
        PopUpManager.instance.EndPopUp(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            preTouchPos = curTouchPos = thisCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(1))
        {
            curTouchPos = thisCamera.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);

            rotVector = new Vector3(0.0f, preTouchPos.x - curTouchPos.x, 0.0f);
            selectObject.transform.Rotate(rotVector * 10000.0f);


            preTouchPos = curTouchPos;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            orthographicSize += 0.001f;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            orthographicSize -= 0.001f;
        }

        if (orthographicSize < min)
        {
            orthographicSize = min;
        }
        else if (orthographicSize > max)
        {
            orthographicSize = max;
        }

        thisCamera.GetComponent<Camera>().orthographicSize = Mathf.Clamp(orthographicSize, 0.001f, 0.01f);


        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = thisCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("PopUp3D");
            //mask = ~mask; //반전일 경우.

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Debug.Log(hit.transform.name);
                //selectObject 이름안에 정보가 들어 있다. 첫번째x크기, 두번째y크기
                string[] arrStr = selectObject.name.Split('_');

                {   //나중에 오브젝트 풀링 쓰자 ResourceManager에서 관리 할 것.
                    foreach (GameObject obj in CheckLocationByClick.instance.listSelectTile)
                    {
                        Destroy(obj);
                    }

                    CheckLocationByClick.instance.listSelectTile.Clear();
                }

                for (int i = 0; i < int.Parse(arrStr[1]) * int.Parse(arrStr[2]); i++)
                {
                    GameObject obj = Instantiate(Resources.Load("Prefabs/SelectTile")) as GameObject;
                    obj.name = "selectTile";
                    obj.transform.parent = CheckLocationByClick.instance.selectTileGroup.transform;
                    CheckLocationByClick.instance.listSelectTile.Add(obj);
                }

                //SelectTile들 좌표 설정. 맨 첫번째를 기준으로.
                for (int idx = 0, i = 0; i < int.Parse(arrStr[1]); i++)
                {
                    for (int j = 0; j < int.Parse(arrStr[2]); j++)
                    {
                        CheckLocationByClick.instance.listSelectTile[idx].GetComponent<SelectTile>().localPoint = new SelectPoint(j, i);

                        CheckLocationByClick.instance.listSelectTile[idx].transform.eulerAngles = new Vector3(90.0f, 45.0f, 0.0f);
                        CheckLocationByClick.instance.listSelectTile[idx].transform.localPosition = new Vector3(j * 0.5f, 0.0f, i * -0.5f);
                        CheckLocationByClick.instance.listSelectTile[idx].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        CheckLocationByClick.instance.listSelectTile[idx].GetComponent<SelectTile>().rootSelectTile = CheckLocationByClick.instance.listSelectTile[0];

                        //프로젝트뷰에 있을때 비활성니까.
                        CheckLocationByClick.instance.listSelectTile[idx].SetActive(true);
                        idx++;
                    }
                }


                InGameManager.instance.selectedObjectToPlace = Instantiate(selectObject);
                InGameManager.instance.selectedObjectToPlace.transform.parent = CheckLocationByClick.instance.selectTileGroup.transform.parent;
                //센터에 맞춰야함.
                InGameManager.instance.selectedObjectToPlace.transform.localPosition = new Vector3((float.Parse(arrStr[1]) - 1.0f) * 0.25f
                    , 0.0f
                    , (float.Parse(arrStr[2]) - 1.0f) * -0.25f);

                //To test.
                //InGameManager.instance.selectedObjectToPlace.transform.localPosition = new Vector3((1.0f - 1.0f) * 0.25f
                //    , 0.0f
                //    , (2.0f - 1.0f) * -0.25f);

                InGameManager.instance.selectedObjectToPlace.transform.eulerAngles = Vector3.zero;
                InGameManager.instance.selectedObjectToPlace.name = selectObject.name;
                CheckLocationByClick.instance.isConstruction = true;

                Destroy(selectObject);

                thisCamera.SetActive(false);
                PopUpManager.instance.EndPopUp(this.gameObject);

            }

        }
    }
}
