using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoading : MonoBehaviour {

	private static SceneLoading s_instance = null;
	public static SceneLoading instance
	{
		get
		{
			if (null == s_instance)
			{
				s_instance = FindObjectOfType(typeof(SceneLoading)) as SceneLoading;
				if (null == s_instance)
				{
					////DebugLogCustomize.instance.Log(DebugLogCustomize.LOGTYPE.DB, "Fail to get Manager Instance");
				}
			}
			return s_instance;
		}
	}

	string nextSceneName;
    string preSceneName;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {
    }


	public void ChangeScene (string _sceneName) //sceneName:Ingame, Lobby, Title
    {

        if (_sceneName != null)
        {
            //씬을 바꾸는 로딩.
            nextSceneName = _sceneName;
            preSceneName = SceneManager.GetActiveScene().name;
        }
        else
        {
            //일반 로딩 화면.

        }
    }


	void FadeOutEnd()
	{
		StartCoroutine("GoToNextScene"); 
	}


    IEnumerator GoToNextScene() 
	{
        //인게임 -> 로비 일 경우 기타 오브젝트 제거
        if (nextSceneName == "Lobby")
        {
            ResourceManager.instance.DisableAllObj();
        }

        // 리소스, 가비지콜렉터 정리.
        System.GC.Collect();
        Resources.UnloadUnusedAssets();

        yield return CoroutineManager.instance.GetWaitForSeconds(2.0f);
        AsyncOperation async = SceneManager.LoadSceneAsync(nextSceneName);

        while (!async.isDone)
		{
            yield return true; 
		}
    }

}







