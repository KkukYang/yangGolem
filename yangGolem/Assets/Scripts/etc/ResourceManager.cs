 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// 게임에서 사용되는 각종 리소스 관리.
public class ResourceManager : MonoBehaviour {
	
	private static ResourceManager s_instance = null;
	
	public static ResourceManager instance
	{
		get
		{
			if (null == s_instance)
			{
				s_instance = FindObjectOfType(typeof(ResourceManager)) as ResourceManager;
				if (null == s_instance)
				{
					////DebugLogCustomize.instance.Log(DebugLogCustomize.LOGTYPE.DB, "Fail to get Manager Instance");
				}
			}
			return s_instance;
		}
	}
	
	public Dictionary<string, GameObject> hero = new Dictionary<string, GameObject>();		
	//public Dictionary<string, GameObject> bullet = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> enemy = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> skill = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> effect = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> item = new Dictionary<string, GameObject>();
	public Dictionary<string, GameObject> popup = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> fieldObj = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> floorTile = new Dictionary<string, GameObject>();

    public Queue<GameObject> scoreTextQueue = new Queue<GameObject>();
    public Queue<GameObject> damageTextQueue = new Queue<GameObject>();

    GameObject itemBox;		// 아이템들을 자식으로갖는 오브젝트.
	public Dictionary<string, Queue<GameObject>> itemDic = new Dictionary<string, Queue<GameObject>>();				// 아이템 오브젝트를 재활용하기위해 저장하는 딕셔너리.
	
	GameObject effectBox;
	public Dictionary<string, Queue<GameObject>> effectDic = new Dictionary<string, Queue<GameObject>>();

    //public Dictionary<string, Queue<GameObject>> bulletDic = new Dictionary<string, Queue<GameObject>>();
    //public Dictionary<string, Queue<GameObject>> bulletSampleDic = new Dictionary<string, Queue<GameObject>>();
    //public Dictionary<string, Queue<GameObject>> enemyBulletDic = new Dictionary<string, Queue<GameObject>>();	// 적 기체의 발사체들을 재활용하기위해 쌓는 딕셔너리.

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		itemBox = transform.Find("ItemBox").gameObject;
		effectBox = transform.Find("EffectBox").gameObject;

		// 영웅 셋팅.
		object[] heroObj = Resources.LoadAll("Prefabs/Hero");
		for(int i = 0; i < heroObj.Length; i++)
		{
			GameObject obj = heroObj[i] as GameObject;
			hero.Add(obj.name, obj);
		}

		// 적 셋팅.
		object[] enemyObj = Resources.LoadAll("Prefabs/Enemy");
		for(int i = 0; i < enemyObj.Length; i++)
		{
			GameObject obj = enemyObj[i] as GameObject;
            obj.SetActive(false);
            enemy.Add(obj.name, obj);
		}
		
		// 스킬 셋팅.
		object[] skillObj = Resources.LoadAll("Prefabs/Skill");
		for(int i = 0; i < skillObj.Length; i++)
		{
			GameObject obj = skillObj[i] as GameObject;
			skill.Add(obj.name, obj);
		}
		
		// 이펙트 셋팅.
		object[] effObj = Resources.LoadAll("Prefabs/Effect");
		for(int i = 0; i < effObj.Length; i++)
		{
			GameObject obj = effObj[i] as GameObject;
            obj.SetActive(false);
			effect.Add(obj.name, obj);
		}

		// 아이템 셋팅.
		object[] itemObj = Resources.LoadAll("Prefabs/Item");
		for(int i = 0; i < itemObj.Length; i++)
		{
			GameObject obj = itemObj[i] as GameObject;
            obj.SetActive(false);
			item.Add(obj.name, obj);
		}

        // 필드오브젝트 셋팅.
        object[] arrfieldObj = Resources.LoadAll("Prefabs/FieldObj");
        for (int i = 0; i < arrfieldObj.Length; i++)
        {
            GameObject obj = arrfieldObj[i] as GameObject;
            obj.SetActive(false);
            fieldObj.Add(obj.name, obj);
        }

        // 바닥타일 셋팅.
        object[] floorTileObj = Resources.LoadAll("Prefabs/FloorTile");
        for (int i = 0; i < floorTileObj.Length; i++)
        {
            GameObject obj = floorTileObj[i] as GameObject;
            obj.SetActive(false);
            floorTile.Add(obj.name, obj);
        }


