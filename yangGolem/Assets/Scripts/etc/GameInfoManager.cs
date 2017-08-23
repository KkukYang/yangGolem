using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonFx.Json;
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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        Screen.SetResolution(1920, 1080, false);
        Application.runInBackground = true;
        Time.timeScale = timeScale;
    }

    void Start()
    {

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

    }

}
