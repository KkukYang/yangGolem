using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;
using LitJson;
using System.IO;


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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(1920, 1080, false);
        Application.runInBackground = true;
        Time.timeScale = timeScale;
		Application.targetFrameRate = 60;

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
            playerInventory.dicPlayerInventory = new Dictionary<string, int>();

        }

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


    void Update()
    {
        playerInventory.ToString();
    }

    private void OnApplicationQuit()
    {
        string jsonText = JsonMapper.ToJson(playerInventory);
        Debug.Log("playerInventory : " + jsonText);
        GameInfoManager.instance.WriteStringToFile(jsonText, "playerInventory");
    }
}

public class FieldObjectInfoFromJson
{
    int id { get; set; }
    string name { get; set; }
}

public class PlayerInventory
{
    public Dictionary<string, int> dicPlayerInventory { get; set; }
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
public class ItemOnLandInfo
{
    public int itemID { get; set; }
    public string itemName { get; set; }
    public int positionID { get; set; } //항상 최신화.
    public int layerID { get; set; }    //항상 최신화.
}