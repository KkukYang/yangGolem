using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;

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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(1920, 1080, true);

        //userDataInfo = new UserDataInfo();
        //string jsonText = JsonWriter.Serialize(userDataInfo);
        //PlayerPrefs.SetString("jsonText", jsonText);

        //string jsonText = PlayerPrefs.GetString("jsonText");
        //userDataInfo = JsonReader.Deserialize<UserDataInfo>(jsonText);

        //Debug.Log("");
    }

    void Start()
    {

    }

    void Update()
    {

    }



    //IEnumerator LoopForObj(GameObject obj)
    //{
    //    while (obj.activeSelf)
    //    {
    //        Time.timeScale = 0.0f;
    //        yield return null;
    //    }

    //}
}


//public class UserDataInfo
//{
//    public int user_no;
//    public Dictionary<string, string> dicData;
//    public List<string> listData;

//    public UserDataInfo()
//    {
//        user_no = 1000000001;
//        dicData = new Dictionary<string, string>();
//        listData = new List<string>();

//        {
//            for (int i = 0; i < 10; i++)
//            {
//                dicData.Add(i.ToString(), "a" + (i * 123).ToString());
//                listData.Add("a" + (i * 1123).ToString());
//            }
//        }
//    }
//}

