using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using JsonFx.Json;
using System;

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
    public List<GeographyCube> listGeoCube = new List<GeographyCube>();
    public GameObject curSelectCube = null;
    public GameObject floorTileGroup;


    public GameObject curSelectObject = null; //나무 or 건물.
    public List<GeographyCube> listCurSelectCubes = new List<GeographyCube>();
    public string[] arrStrCurSelectObjectName = null;
    public bool isConstruction = false;
    public int layerIDMin;

    public int col = 20; //열이 몇개?
    public int row = 20; //행이 몇개?

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
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < col; x++)
            {
                int _layer = 0; //층
                foreach (int type in stageInfo.arrListCubeInStage[(y * col) + x])
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
                        case (int)EnumCubeType.SlopeUp:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.SlopeUp.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.SlopeUp);
                            }
                            break;
                        case (int)EnumCubeType.SlopeDown:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.SlopeDown.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.SlopeDown);
                            }
                            break;
                        case (int)EnumCubeType.SlopeLeft:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.SlopeLeft.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.SlopeLeft);
                            }
                            break;
                        case (int)EnumCubeType.SlopeRight:
                            {
                                GameObject _tileObj = Instantiate(ResourceManager.instance.floorTile[EnumCubeType.SlopeRight.ToString()]) as GameObject;
                                //좌표에 따른 위치 지정.
                                SetTileOnLand(_tileObj, floorTileGroup, x, y, _layer++, EnumCubeType.SlopeRight);
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
        _obj.transform.localPosition = new Vector3(_x - (0.5f * 0.5f) * (row - 1)
            , _layer
            , -_y + (0.5f * 0.5f) * (col - 1));
        _obj.transform.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
        _obj.transform.localScale = Vector3.one;

        _obj.GetComponent<GeographyCube>().cubeType = _type;
        _obj.GetComponent<GeographyCube>().positionID = _y * col + _x;
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
            stageInfo.arrListCubeInStage = new List<int>[row * col];

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    stageInfo.arrListCubeInStage[i * col + j] = new List<int>(1);
                    stageInfo.arrListCubeInStage[i * col + j].Add((int)EnumCubeType.Normal);
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

        //나중에 UI팝업으로 뺄것.
        if (Input.GetKeyDown(KeyCode.Z) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/Grass")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.Grass;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.C) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/Soil")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.Soil;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.X) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/Water")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.Water;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.V) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeUp")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeUp;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.B) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeDown")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeDown;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.N) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeLeft")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeLeft;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.M) && curSelectCube == null) //예를들어 Normal
        {
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeRight")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeRight;
            curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
            curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
            curSelectCube.SetActive(true);
        }


        //지형 짓기.
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
                if (
                    (hit.transform.parent.GetComponent<GeographyCube>().cubeType != EnumCubeType.Water
                    && hit.transform.parent.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeUp
                    && hit.transform.parent.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeDown
                    && hit.transform.parent.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeLeft
                    && hit.transform.parent.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeRight) ||
                    (
                        hit.transform.parent.GetComponent<GeographyCube>().cubeType == EnumCubeType.Water
                        && curSelectCube.GetComponent<GeographyCube>().cubeType == EnumCubeType.Water
                    ))
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

                    if (curSelectCube.GetComponent<GeographyCube>().cubeType != EnumCubeType.Water
                        //&& curSelectCube.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeUp
                        //&& curSelectCube.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeDown
                        //&& curSelectCube.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeLeft
                        //&& curSelectCube.GetComponent<GeographyCube>().cubeType != EnumCubeType.SlopeRight
                        )
                    {
                        if (curSelectCube.transform.Find("Cube").GetComponent<BoxCollider>() != null)
                        {
                            curSelectCube.transform.Find("Cube").GetComponent<BoxCollider>().enabled = true;
                        }
                        else if (curSelectCube.transform.Find("Cube").GetComponent<MeshCollider>() != null)
                        {
                            curSelectCube.transform.Find("Cube").GetComponent<MeshCollider>().enabled = true;
                        }
                    }

                    curSelectCube = null;
                }
            }
        }


        //건물이나 나무짓기.
        if (isConstruction)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("Cube");
            //mask = ~mask; //반전일 경우.

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                int _stdPositionId = hit.transform.parent.GetComponent<GeographyCube>().positionID;
                int _stdLayerId = hit.transform.parent.GetComponent<GeographyCube>().layerID;

                if (arrStrCurSelectObjectName != null)
                {
                    if (_stdPositionId % col + int.Parse(arrStrCurSelectObjectName[1]) <= col
                        && _stdPositionId / row + int.Parse(arrStrCurSelectObjectName[2]) <= row)
                    {
                        foreach (GeographyCube _cube in listCurSelectCubes)
                        {
                            _cube.isCurSelectFromTileInfo = false;
                        }

                        listCurSelectCubes.Clear();

                        //좌표중 가장 높이 있는것을 찾아야.
                        //그리드에 켜놓기위해.
                        for (int i = 0; i < int.Parse(arrStrCurSelectObjectName[1]); i++)
                        {
                            for (int j = 0; j < int.Parse(arrStrCurSelectObjectName[2]); j++)
                            {
                                var layerIDMax = (from _cube in listGeoCube
                                                  where _cube.positionID == _stdPositionId + (i * col) + j
                                                  group _cube by _cube.layerID into g
                                                  select g.Max(p => p.layerID)).Max();

                                listCurSelectCubes.Add(listGeoCube.Find(_cube => _cube.positionID == _stdPositionId + (i * col) + j && _cube.layerID == layerIDMax));
                            }
                        }

                        layerIDMin = (from _cube in listCurSelectCubes
                                      group _cube by _cube.layerID into g
                                      select g.Min(p => p.layerID)).Min();

                        foreach (GeographyCube _cube in listCurSelectCubes)
                        {
                            _cube.isCurSelectFromTileInfo = true;
                        }

                        //curSelectObject 중앙에 위치시키기 위해.
                        Vector3 _vecStdCube = listCurSelectCubes[0].transform.localPosition;
                        curSelectObject.transform.localPosition = new Vector3(_vecStdCube.x + (int.Parse(arrStrCurSelectObjectName[1]) - 1) * 0.5f
                            , _vecStdCube.y
                            , _vecStdCube.z - (int.Parse(arrStrCurSelectObjectName[2]) - 1) * 0.5f);


                    }
                }
            }
        }



        //놓을곳 지정.
        if (isConstruction && Input.GetMouseButtonUp(0)
            && PopUpManager.instance.listPopUp.Find(popup => popup.name == "PopUpTest") == null)
        {
            bool result = true;

            foreach (GeographyCube _cube in listCurSelectCubes)
            {
                if (!_cube.isSuitable)
                {
                    result = false;
                    break;
                }
            }

            if (result)
            {
                curSelectObject.transform.parent = floorTileGroup.transform;
                curSelectObject.layer = LayerMask.NameToLayer("FieldObject");
                curSelectObject.transform.SetChildLayer(LayerMask.NameToLayer("FieldObject"));

                for (int i = 0; i < int.Parse(arrStrCurSelectObjectName[1]); i++)
                {
                    for (int j = 0; j < int.Parse(arrStrCurSelectObjectName[2]); j++)
                    {
                        int _positionID = listCurSelectCubes[0].positionID;

                        stageInfo.arrListCubeInStage[_positionID + (i * col) + j].Add(int.Parse(arrStrCurSelectObjectName[3]) * 10 + ((i == 0 && j == 0) ? 1 : 0));
                    }
                }

                curSelectObject = null;
                isConstruction = false;
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
    SlopeRight,
    SlopeLeft,
    SlopeUp,
    SlopeDown,
}

public class StageInfo
{
    public List<int>[] arrListCubeInStage { get; set; }

}