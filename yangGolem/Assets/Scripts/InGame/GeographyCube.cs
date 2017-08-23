using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeographyCube : MonoBehaviour
{
    public EnumCubeType cubeType;
    public bool isEnableBuild = false;
    public bool isCurSelectFromTileInfo = false;

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
    }

    void Start()
    {

    }

    void Update()
    {
        if (TileInfoManager.instance.curSelectCube != null && isEnableBuild && !isCurSelectFromTileInfo)
        {
            transform.Find("Grid").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("Grid").gameObject.SetActive(false);
        }

    }
}