        // 팝업 셋팅.
        object[] PopUpObj = Resources.LoadAll("Prefabs/PopUp");
		for(int i = 0; i < PopUpObj.Length; i++)
		{
			GameObject obj = PopUpObj[i] as GameObject;
			popup.Add(obj.name, obj);
		}
	}

    public GameObject GetPopUp(string name)
    {
        if (PopUpManager.instance.transform.Find(name) != null)
        {
            if (PopUpManager.instance.transform.Find(name).gameObject.activeSelf
                && name != "GrayBack")
            {
                GameObject obj = Instantiate(popup[name]) as GameObject;
                obj.name = name;
                obj.transform.parent = PopUpManager.instance.transform;
                return obj;
            }
            else
            {
                return PopUpManager.instance.transform.Find(name).gameObject;
            }
        }
        else
        {
            GameObject obj = Instantiate(popup[name]) as GameObject;
            obj.name = name;
            obj.transform.parent = PopUpManager.instance.transform;
            return obj;
        }
    }

	public GameObject GetScoreText()
	{
		GameObject targetObj = null;

		if(scoreTextQueue.Count > 0)
			targetObj = scoreTextQueue.Dequeue();
		else
		{
			targetObj = Instantiate(effect["ScoreText"]) as GameObject;
			targetObj.transform.parent = effectBox.transform;
			targetObj.GetComponent<ScoreText>().targetQueue = scoreTextQueue;
		}
	
		return targetObj;
	}

    public GameObject GetDamageText()
    {
        GameObject targetObj = null;

        if (damageTextQueue.Count > 0)
            targetObj = damageTextQueue.Dequeue();
        else
        {
            targetObj = Instantiate(effect["DamageText"]) as GameObject;
            targetObj.transform.parent = effectBox.transform;
            targetObj.GetComponent<DamageText>().targetQueue = damageTextQueue;
        }

        return targetObj;
    }


    public GameObject CreateEffectObj(string name, Vector3 pos, float playTime = 0.0f)
	{
		GameObject targetObj = null;
		Queue<GameObject> targetQueue = new Queue<GameObject>();

		if(effectDic.ContainsKey(name))
			targetQueue = effectDic[name];
		else
			effectDic.Add(name, targetQueue);

        if (targetQueue.Count > 0)
        {
            targetObj = targetQueue.Dequeue();
            targetObj.transform.parent = effectBox.transform;
            targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
            targetObj.GetComponent<GameEffect>().playTime = playTime;
        }
        else
		{
			targetObj = Instantiate(effect[name]) as GameObject;
			targetObj.transform.parent = effectBox.transform;
			targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
			targetObj.GetComponent<GameEffect>().playTime = playTime;
		}

		targetObj.transform.position = pos;

		return targetObj;
	}


    public void DisableAllObj()
    {
        // 리소스, 가비지콜렉터 정리.
        //System.GC.Collect();
        //Resources.UnloadUnusedAssets();

        //InGame Active off.
        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            if(GameObject.Find("GamePatternManager") != null)
            {
                GameObject.Find("GamePatternManager").SetActive(false);
            }

            foreach (Transform tmObj in GameObject.Find("RoomArray").transform)
            {
                tmObj.gameObject.SetActive(false);
            }
        }

        //ObjectPool Clear
        itemDic.Clear();
        effectDic.Clear();
        //bulletDic.Clear();
        //bulletSampleDic.Clear();
        //enemyBulletDic.Clear();
        scoreTextQueue.Clear();
        damageTextQueue.Clear();

        //each child set off in ResourceManager
        foreach (Transform tmObj in transform.Find("EnemyBox"))
        {
            Destroy(tmObj.gameObject);
        }

        foreach (Transform tmObj in transform.Find("ItemBox"))
        {
            Destroy(tmObj.gameObject);
        }

        foreach (Transform tmObj in transform.Find("EffectBox"))
        {
            Destroy(tmObj.gameObject);
        }

        foreach (Transform tmObj in transform.Find("EtcBox"))
        {
            Destroy(tmObj.gameObject);
        }

    }

    //public void SetLayerChange(GameObject _obj, string _layerName)
    //{
    //    foreach(Transform tm in _obj.transform)
    //    {
    //        tm.lay
    //    }
    //}

}
