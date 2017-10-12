using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;
using LitJson;


//Spwanning operate in (TileInfoManager.cs).
public class MonsterManager : MonoBehaviour
{
    private static MonsterManager s_instance = null;
    public static MonsterManager instance
    {
        get
        {
            if(s_instance == null)
            {
                s_instance = FindObjectOfType(typeof(MonsterManager)) as MonsterManager;

                if(s_instance == null)
                {
                    Debug.Log("MonsterManager null");
                }
            }

            return s_instance;
        }
    }

    public MonsterInfoInStage monsterInfoInStage = null;
    public float monsterSpawnRate = 10.0f;
    public int[] arrMonsterCnt;

    public List<MonsterBehaviour> listMonster = new List<MonsterBehaviour>(); //보이는 몬스터 관리

    //MonsterManager는 독립적으로 가상의 몬스터를 갖고 있어야 함.
    private void Awake()
    {

    }


    void Start()
    {
        InitializeMonsterInfo();
    }


    private void InitializeMonsterInfo()
    {
        //바닥 타일 데이터 셋팅. 실제 큐브 객체생성은 하지 않음.

        string jsonText = GameInfoManager.instance.ReadStringFromFile("monsterInfo");
        Debug.Log("InitializeMonsterInfo() : " + jsonText);

        if (jsonText != null && !GameInfoManager.instance.isInit)
        {
            monsterInfoInStage = JsonMapper.ToObject<MonsterInfoInStage>(jsonText);
        }
        else
        {
            monsterInfoInStage = new MonsterInfoInStage();
            //monsterInfoInStage.dicMonsterMaxCount = new Dictionary<string, int>();
            monsterInfoInStage.dicMonsterMaxCount.Add(((int)MONSTERTYPE.Chicken).ToString(), arrMonsterCnt[0]);
            monsterInfoInStage.dicMonsterMaxCount.Add(((int)MONSTERTYPE.Pig).ToString(), arrMonsterCnt[1]);
            monsterInfoInStage.dicMonsterMaxCount.Add(((int)MONSTERTYPE.Slime).ToString(), arrMonsterCnt[2]);
            monsterInfoInStage.dicMonsterMaxCount.Add(((int)MONSTERTYPE.Wolf).ToString(), arrMonsterCnt[3]);

            //monsterInfoInStage.dicMonsterInfo = new Dictionary<string, List<MonsterInfo>>();
            //MonsterInfo 생성. 타일의 확률에 따라서 몬스터가 생성됨.
            int _color32Idx = 0;
            Color32[] arrColorMap = TileInfoManager.instance.textureMap.GetPixels32();

            Debug.Log("arrcolor.length : " + arrColorMap.Length);

            foreach (Color32 _color32 in arrColorMap)
            {
                if (Random.Range(0.0f, 100.0f) <= monsterSpawnRate)
                {
                    MonsterSpawn(_color32.r, _color32Idx);
                }
                _color32Idx++;
            }
        }

        foreach(var _a in monsterInfoInStage.dicMonsterInfo)
        {
            Debug.Log(_a.Key + " : " + _a.Value.Count);
        }
    }


