using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GeographyCube : MonoBehaviour
{
    public EnumCubeType cubeType;
    public bool isEnableBuild = false;  //위에 지형을 놓을때.
    public bool isSuitable = false; //나무 등등 놓을때.
    public bool isCurSelectFromTileInfo = false; //TileInfoManager 에서 현재 선택한 큐브.
    public bool isExistOnCube = false;
    //public bool isPicked = false; //

    //public float pickedTimer = 0.0f;

    public int positionID;
    public int layerID;

    public GameObject grid;
    public GameObject cube;
    public GameObject upper;
	public GameObject invisibleCube;

    private void OnEnable()
    {
        if(cubeType == EnumCubeType.NULL 
            //|| cubeType == EnumCubeType.Water
            || cubeType == EnumCubeType.SlopeUp
            || cubeType == EnumCubeType.SlopeDown
            || cubeType == EnumCubeType.SlopeLeft
            || cubeType == EnumCubeType.SlopeRight)
        {
            isEnableBuild = false;
        }
        else
        {
            isEnableBuild = true;
        }

        invisibleCube = this.transform.Find("InvisibleCube").gameObject;
        grid = this.transform.Find("Grid").gameObject;
        upper = invisibleCube.transform.Find("Upper").gameObject;
        cube = invisibleCube.transform.Find("Cube").gameObject;
    }

    void Start()
    {

    }

    void Update()
    {
        if ((TileInfoManager.instance.curSelectCube != null && isEnableBuild && !isCurSelectFromTileInfo)
            ||(TileInfoManager.instance.curSelectObject != null && isCurSelectFromTileInfo))
        {
            grid.SetActive(true);

            if(cubeType == EnumCubeType.Water)
            {
                if (cube.GetComponent<BoxCollider>() != null)
                {
                    cube.GetComponent<BoxCollider>().enabled = true;
                }
            }

            if (isCurSelectFromTileInfo 
                && layerID != TileInfoManager.instance.layerIDMin )
                //&& isExistOnCube)
            {
                grid.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.7f);   //빨간색.
                isSuitable = false;
            }
            else
            {
                if (isExistOnCube)
                {
                    grid.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.7f);   //빨간색.
                    isSuitable = false;
                }
                else
                {
                    grid.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 0.8f, 0.4f);
                    isSuitable = true;
                }
            }
        }
        else
        {
            grid.SetActive(false);

            if (cubeType == EnumCubeType.Water)
            {
                if (cube.GetComponent<BoxCollider>() != null)
                {
                    cube.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }


        if (isCurSelectFromTileInfo && TileInfoManager.instance.curSelectCube != null)
        {
            if(cube.GetComponent<BoxCollider>() != null)
            {
                cube.GetComponent<BoxCollider>().enabled = false;
            }
            else if(cube.GetComponent<MeshCollider>() != null)
            {
                cube.GetComponent<MeshCollider>().enabled = false;
            }
        }
        else
        {
            if(cubeType != EnumCubeType.Water)
            {
                if (cube.GetComponent<BoxCollider>() != null)
                {
                    cube.GetComponent<BoxCollider>().enabled = true;
                }
                else if (cube.GetComponent<MeshCollider>() != null)
                {
                    cube.GetComponent<MeshCollider>().enabled = true;
                }
            }
        }

//        //Select Located Object.
//        if (Input.GetMouseButtonDown(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;
//            int mask = 1 << LayerMask.NameToLayer("Cube");

//            if (Physics.Raycast(ray, out hit, 100.0f, mask))
//            {
//                if (hit.transform.parent == this.transform)
//                {
//                    //Debug.Log ("Picked");
//                    isPicked = true;
//                    pickedTimer = 0.0f;
//                }
//            }
//        }
//        else if (Input.GetMouseButton(0))
//        {
//            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//            RaycastHit hit;
//            int mask = 1 << LayerMask.NameToLayer("Cube");

//            if (Physics.Raycast(ray, out hit, 100.0f, mask))
//            {
//                if (hit.transform.parent == this.transform)
//                {
//                    if (isPicked)
//                    {
//                        pickedTimer += Time.deltaTime;
//                    }

//                    if (pickedTimer >= 1.0f)
//                    {

//                        TileInfoManager.instance.curSelectCube = this.gameObject;
//                        isCurSelectFromTileInfo = true;
//                        transform.SetChildLayer(LayerMask.NameToLayer("Default"));

//                        TileInfoManager.instance.stageInfo.arrListCubeInStage[positionID].Remove((int)cubeType);
//                        TileInfoManager.instance.listGeoCube.Remove(this);

//                        {
//                            int layerMax = 0;
//                            List<GeographyCube> _listCube = TileInfoManager.instance.listGeoCube.FindAll(_cube => _cube.positionID == positionID);

//                            foreach (var _cube in _listCube)
//                            {
//                                if (_cube.layerID > layerMax)
//                                {
//                                    layerMax = _cube.layerID;
//                                }
//                            }

//                            if (_listCube.Count > 0)
//                            {
//                                GeographyCube cube = _listCube.Find(_cube => _cube.layerID == layerMax);
//                                cube.isExistOnCube = false;
//                            }
//                        }

////                        {
////                            int layerMax = (from listA in TileInfoManager.instance.listGeoCube.FindAll(_cube => _cube.positionID == positionID)
////                                            group listA by listA.layerID into g
////                                            select g.Max(m => m.layerID)).First();
////
////                            Debug.Log(layerMax);
////
////                            GeographyCube cube = TileInfoManager.instance.listGeoCube.FindAll(_cube => _cube.positionID == positionID).Find(_cube => _cube.layerID == layerMax);
////                            cube.isExistOnCube = false;
////                        }

//                    }
//                }
//            }
//        }
//        else if (Input.GetMouseButtonUp(0))
//        {
//            isPicked = false;
//            pickedTimer = 0.0f;
//        }

    }

}
