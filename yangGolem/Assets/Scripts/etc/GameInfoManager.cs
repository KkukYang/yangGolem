using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;
using LitJson;
using System.IO;

//외부데이터 파싱해오는 정도급의 용도. // 게임의 정보. 버전 등등.

public class GameInfoManager : MonoBehaviour
{
    private static GameInfoManager s_instance = null;
    public static GameInfoManager instance
    {
        get
        {
            s_instance = FindObjectOfType(typeof(GameInfoManager)) as GameInfoManager;
            return (s_instance != null) ? s_instance : null;
        }
    }

    //public UserDataInfo userDataInfo;
    public float timeScale = 1.0f;

    public PlayerInventory playerInventory;

    public bool isInit = false;

    public Dictionary<int, MonsterSpawnInfoFromJson> dicMonsterSpawnInfoEachTile = new Dictionary<int, MonsterSpawnInfoFromJson>();  // 파싱해놓은 참고데이터.
    public Dictionary<int, FieldObjectInfoFromJson> dicFieldObjectInfo = new Dictionary<int, FieldObjectInfoFromJson>();             // key : 10000, 10001 
    public Dictionary<int, ItemCombinationInfoFromJson> dicItemCombinationInfo = new Dictionary<int, ItemCombinationInfoFromJson>(); // key : 200000, 211000 
    public Dictionary<int, ItemInfoFromJson> dicItemInfo = new Dictionary<int, ItemInfoFromJson>(); // key : 100000, 100001 
    public Dictionary<int, MonsterInfoFromJson> dicMonsterInfo = new Dictionary<int, MonsterInfoFromJson>();        // key : 10000, 10001 



    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(1920, 1080, false);
        Application.runInBackground = true;
        Time.timeScale = timeScale;
		Application.targetFrameRate = 50;

        MonsterSpawnInfoEachTileParseFromJson();
        FieldObjectInfoParseFromJson();
        ItemCombinationInfoParseFromJson();
        ItemInfoParseFromJson();
        MonsterInfoParseFromJson();

