using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckLocationByClick : MonoBehaviour {

    static private CheckLocationByClick s_instance = null;
    static public CheckLocationByClick instance
    {
        get
        {
            s_instance = FindObjectOfType(typeof(CheckLocationByClick)) as CheckLocationByClick;
            return (s_instance != null) ? s_instance : null;
        }
    }

    //Current Grid State.
    //Grid  : Plane Scale 2 Time.

    //Ray Check Result.
    //Up    : 0.0f,   0.0f,   14.0f
    //Down  : 0.0f,   0.0f,   -14.0f
    //Left  : -14.0f, 0.0f,   0.0f
    //Right : 14.0f,  0.0f,   0.0f


    public int clickLayer = 8;
    public int blockLayer = 9;

    public int col = 20;
    public int row = 20;

    public SelectPoint selectPoint;
    //public SelectPoint preSelectPoint;

    public GameObject grid;
    public GameObject objClickedPositon;
    public GameObject selectTileGroup;
    public GameObject floorLand;

    public Vector3 preMousePos;

    public bool isConstruction = false;
    public bool isCheckTest = false;

    Bounds bounds;
    Bounds boundsISO;
    //public Vector3[] arrayEdgePointVec = new Vector3[4];
    //public Vector3[] arrayEdgePointVecISO = new Vector3[4];

    public Vector3 clickedPosition;
    public Vector3 rotatedPosition = Vector3.zero;

    public List<GameObject> listSelectTile = new List<GameObject>();

    //private enum EdgePointIndex
    //{
    //    Up = 0,
    //    Down,
    //    Left,
    //    Right            
    //}



    void Start ()
    {
        //Get Bounds normal and ISO
        boundsISO = grid.GetComponent<MeshCollider>().bounds;

        Vector3 tempVec = grid.transform.eulerAngles;
        grid.transform.eulerAngles = Vector3.zero;
        bounds = grid.GetComponent<MeshCollider>().bounds;
        grid.transform.eulerAngles = tempVec;
        grid.GetComponent<MeshRenderer>().enabled = false;

    }

    void Update ()
    {
        selectTileGroup.transform.parent.gameObject.SetActive(isConstruction);

        if (isConstruction)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("Grid") | 1 << LayerMask.NameToLayer("Floor");
            //mask = ~mask; //반전일 경우.

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                SetSelectTile(hit.point);
            }

            preMousePos = Input.mousePosition;
            grid.GetComponent<MeshRenderer>().enabled = true;

        }

        //놓을곳 지정.
        if (isConstruction && Input.GetMouseButtonUp(0)
            && PopUpManager.instance.listPopUp.Find(popup => popup.name == "PopUpTest") == null)
        {
            bool result = true;

            foreach(GameObject obj in listSelectTile)
            {
                if(!obj.GetComponent<SelectTile>().isEnable)
                {
                    result = false;
                    break;
                }
            }

            if(result)
            {
                InGameManager.instance.selectedObjectToPlace.transform.parent = floorLand.transform;
                InGameManager.instance.selectedObjectToPlace.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;
                InGameManager.instance.selectedObjectToPlace.layer = LayerMask.NameToLayer("PopUp3D");
                InGameManager.instance.selectedObjectToPlace.transform.SetChildLayer(LayerMask.NameToLayer("PopUp3D"));
                foreach (GameObject obj in listSelectTile)
                {
                    obj.SetActive(false);
                }

                isConstruction = false;
                grid.GetComponent<MeshRenderer>().enabled = false;
            }
        }


        //if(preSelectPoint._x != selectPoint._x || preSelectPoint._y != selectPoint._y)
        //if (((Input.mousePosition != preMousePos)) && /*Input.GetMouseButton(0)) ||*/ Input.GetMouseButtonUp(0))
        //if (isConstruction)
        if(isCheckTest && Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("Grid") | 1 << LayerMask.NameToLayer("Floor");
            //mask = ~mask; //반전일 경우.

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                SetSelectTile(hit.point);
                Debug.Log("hit object : " + hit.collider.name + " " + selectPoint._x + ", " + selectPoint._y + "\nhit point : " + hit.point + "\nlayer : " + hit.transform.gameObject.layer);
            }

            preMousePos = Input.mousePosition;
        }

    }

    void SetSelectTile(Vector3 _vec)
    {
        clickedPosition = _vec;
        objClickedPositon.transform.position = clickedPosition;

        rotatedPosition.x = Mathf.Cos(Mathf.Deg2Rad * 45.0f) * clickedPosition.x + (-1 * Mathf.Sin(Mathf.Deg2Rad * 45.0f) * clickedPosition.z);
        rotatedPosition.z = Mathf.Sin(Mathf.Deg2Rad * 45.0f) * clickedPosition.x + Mathf.Cos(Mathf.Deg2Rad * 45.0f) * clickedPosition.z;

        selectPoint._y = Mathf.CeilToInt(rotatedPosition.x);
        selectPoint._x = Mathf.FloorToInt(rotatedPosition.z);

        Vector3 selectTilePos = new Vector3(
            Mathf.Cos(Mathf.Deg2Rad * -45.0f) * (selectPoint._y - bounds.max.x / row) + (-1 * Mathf.Sin(Mathf.Deg2Rad * -45.0f) * (selectPoint._x + bounds.max.x / row))
            , 0.0f
            , Mathf.Sin(Mathf.Deg2Rad * -45.0f) * (selectPoint._y - bounds.max.z / col) + Mathf.Cos(Mathf.Deg2Rad * -45.0f) * (selectPoint._x + bounds.max.z / col));

        //x는 9를 더하고, y는 10을 더하고.
        selectPoint._y += 9;
        selectPoint._x += 10;

        if(listSelectTile.Count > 0)
        {
            listSelectTile[0].GetComponent<SelectTile>().underPoint = selectPoint;
        }
        else
        {
            Debug.Log("listSelectTile.Count : 0");
        }

        selectTileGroup.transform.parent.position = selectTilePos;
    }


}

[System.Serializable]
public class SelectPoint
{
    public int _x;
    public int _y;

    public SelectPoint()
    {
        _x = 0;
        _y = 0;
    }

    public SelectPoint(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public static SelectPoint operator +(SelectPoint left, SelectPoint right)
    {
        return new SelectPoint(left._x + right._x, left._y + right._y);
    }
}