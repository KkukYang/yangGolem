﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

public class TileInfoManager : MonoBehaviour
{
    private static TileInfoManager s_instance = null;
    public static TileInfoManager instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(TileInfoManager)) as TileInfoManager;
                if (s_instance == null)
                {
                    Debug.Log("TileInfoManager null");
                }
            }
            return s_instance;
        }
    }

    public StageInfo stageInfo;
    public GameObject floorTileGroup;
    public List<GeographyCube> listGeoCube = new List<GeographyCube>();
    public GameObject curSelectCube = null;

    //Test
    public float testX, testY;

    private void Awake()
    {
        InitializeStage();
    }

    private void Start()
    {
        //바닥타일 배치.
        SetFloorTile();


        //지형 & 오브젝트 배치.
        SetGeography();

    }

    void SetFloorTile()
    {
        float upVal = 0.0001f;

        for (int y = 0; y < CheckLocationByClick.instance.row; y++)
        {
            for (int x = 0; x < CheckLocationByClick.instance.col; x++)
            {
                int _layer = 0; //층
                foreach (int type in stageInfo.arrListCubeInStage[(y * CheckLocationByClick.instance.col) + x])
                {
                    switch (type)
                    {
                        case (int)EnumCubeType.Normal:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.Normal.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.Normal);
                            }
                            break;

                        case (int)EnumCubeType.Grass:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.Grass.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.Grass);

                            }
                            break;

                        case (int)EnumCubeType.Soil:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.Soil.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.Soil);
                            }
                            break;

                        case (int)EnumCubeType.Water:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.Water.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.Water);
                            }
                            break;

                        default:
                            {
                                Debug.Log(x + ", " + y + " exception!!");
                            }
                            break;
                    }
                }
            }
        }

    }

    void SetTileOnLand(GameObject _obj, GameObject _group, int _x, int _y, int _layer, EnumCubeType _type)
    {
        _obj.transform.parent = _group.transform;
        _obj.transform.localPosition = new Vector3(_x * 0.5f - (0.5f * 0.5f) * (CheckLocationByClick.instance.row - 1)
            , _layer * 0.5f
            , _y * -0.5f + (0.5f * 0.5f) * (CheckLocationByClick.instance.col - 1));
        _obj.transform.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
        _obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        _obj.GetComponent<GeographyCube>().cubeType = _type;
        _obj.GetComponent<GeographyCube>().positionID = _y * CheckLocationByClick.instance.col + _x;
        _obj.GetComponent<GeographyCube>().layerID = _layer;

        listGeoCube.Add(_obj.GetComponent<GeographyCube>());

        _obj.SetActive(true);
    }

    void SetGeography()
    {

    }

    private void InitializeStage()
    {
        //바닥 타일 데이터 셋팅.
        Initialize();

    }

    void Initialize()
    {
        string jsonText = GameInfoManager.instance.ReadStringFromFile("stageInfo");
        Debug.Log(jsonText);

        if (jsonText != null)
        {
            stageInfo = JsonReader.Deserialize<StageInfo>(jsonText);
        }
        else
        {
            stageInfo = new StageInfo();
            stageInfo.arrListCubeInStage = new List<int>[CheckLocationByClick.instance.row * CheckLocationByClick.instance.col];

            for (int i = 0; i < CheckLocationByClick.instance.row; i++)
            {
                for (int j = 0; j < CheckLocationByClick.instance.col; j++)
                {
                    stageInfo.arrListCubeInStage[i * CheckLocationByClick.instance.col + j] = new List<int>(1);
                    stageInfo.arrListCubeInStage[i * CheckLocationByClick.instance.col + j].Add((int)EnumCubeType.Normal);
                }
            }
        }
    }


    #region InitTileInfoFromCsv
    //private int[,] InitTileInfoFromCsv(List<int>[,] arrList, int stage = 0)
    //{
    //    TextAsset data;
    //    data = Resources.Load("CSVs/FloorTileInfo_" + stage) as TextAsset;

    //    if (data == null)
    //    {
    //        Debug.LogError("> [ConfigManager: Csv Load Fail]");
    //    }

    //    System.StringSplitOptions option = System.StringSplitOptions.RemoveEmptyEntries;
    //    string[] lines = data.text.Split(new char[] { '\r', '\n' }, option);
    //    char[] spliter = new char[1] { ',' };

    //    for (int y = 0; y < lines.Length; y++)
    //    {
    //        string[] temp = lines[y].Split(spliter, option);

    //        for (int x = 0; x < temp.Length; x++)
    //        {
    //            arrLayer[y, x] = int.Parse(temp[x]);
    //        }
    //    }

    //    return arrLayer;
    //}
    #endregion


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/Grass")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.Grass;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }

        if (curSelectCube != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("Cube");

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Vector3 vecHit = hit.transform.position;
                curSelectCube.transform.position = new Vector3(vecHit.x, vecHit.y + 1.5f, vecHit.z);
                curSelectCube.transform.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
            }

            if (Input.GetMouseButtonDown(0))
            {
                int _positionID = hit.transform.parent.GetComponent<GeographyCube>().positionID;
                int _layerID = hit.transform.parent.GetComponent<GeographyCube>().layerID;

                curSelectCube.transform.parent = floorTileGroup.transform;
                curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = false;
                curSelectCube.GetComponent<GeographyCube>().positionID = _positionID;
                curSelectCube.GetComponent<GeographyCube>().layerID = _layerID + 1;
                curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Cube"));
                stageInfo.arrListCubeInStage[_positionID].Add((int)curSelectCube.GetComponent<GeographyCube>().cubeType);
                listGeoCube.Add(curSelectCube.GetComponent<GeographyCube>());

                curSelectCube = null;
            }
        }


    }

    private void OnApplicationQuit()
    {
        string jsonText = JsonWriter.Serialize(stageInfo);
        Debug.Log(jsonText);
        GameInfoManager.instance.WriteStringToFile(jsonText, "stageInfo");
    }
}


public enum EnumFieldObject
{
    NULL = 0,
    Tree,
    Geography,
}

public enum EnumCubeType
{
    NULL = 0,
    Normal,
    Grass,
    Soil,
    Water,
    Slope,
}

public class StageInfo
{
    public List<int>[] arrListCubeInStage { get; set; }

}