        InitializePlayerInventory();
    }

    void Start()
    {

    }



    private void InitializePlayerInventory()
    {
        //바닥 타일 데이터 셋팅. 실제 큐브 객체생성은 하지 않음.

        string jsonText = ReadStringFromFile("playerInventory");
        Debug.Log("InitializeStage() : " + jsonText);

        if (jsonText != null && !isInit)
        {
            playerInventory = JsonFx.Json.JsonReader.Deserialize<PlayerInventory>(jsonText);
        }
        else
        {
            playerInventory = new PlayerInventory();
            playerInventory.dicPlayerInventory = new Dictionary<int, ItemInfo>();

            //카운트 임의로 올려 놓기.
            foreach(var item in dicItemInfo)
            {
                ItemInfo _tempItemInfo = new ItemInfo()
                {
                    itemID = item.Key
                    , itemName = item.Value.name
                    , itemCnt = 10
                };

                playerInventory.dicPlayerInventory.Add(item.Key, _tempItemInfo);
            }
        }

    }


    void Update()
    {
        playerInventory.ToString();

        dicFieldObjectInfo.ToString();
        dicItemCombinationInfo.ToString();
        dicItemInfo.ToString();
        dicMonsterInfo.ToString();
        dicMonsterSpawnInfoEachTile.ToString();
    }

    private void OnApplicationQuit()
    {
        string jsonText = JsonFx.Json.JsonWriter.Serialize(playerInventory);
        Debug.Log("playerInventory : " + jsonText);
        GameInfoManager.instance.WriteStringToFile(jsonText, "playerInventory");
    }


    public void WriteStringToFile(string str, string filename)
    {
        string path = PathForDocumentsFile(filename);
        FileInfo fileInfo = new FileInfo(path);

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (fileInfo.Exists)
            {
                return;
            }
            else
            {
                FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine(str);

                sw.Close();
                file.Close();
            }
        }
        else
        {
            FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(str);

            sw.Close();
            file.Close();
        }


    }


    public string ReadStringFromFile(string filename)
    {
        string path = PathForDocumentsFile(filename);

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadLine();

            sr.Close();
            file.Close();

            return str;
        }

        else
        {
            return null;
        }
    }


    public string PathForDocumentsFile(string filename)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), filename);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, filename);
        }
    }


    void MonsterSpawnInfoEachTileParseFromJson()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("_JSONs/MonsterSpawnInfo");
        JsonData array = JsonMapper.ToObject(textAsset.text);

        for (int i = 0; i < array.Count; i++)
        {
            MonsterSpawnInfoFromJson monsterSpawnInfo = new MonsterSpawnInfoFromJson(array[i]["id"].ToString()
                                                                    , array[i]["gen1"].ToString()
                                                                    , array[i]["gen2"].ToString()
                                                                    , array[i]["gen3"].ToString());

            dicMonsterSpawnInfoEachTile.Add(int.Parse(array[i]["id"].ToString()), monsterSpawnInfo);
        }
    }


    void FieldObjectInfoParseFromJson()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("_JSONs/FieldObjectInfo");
        JsonData array = JsonMapper.ToObject(textAsset.text);

        for (int i = 0; i < array.Count; i++)
        {
            FieldObjectInfoFromJson fieldObjectInfo = new FieldObjectInfoFromJson(int.Parse(array[i]["id"].ToString())
                                                                    , array[i]["name"].ToString()
                                                                    , array[i]["drop"].ToString());

            dicFieldObjectInfo.Add(int.Parse(array[i]["id"].ToString()), fieldObjectInfo);
        }
    }


    void ItemCombinationInfoParseFromJson()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("_JSONs/ItemCombinationInfo");
        JsonData array = JsonMapper.ToObject(textAsset.text);

        for (int i = 0; i < array.Count; i++)
        {
            ItemCombinationInfoFromJson itemCombinationInfo = new ItemCombinationInfoFromJson(int.Parse(array[i]["id"].ToString())
                                                                    , array[i]["name"].ToString()
                                                                    , array[i]["material"].ToString());

            dicItemCombinationInfo.Add(int.Parse(array[i]["id"].ToString()), itemCombinationInfo);
        }
    }


    void ItemInfoParseFromJson()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("_JSONs/ItemInfo");
        JsonData array = JsonMapper.ToObject(textAsset.text);

        for (int i = 0; i < array.Count; i++)
        {
            ItemInfoFromJson itemInfo = new ItemInfoFromJson(int.Parse(array[i]["id"].ToString())
                                                                    , array[i]["name"].ToString()
                                                                    , int.Parse(array[i]["re_hp"].ToString())
                                                                    , int.Parse(array[i]["add_ap"].ToString())
                                                                    , int.Parse(array[i]["add_hp"].ToString())
                                                                    , int.Parse(array[i]["add_sp"].ToString())
                                                                    , int.Parse(array[i]["durability"].ToString()));

            dicItemInfo.Add(int.Parse(array[i]["id"].ToString()), itemInfo);
        }
    }


    void MonsterInfoParseFromJson()
    {
        TextAsset textAsset = (TextAsset)Resources.Load("_JSONs/MonsterInfo");
        JsonData array = JsonMapper.ToObject(textAsset.text);

        for (int i = 0; i < array.Count; i++)
        {
            MonsterInfoFromJson monsterInfoFromJson = new MonsterInfoFromJson(int.Parse(array[i]["id"].ToString())
                                                                    , array[i]["name"].ToString()
                                                                    , int.Parse(array[i]["hp"].ToString())
                                                                    , int.Parse(array[i]["ap"].ToString())
                                                                    , int.Parse(array[i]["sp"].ToString())
                                                                    , array[i]["drop"].ToString());

            dicMonsterInfo.Add(int.Parse(array[i]["id"].ToString()), monsterInfoFromJson);
        }
    }


}



