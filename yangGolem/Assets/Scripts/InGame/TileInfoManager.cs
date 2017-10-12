using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using JsonFx.Json;
using LitJson;
using System.IO;

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
    public List<FieldObject> listFieldObject = new List<FieldObject>();
    public GameObject curSelectCube = null;
    public GameObject floorTileGroup;
    public GameObject fieldObjectGroup;
    public GameObject monsterGroup;


    public GameObject curSelectObject = null; //나무 or 건물.
    public List<GeographyCube> listCurSelectCubes = new List<GeographyCube>();
    public string[] arrStrCurSelectObjectName = null;
    public bool isConstruction = false;
    public int layerIDMin;

    public int col; //열이 몇개?
    public int row; //행이 몇개?

    public GameObject heroObj;
    public int viewAround; //hero반경 으로 몇정도 내다볼지. 

    //public int heroPosIndex = 0;
    //public int heroLayer = 0;
    //public bool isPicked = false; //픽이 되었는지.
    //public float pickedTimer = 0.0f; //몇초간 눌렸는지 타이머.
    public Texture2D textureMap;
    public float eachTileScale = 1.5f;


    //Test
    //public float testX, testY;
    //public bool isInitMapCube;  //강제 런타임하기전에 초기화 설정.
    //public bool isRenderCube;   //Q누르면 렌더 지워짐.

    public int testInitPlayerPosX;
    public int testInitPlayerPosY;

    private void Awake()
    {
        InitializeStage();

        //타일에서의 몬스터 스폰정보 파싱해야함.

        //stageInfo.heroPos = (int)((row / 2) * col + (col / 2));  //정중앙.
        //heroObj.transform.localPosition = new Vector3((col / 2) - (0.5f * 0.5f) * (row - 1)
        //    , heroObj.transform.localPosition.y
        //    , -(row / 2) + (0.5f * 0.5f) * (col - 1)); // 정중앙

        //Hero Set Position.
        heroObj.transform.localPosition = new Vector3((stageInfo.heroPos % row) * eachTileScale - (0.5f * 0.5f) * (row - 1)
            , heroObj.transform.localPosition.y + stageInfo.heroLayer * eachTileScale
            , (stageInfo.heroPos / col) * eachTileScale + (0.5f * 0.5f) * (col - 1));

    }


    private void Start()
    {
        //지형 & 오브젝트 배치.
        SetGeographyAndFieldObject();

        StartCoroutine("UpdateGeographyAndFieldObject");
    }


    IEnumerator UpdateGeographyAndFieldObject()
    {
        int preHeroPosition = 0;
		Player _player = heroObj.GetComponent<Player>();
        while (true)
        {
            //이전 위치와 다르다면 범위 타일 업데이트 해줘야.
			if (_player.curBottomPositionID != preHeroPosition)
            {
                List<GeographyCube> _tempListCube = (from _cube in listGeoCube
					                                where (_player.curBottomPositionID / row) - viewAround > (_cube.positionID / row)
					                                || (_player.curBottomPositionID / row) + viewAround < (_cube.positionID / row)
					                                || (_player.curBottomPositionID % col) - viewAround > (_cube.positionID % col)
					                                || (_player.curBottomPositionID % col) + viewAround < (_cube.positionID % col)
                                                    select _cube).ToList();

                foreach (GeographyCube _cube in _tempListCube)
                {
                    listGeoCube.Remove(_cube);
                    _cube.transform.parent = ResourceManager.instance.cubeBox.transform;
                    _cube.gameObject.SetActive(false);
                }


                List<FieldObject> _tempListFieldObj = (from _fieldObj in listFieldObject
                                                     where (_player.curBottomPositionID / row) - viewAround > (_fieldObj.positionID / row)
                                                     || (_player.curBottomPositionID / row) + viewAround < (_fieldObj.positionID / row)
                                                     || (_player.curBottomPositionID % col) - viewAround > (_fieldObj.positionID % col)
                                                     || (_player.curBottomPositionID % col) + viewAround < (_fieldObj.positionID % col)
                                                     select _fieldObj).ToList();

                foreach (FieldObject _fieldObj in _tempListFieldObj)
                {
                    listFieldObject.Remove(_fieldObj);
                    _fieldObj.transform.parent = ResourceManager.instance.fieldObjectBox.transform;
                    _fieldObj.gameObject.SetActive(false);
                }


                List<MonsterBehaviour> _tempListMonsterObj = (from _monsterObj in MonsterManager.instance.listMonster
                                                        where (_player.curBottomPositionID / row) - (viewAround) > (_monsterObj.GetComponent<MonsterBehaviour>().monsterInfo.positionID / row)
                                                        || (_player.curBottomPositionID / row) + (viewAround) < (_monsterObj.GetComponent<MonsterBehaviour>().monsterInfo.positionID / row)
                                                        || (_player.curBottomPositionID % col) - (viewAround) > (_monsterObj.GetComponent<MonsterBehaviour>().monsterInfo.positionID % col)
                                                        || (_player.curBottomPositionID % col) + (viewAround) < (_monsterObj.GetComponent<MonsterBehaviour>().monsterInfo.positionID % col)
                                                        select _monsterObj.GetComponent<MonsterBehaviour>()).ToList();

                foreach (MonsterBehaviour _monsterObj in _tempListMonsterObj)
                {
                    MonsterManager.instance.listMonster.Remove(_monsterObj);
                    _monsterObj.transform.parent = ResourceManager.instance.monsterBox.transform;
                    _monsterObj.gameObject.SetActive(false);
                }

                stageInfo.heroPos = _player.curBottomPositionID;
				stageInfo.heroLayer = _player.curBottomLayerID;

                SetGeographyAndFieldObject();
            }

			preHeroPosition = _player.curBottomPositionID;

            yield return null;
        }
    }

    void SetGeographyAndFieldObject() //기존에는 모두 깔았는데 이번엔 히어로 좌표 기준으로 범위만큼 깔아둔다.
    {

        for (int y = (stageInfo.heroPos / col) - viewAround; y < (stageInfo.heroPos / col) + viewAround + 1; y++)
        //for (int y = 0; y < row; y++)
        {
            for (int x = (stageInfo.heroPos % col) - viewAround; x < (stageInfo.heroPos % col) + viewAround + 1; x++)
            //for (int x = 0; x < col; x++)
            {
                if (y < 0 || x < 0 || y > row - 1 || x > col - 1)
                {
                    continue;
                }

                if ((from _cube in listGeoCube
                     where _cube.positionID == (y * col) + x
                     select _cube).ToList().Count > 0)
                {
                    continue;
                }

                int _layer = 0; //층
                for (int idx = 0; idx < stageInfo.arrListCubeInStage[(y * col) + x].Count; idx++)
                {
                    int _type = stageInfo.arrListCubeInStage[(y * col) + x][idx];
                    bool _isExistOnCube = (stageInfo.arrListCubeInStage[(y * col) + x].Count - 2 > idx) ? true : false; //마지막하나는 무조건 필드오브젝트 있음. 그래서 -2

                    ////이거풀면 밑에 _layer++; 없애야함. //field object는 ++_layer 해줘야.
                    //if (_isExistOnCube)
                    //{
                    //    _layer++;
                    //    continue;
                    //}

                    switch (_type)
                    {
                        case (int)EnumCubeType.StartArea:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.StartArea, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.RankGrass:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.RankGrass, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.HalfGrass:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.HalfGrass, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.Grass:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.Grass, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.GrassSand:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.GrassSand, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.Sand:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.Sand, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.Water:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.Water, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.SlopeLeft:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.SlopeLeft, _isExistOnCube));
                            }
                            break;
                        case (int)EnumCubeType.SlopeRight:
                            {
                                StartCoroutine(SetTileOnLand(floorTileGroup, x, y, _layer++, EnumCubeType.SlopeRight, _isExistOnCube));
                            }
                            break;
                        default:
                            {
                                switch (_type / 10)
                                {
                                    case (int)EnumFieldObject.NULL:
                                        break;
                                    case (int)EnumFieldObject.Rock:
                                        {
                                            SetFieldOnLand(fieldObjectGroup, x, y, _layer, EnumFieldObject.Rock);
                                        }
                                        break;
                                    case (int)EnumFieldObject.Tree:
                                        {
                                            SetFieldOnLand(fieldObjectGroup, x, y, _layer, EnumFieldObject.Tree);
                                        }
                                        break;
                                    case 25:
                                    case 255:
                                        break;
                                    default:
                                        {
                                            Debug.Log("Execption!! : " + _type + ", " + x + ", " + y);
                                        }
                                        break;
                                }
                            }
                            break;
                    }

                    //_layer++;
                }
            }
        }
    }


    IEnumerator SetTileOnLand(GameObject _group, int _x, int _y, int _layer, EnumCubeType _type, bool _isExistOnCube)
    {
        GameObject _obj = null;
        int _positionID = _y * col + _x;

        if (ResourceManager.instance.cubeBox.transform.Find(_type.ToString()) == null)
        {
            try
            {
                _obj = Instantiate(ResourceManager.instance.floorTile[_type.ToString()]) as GameObject;
            }
            catch
            {
                Debug.Log("execption SetTileOnLand() : " + _type.ToString());
            }
        }
        else
        {
            _obj = ResourceManager.instance.cubeBox.transform.Find(_type.ToString()).gameObject;
        }

        //if (ResourceManager.instance.cubeBox.transform.Find(_type.ToString()) == null)
        //{
        //	_obj = Instantiate(ResourceManager.instance.floorTile["_Normal"]) as GameObject;
        //}
        //else
        //{
        //	_obj = ResourceManager.instance.cubeBox.transform.Find(_type.ToString()).gameObject;
        //}


        _obj.name = _type.ToString();
        _obj.transform.parent = _group.transform;
		if (_isExistOnCube) {
			_obj.transform.localScale = Vector3.one * eachTileScale;
		} else {
			_obj.transform.localScale = new Vector3(eachTileScale, Random.Range(0.8f, 1.0f) * eachTileScale, eachTileScale);
        }

        _obj.transform.localPosition = new Vector3(_x * eachTileScale - (0.5f * 0.5f) * (row - 1)
            , _layer * eachTileScale
            , _y * eachTileScale + (0.5f * 0.5f) * (col - 1));
        _obj.transform.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);

        _obj.GetComponent<GeographyCube>().cubeType = _type;
        _obj.GetComponent<GeographyCube>().positionID = _positionID;
        _obj.GetComponent<GeographyCube>().layerID = _layer;
        _obj.GetComponent<GeographyCube>().isExistOnCube = _isExistOnCube;

        listGeoCube.Add(_obj.GetComponent<GeographyCube>());

        _obj.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        //몬스터 스폰. 이미 등록되어 있는 얘들을 뿌린다. 먼저 해당 타일에 있는지 판단하자.
        MonsterInfo monsterInfo = null;
        foreach (var _monsterGroup in MonsterManager.instance.monsterInfoInStage.dicMonsterInfo)
        {
            monsterInfo = _monsterGroup.Value.Find(_monsterInfo => _monsterInfo.positionID == _positionID);

            if (monsterInfo != null)
            {
                //등록된, 몬스터 생성. 리스트에 넣기.
                GameObject _monster = null;

                if (ResourceManager.instance.monsterBox.transform.Find(monsterInfo.monsterName) == null)
                {
                    _monster = Instantiate(ResourceManager.instance.monster[monsterInfo.monsterName]) as GameObject;
                }
                else
                {
                    _monster = ResourceManager.instance.monsterBox.transform.Find(monsterInfo.monsterName).gameObject;
                }

                _monster.name = monsterInfo.monsterName;
                _monster.GetComponent<MonsterBehaviour>().monsterInfo = monsterInfo;
                _monster.transform.parent = monsterGroup.transform;
                _monster.transform.localScale = Vector3.one * 1.5f;
                _monster.transform.position = new Vector3(_obj.transform.position.x
                    , _obj.transform.Find("InvisibleCube").GetComponent<BoxCollider>().bounds.max.y
                    , _obj.transform.position.z);
                //_monster.GetComponent<NavMeshAgent>().enabled = false;

                MonsterManager.instance.listMonster.Add(_monster.GetComponent<MonsterBehaviour>());
                _monster.SetActive(true);
            }
        }


        ////몬스터 스폰. 이 함수가 호출되는 것이 낮은 확률로.
        //if(monsterInfo != null)
        //{
        //    if (Random.Range(0.0f, 100.0f) <= MonsterManager.instance.monsterSpawnRate)
        //    {
        //        MonsterSpawn(_type);
        //    }
        //}
    }


    void SetFieldOnLand(GameObject _group, int _x, int _y, int _layer, EnumFieldObject _type)
    {

        GameObject _obj = null;
        _obj = Instantiate(ResourceManager.instance.fieldObj[_type.ToString()
			+ ((_type == EnumFieldObject.Rock) ? ("_" + Random.Range(1, 4 + 1)).ToString() : "_2")
            ]) as GameObject;

        //string[] arrStr = _obj.name.Split('_');

        _obj.name = _type.ToString();
        _obj.transform.parent = _group.transform;
        _obj.transform.localScale = new Vector3(eachTileScale, Random.Range(0.7f, 1.0f) * eachTileScale, eachTileScale);
        _obj.transform.localPosition = new Vector3((_x * eachTileScale - (0.5f * 0.5f) * (row - 1))// + (int.Parse(arrStr[1]) - 1) * 0.5f
            , _layer * eachTileScale
            , _y * eachTileScale + (0.5f * 0.5f) * (col - 1));// - (int.Parse(arrStr[2]) - 1) * 0.5f);
        _obj.transform.eulerAngles = new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f);

        _obj.transform.SetChildLayer(LayerMask.NameToLayer("FieldObject"));
        _obj.layer = LayerMask.NameToLayer("FieldObject");

        _obj.GetComponent<FieldObject>().objType = _type;
        _obj.GetComponent<FieldObject>().positionID = _y * col + _x;
        _obj.GetComponent<FieldObject>().layerID = _layer;

        listFieldObject.Add(_obj.GetComponent<FieldObject>());

        _obj.SetActive(true);
    }



    private void InitializeStage()
    {
        //바닥 타일 데이터 셋팅. 실제 큐브 객체생성은 하지 않음.

        string jsonText = GameInfoManager.instance.ReadStringFromFile("stageInfo");
        Debug.Log("InitializeStage() : " + jsonText);

        if (jsonText != null && !GameInfoManager.instance.isInit)
        {
            stageInfo = JsonFx.Json.JsonReader.Deserialize<StageInfo>(jsonText);
        }
        else
        {
            stageInfo = new StageInfo();

            stageInfo.heroPos = testInitPlayerPosY * col + testInitPlayerPosX;
            stageInfo.heroLayer = 0;
            stageInfo.arrListCubeInStage = new List<int>[row * col];

            for (int y= 0; y < row; y++)
            {
                for (int x = 0; x < col;x++)
                {
                    stageInfo.arrListCubeInStage[y * col + x] = new List<int>();

                    Color32 tempPixelColor32 = textureMap.GetPixel(x, y);

                    for (int idx = 0; idx < (26 - (int)tempPixelColor32.a / 10); idx++)
                    {
                        int _type = (int)tempPixelColor32.r;    //큐브.
                        stageInfo.arrListCubeInStage[y * col + x].Add(_type);

                        _type = (int)tempPixelColor32.g;    //필드오브젝트.
                        stageInfo.arrListCubeInStage[y * col + x].Add(_type * 10 + 1);  //진쨰배기는 1을 더해준다.
                    }

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
        ////나중에 UI팝업으로 뺄것.
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
            curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/Sand")) as GameObject;
            curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.Sand;
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
        //else if (Input.GetKeyDown(KeyCode.V) && curSelectCube == null) //예를들어 Normal
        //{
        //    curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeUp")) as GameObject;
        //    curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeUp;
        //    curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
        //    curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
        //    curSelectCube.SetActive(true);
        //}
        //else if (Input.GetKeyDown(KeyCode.B) && curSelectCube == null) //예를들어 Normal
        //{
        //    curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeDown")) as GameObject;
        //    curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeDown;
        //    curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
        //    curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
        //    curSelectCube.SetActive(true);
        //}
        //else if (Input.GetKeyDown(KeyCode.N) && curSelectCube == null) //예를들어 Normal
        //{
        //    curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeLeft")) as GameObject;
        //    curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeLeft;
        //    curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
        //    curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
        //    curSelectCube.SetActive(true);
        //}
        //else if (Input.GetKeyDown(KeyCode.M) && curSelectCube == null) //예를들어 Normal
        //{
        //    curSelectCube = Instantiate(Resources.Load("Prefabs/FloorTile/SlopeRight")) as GameObject;
        //    curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeRight;
        //    curSelectCube.GetComponent<GeographyCube>().isCurSelectFromTileInfo = true;
        //    curSelectCube.transform.SetChildLayer(LayerMask.NameToLayer("Default"));
        //    curSelectCube.SetActive(true);
        //}


        //지형 짓기.
        if (curSelectCube != null)// && isPicked == false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int mask = 1 << LayerMask.NameToLayer("Cube");

            if (Physics.Raycast(ray, out hit, 100.0f, mask))
            {
                Vector3 vecHit = hit.transform.position;
                curSelectCube.transform.position = new Vector3(vecHit.x
                    , hit.transform.GetComponent<BoxCollider>().bounds.max.y
                    , vecHit.z);
                curSelectCube.transform.eulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
                curSelectCube.transform.localScale = Vector3.one * eachTileScale;
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.transform.parent.GetComponent<GeographyCube>().isExistOnCube == false)
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
                            if (curSelectCube.transform.Find("InvisibleCube/Cube").GetComponent<BoxCollider>() != null)
                            {
                                curSelectCube.transform.Find("InvisibleCube/Cube").GetComponent<BoxCollider>().enabled = true;
                            }
                            else if (curSelectCube.transform.Find("InvisibleCube/Cube").GetComponent<MeshCollider>() != null)
                            {
                                curSelectCube.transform.Find("InvisibleCube/Cube").GetComponent<MeshCollider>().enabled = true;
                            }
                        }

                        curSelectCube = null;
                    }
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
                //int _stdLayerId = hit.transform.parent.GetComponent<GeographyCube>().layerID;

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
                                if (listGeoCube.Find(_cube => _cube.positionID == _stdPositionId + (i * col) + j) != null)
                                {
                                    var layerIDMax = (from _cube in listGeoCube
                                                      where _cube.positionID == _stdPositionId + (i * col) + j
                                                      group _cube by _cube.layerID into g
                                                      select g.Max(p => p.layerID)).Max();

                                    listCurSelectCubes.Add(listGeoCube.Find(_cube => _cube.positionID == _stdPositionId + (i * col) + j && _cube.layerID == layerIDMax));
                                }
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
                        Vector3 _vecStdCube = listCurSelectCubes[0].transform.localPosition; //기준큐브의 좌표 인덱스 0.
                        curSelectObject.transform.localPosition = new Vector3(_vecStdCube.x + (int.Parse(arrStrCurSelectObjectName[1]) - 1) * 0.5f
                            , _vecStdCube.y
                            , _vecStdCube.z - (int.Parse(arrStrCurSelectObjectName[2]) - 1) * 0.5f);

                    }
                }
            }
        }



        //놓을곳 지정. //필드오브젝트
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
                foreach (GeographyCube _cube in listCurSelectCubes)
                {
                    _cube.isExistOnCube = true;
                    _cube.isCurSelectFromTileInfo = false;
                }

                curSelectObject.transform.parent = floorTileGroup.transform;
                curSelectObject.layer = LayerMask.NameToLayer("FieldObject");
                curSelectObject.transform.SetChildLayer(LayerMask.NameToLayer("FieldObject"));

                for (int i = 0; i < int.Parse(arrStrCurSelectObjectName[1]); i++)
                {
                    for (int j = 0; j < int.Parse(arrStrCurSelectObjectName[2]); j++)
                    {
                        int _positionID = listCurSelectCubes[0].positionID;

                        //stageInfo.arrListCubeInStage[_positionID + (i * col) + j].Add(int.Parse(arrStrCurSelectObjectName[3]) * 10 + ((i == 0 && j == 0) ? 1 : 0));
                    }
                }


                curSelectObject = null;
                isConstruction = false;
            }
        }


        ////배치된 오브젝트 수정
        ////Select Located Object.
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    int mask = 1 << LayerMask.NameToLayer("Cube");

        //    if (Physics.Raycast(ray, out hit, 100.0f, mask))
        //    {
        //        if(hit.transform.parent.GetComponent<GeographyCube>() != null 
        //            || hit.transform.parent.GetComponent<FieldObject>() != null)
        //        {
        //            isPicked = true;
        //            pickedTimer = 0.0f;
        //        }
        //    }
        //}
        //else if (Input.GetMouseButton(0))
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    int mask = 1 << LayerMask.NameToLayer("Cube");

        //    if (Physics.Raycast(ray, out hit, 100.0f, mask))
        //    {
        //        if (hit.transform.parent.GetComponent<GeographyCube>() != null
        //            || hit.transform.parent.GetComponent<FieldObject>() != null)
        //        {
        //            if (isPicked)
        //            {
        //                pickedTimer += Time.deltaTime;
        //            }

        //            if (pickedTimer >= 1.0f)
        //            {
        //                if (hit.transform.parent.GetComponent<GeographyCube>() != null)
        //                {
        //                    GeographyCube pickedCube = hit.transform.parent.GetComponent<GeographyCube>();

        //                    curSelectCube = pickedCube.gameObject;
        //                    pickedCube.isCurSelectFromTileInfo = true;
        //                    transform.SetChildLayer(LayerMask.NameToLayer("Default"));

        //                    stageInfo.arrListCubeInStage[pickedCube.positionID].Remove((int)pickedCube.cubeType);
        //                    listGeoCube.Remove(pickedCube);

        //                    {
        //                        int layerMax = 0;
        //                        List<GeographyCube> _listCube = listGeoCube.FindAll(_cube => _cube.positionID == pickedCube.positionID);

        //                        foreach (var _cube in _listCube)
        //                        {
        //                            if (_cube.layerID > layerMax)
        //                            {
        //                                layerMax = _cube.layerID;
        //                            }
        //                        }

        //                        if (_listCube.Count > 0)
        //                        {
        //                            GeographyCube cube = _listCube.Find(_cube => _cube.layerID == layerMax);
        //                            cube.isExistOnCube = false;
        //                        }
        //                    }

        //                    //{
        //                    //    int layerMax = (from listA in TileInfoManager.instance.listGeoCube.FindAll(_cube => _cube.positionID == positionID)
        //                    //                    group listA by listA.layerID into g
        //                    //                    select g.Max(m => m.layerID)).First();

        //                    //    Debug.Log(layerMax);

        //                    //    GeographyCube cube = TileInfoManager.instance.listGeoCube.FindAll(_cube => _cube.positionID == positionID).Find(_cube => _cube.layerID == layerMax);
        //                    //    cube.isExistOnCube = false;
        //                    //}
        //                }
        //                else if(hit.transform.parent.GetComponent<FieldObject>() != null)
        //                {

        //                }

        //            }
        //        }
        //    }
        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    isPicked = false;
        //    pickedTimer = 0.0f;
        //}

    }


    private void OnApplicationQuit()
    {
        //히어로가 마지막에 있던곳을 저장하자.
		stageInfo.heroPos = heroObj.GetComponent<Player>().curBottomPositionID;
		stageInfo.heroLayer = heroObj.GetComponent<Player>().curBottomLayerID;

        string jsonText = JsonFx.Json.JsonWriter.Serialize(stageInfo);
        Debug.Log("stageInfo : " + jsonText);
        GameInfoManager.instance.WriteStringToFile(jsonText, "stageInfo");
    }
}


public enum EnumFieldObject
{
    NULL = 0,
    Tree = 100,
    Rock = 180,
}

public enum EnumCubeType
{
    NULL        = -1,
    StartArea   = 0,
    RankGrass   = 100,
    HalfGrass   = 120,
    Grass       = 140,
    GrassSand   = 160,
    Sand        = 180,
    Water       = 200,
    SlopeRight,
    SlopeLeft,
    SlopeUp,
    SlopeDown,
}

public class StageInfo //저장용.
{
    public List<int>[] arrListCubeInStage { get; set; } //플레이어가 조작한 것은 여기다 저장.
    public int heroPos { get; set; }
    public int heroLayer { get; set; }
}

