using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeographyCube : MonoBehaviour
{
    public EnumCubeType cubeType;
    public bool isEnableBuild = false;
    public bool isCurSelectFromTileInfo = false;

    public int positionID;
    public int layerID;

    public GameObject grid;
    public GameObject cube;

    private void OnEnable()
    {
        if(cubeType == EnumCubeType.NULL 
            || cubeType == EnumCubeType.Water
            || cubeType == EnumCubeType.Slope)
        {
            isEnableBuild = false;
        }
        else
        {
            isEnableBuild = true;
        }

        cube = transform.Find("Cube").gameObject;
        grid = transform.Find("Grid").gameObject;

    }

    void Start()
    {

    }

    void Update()
    {
        if (TileInfoManager.instance.curSelectCube != null && isEnableBuild && !isCurSelectFromTileInfo)
        {
            grid.SetActive(true);
        }
        else
        {
            grid.SetActive(false);
        }

        if(isCurSelectFromTileInfo)
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
}