public class FieldObjectInfoFromJson
{
   public  int id;
    public string name;
    public List<int> listDrop = new List<int>();

    public FieldObjectInfoFromJson(int _id, string _name, string _drop)
    {
        id = _id;
        name = _name;

        foreach(string temp in _drop.Split('/'))
        {
            listDrop.Add(int.Parse(temp));
        }
    }
}

public class ItemCombinationInfoFromJson
{
    public int id;
    public string name;
    public List<int> listMaterial = new List<int>();

    public ItemCombinationInfoFromJson(int _id, string _name, string _material)
    {
        id = _id;
        name = _name;

        foreach (string temp in _material.Split('/'))
        {
            listMaterial.Add(int.Parse(temp));
        }
    }
}

public class ItemInfoFromJson
{
    public int id;
    public string name;
    public int re_hp;
    public int add_ap;
    public int add_hp;
    public int add_sp;
    public int durability;

    public ItemInfoFromJson(int _id, string _name, int _re_hp, int _add_ap, int _add_hp, int _add_sp, int _durability)
    {
        id = _id;
        name = _name;
        re_hp = _re_hp;
        add_ap = _add_ap;
        add_hp = _add_hp;
        add_sp = _add_sp;
        durability = _durability;
    }
}

public class MonsterInfoFromJson
{
    public int id;
    public string name;
    public int hp;
    public int ap;
    public int sp;
    public List<int> listDrop = new List<int>();

    public MonsterInfoFromJson(int _id, string _name, int _hp, int _ap, int _sp, string _drop)
    {
        id = _id;
        name = _name;
        hp = _hp;
        ap = _ap;
        sp = _sp;

        foreach (string temp in _drop.Split('/'))
        {
            listDrop.Add(int.Parse(temp));
        }
    }
}

public class MonsterSpawnInfoFromJson
{
    public int id; // 타일ID
    public Dictionary<int, string[]> dicMonsterGenRate = new Dictionary<int, string[]>(); //string[] ID, %

    public MonsterSpawnInfoFromJson(string _id, string _gen1, string _gen2, string _gen3)
    {
        id = int.Parse(_id);

        if (_gen1.Split('/').Length > 1)
        {
            dicMonsterGenRate.Add(1, _gen1.Split('/'));
        }

        if (_gen2.Split('/').Length > 1)
        {
            dicMonsterGenRate.Add(2, _gen2.Split('/'));
        }

        if (_gen3.Split('/').Length > 1)
        {
            dicMonsterGenRate.Add(3, _gen3.Split('/'));
        }
    }

}

public class PlayerInventory
{
    public Dictionary<int, ItemInfo> dicPlayerInventory { get; set; }
}

[System.Serializable]
public class MonsterInfo
{
    public int monsterID { get; set; }
    public string monsterName { get; set; }
    public int positionID { get; set; } //항상 최신화.
    public int layerID { get; set; }    //항상 최신화.
}


public class MonsterInfoInStage
{
    //최대 개체수.
    public Dictionary<string, int> dicMonsterMaxCount { get; set; }     // key : monsterID

    //데이터상의 월드 몬스터.
    public Dictionary<string, List<MonsterInfo>> dicMonsterInfo { get; set; }  // key : monsterID  Value : List<MonsterInfo>

    public MonsterInfoInStage()
    {
        dicMonsterMaxCount = new Dictionary<string, int>();
        dicMonsterInfo = new Dictionary<string, List<MonsterInfo>>();
    }
}


[System.Serializable]
public class ItemInfo
{
    public int itemID { get; set; }
    public string itemName { get; set; }
    public int itemCnt { get; set; }
}

[System.Serializable]
public class ItemOnLandInfo : ItemInfo
{
    public int positionID { get; set; } //항상 최신화.
    public int layerID { get; set; }    //항상 최신화.
}