    void MonsterSpawn(int _type, int _positionID) // MonsterManager에 MonsterInfo만 생성해 놓고, 나중에 몬스터 스폰할때, 넣어줌.
    {
        int totalRate = 0;
        //int cycle = dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate.Count;
        int randomVal = 0;
        int tempRate = 0;
        int monsterID = 0;

        if (!GameInfoManager.instance.dicMonsterSpawnInfoEachTile.ContainsKey(_type))
        {
            Debug.Log("not exist Tile In MonsterSpawn(int _type) : " + _type);
            return;
        }

        foreach (KeyValuePair<int, string[]> rateKeyValue in GameInfoManager.instance.dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate)
        {
            totalRate += int.Parse(rateKeyValue.Value[1]);
        }

        randomVal = Random.Range(0, totalRate + 1);

        //확률에 적용.
        foreach (KeyValuePair<int, string[]> rateKeyValue in GameInfoManager.instance.dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate)
        {
            tempRate += int.Parse(rateKeyValue.Value[1]);

            if (randomVal < tempRate)
            {
                monsterID = int.Parse(rateKeyValue.Value[0]);    //몬스터ID.
                break;
            }
        }


        //갯수를 초과하진 않았는지.
        if(monsterInfoInStage.dicMonsterInfo.ContainsKey(monsterID.ToString()))
        {
            if (monsterInfoInStage.dicMonsterInfo[monsterID.ToString()].Count >= monsterInfoInStage.dicMonsterMaxCount[monsterID.ToString()])
            {
                return;
            }
        }


        //뽑은 monsterID로 몬스터 생성.
        switch (monsterID)
        {
            case (int)MONSTERTYPE.Chicken:
                {
                    Debug.Log("Chicken " + _positionID);
                    MonsterInfo _monsterInfo = new MonsterInfo();
                    _monsterInfo.monsterID = monsterID;
                    _monsterInfo.monsterName = MONSTERTYPE.Chicken.ToString();
                    _monsterInfo.positionID = _positionID;
                    _monsterInfo.layerID = 0;   //아직 모름.
                    AddMonster(_monsterInfo);

                }
                break;
            case (int)MONSTERTYPE.Pig:
                {
                    Debug.Log("Pig " + _positionID);
                    MonsterInfo _monsterInfo = new MonsterInfo();
                    _monsterInfo.monsterID = monsterID;
                    _monsterInfo.monsterName = MONSTERTYPE.Pig.ToString();
                    _monsterInfo.positionID = _positionID;
                    _monsterInfo.layerID = 0;   //아직 모름.
                    AddMonster(_monsterInfo);

                }
                break;
            case (int)MONSTERTYPE.Slime:
                {
                    Debug.Log("Slime " + _positionID);
                    MonsterInfo _monsterInfo = new MonsterInfo();
                    _monsterInfo.monsterID = monsterID;
                    _monsterInfo.monsterName = MONSTERTYPE.Slime.ToString();
                    _monsterInfo.positionID = _positionID;
                    _monsterInfo.layerID = 0;   //아직 모름.
                    AddMonster(_monsterInfo);

                }
                break;
            case (int)MONSTERTYPE.Wolf:
                {
                    Debug.Log("Wolf " + _positionID);
                    MonsterInfo _monsterInfo = new MonsterInfo();
                    _monsterInfo.monsterID = monsterID;
                    _monsterInfo.monsterName = MONSTERTYPE.Wolf.ToString();
                    _monsterInfo.positionID = _positionID;
                    _monsterInfo.layerID = 0;   //아직 모름.
                    AddMonster(_monsterInfo);

                }
                break;
            default:
                {
                    Debug.Log("Exception monster");
                }
                break;
        }
    }


    /*
    void MonsterSpawn(EnumCubeType _type)
    {
        int totalRate = 0;
        //int cycle = dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate.Count;
        int randomVal = 0;
        int tempRate = 0;
        int monsterID = 0;

        foreach (KeyValuePair<int, string[]> rateKeyValue in dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate)
        {
            totalRate += int.Parse(rateKeyValue.Value[1]);
        }

        randomVal = Random.Range(0, totalRate + 1);

        //확률에 적용.
        foreach (KeyValuePair<int, string[]> rateKeyValue in dicMonsterSpawnInfoEachTile[(int)_type].dicMonsterGenRate)
        {
            tempRate += int.Parse(rateKeyValue.Value[1]);

            if(randomVal < tempRate)
            {
                monsterID = int.Parse(rateKeyValue.Value[0]);    //몬스터ID.
                break;
            }
        }

        //뽑은 monsterID로 몬스터 생성.
        switch(monsterID)
        {
            //case (int)MONSTERTYPE.Normal:
            //    {
            //        Debug.Log("Normal");
            //    }
            //    break;
            case (int)MONSTERTYPE.Chicken:
                {
                    Debug.Log("Chicken");

                }
                break;
            case (int)MONSTERTYPE.Pig:
                {
                    Debug.Log("Pig");

                }
                break;
            case (int)MONSTERTYPE.Slime:
                {
                    Debug.Log("Slime");

                }
                break;
            case (int)MONSTERTYPE.Wolf:
                {
                    Debug.Log("Wolf");

                }
                break;
            default:
                {
                    Debug.Log("Exception monster");
                }
                break;
        }
    }
         */

    public void AddMonster(MonsterInfo _monsterInfo)
    {
        if (monsterInfoInStage.dicMonsterInfo.ContainsKey(_monsterInfo.monsterID.ToString()))
        {
            //이미 등록이 되어있다면. 큐도 생성 되어 있단것.
            monsterInfoInStage.dicMonsterInfo[_monsterInfo.monsterID.ToString()].Add(_monsterInfo);
        }
        else
        {
            //생성 해줘야됨.
            List<MonsterInfo> _tempListMonsterInfo = new List<MonsterInfo>();
            _tempListMonsterInfo.Add(_monsterInfo);
            monsterInfoInStage.dicMonsterInfo.Add(_monsterInfo.monsterID.ToString(), _tempListMonsterInfo);
        }

    }


    void Update()
    {
        monsterInfoInStage.ToString();
        listMonster.ToString();
    }

    private void OnApplicationQuit()
    {
        string jsonText = JsonMapper.ToJson(monsterInfoInStage);
        Debug.Log("monsterInfo : " + jsonText);
        GameInfoManager.instance.WriteStringToFile(jsonText, "monsterInfo");
    }
}


public enum MONSTERTYPE
{
    //Normal  = 0,
    Chicken = 10000,
    Pig     = 10001,
    Slime = 10002,
    Wolf = 10003,
}