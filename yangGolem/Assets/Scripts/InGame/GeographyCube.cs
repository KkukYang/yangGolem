using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeographyCube : MonoBehaviour
{
    public EnumCubeType cubeType;
    public bool isEnableBuild = false;
    public bool isSuitable = false;
    public bool isCurSelectFromTileInfo = false;

    public int positionID;
    public int layerID;

    public GameObject grid;
    public GameObject cube;

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

        cube = transform.Find("Cube").gameObject;
        grid = transform.Find("Grid").gameObject;

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

            if (isCurSelectFromTileInfo && layerID != TileInfoManager.instance.layerIDMin)
            {
                grid.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.7f);
                isSuitable = false;
            }
            else
            {
                grid.GetComponent<MeshRenderer>().material.color = new Color(0.0f, 1.0f, 0.8f, 0.4f);
                isSuitable = true;
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

    }
}
