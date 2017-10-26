using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using DG.Tweening;

// 팝업 오브젝트 관리.
public class PopUpManager : MonoBehaviour {

	private static PopUpManager s_instance = null;
	
	public static PopUpManager instance
	{
		get
		{
			if (null == s_instance)
			{
				s_instance = FindObjectOfType(typeof(PopUpManager)) as PopUpManager;
				if (null == s_instance)
				{
					//DebugLogCustomize.instance.Log(DebugLogCustomize.LOGTYPE.DB, "Fail to get Manager Instance");
				}
			}
			return s_instance;
		}
	}

    public List<GameObject> listPopUp = new List<GameObject>();	// 생성된 팝업 리스트.

    //public List<GameObject> listPopUp
    //{
    //    get
    //    {
    //        return popupList;
    //    }
    //}

    float closeMarketPopUpTimer;

    public bool isPerformance = false;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);
        closeMarketPopUpTimer = -1.0f;
    }


	void Update()
    {
        if(closeMarketPopUpTimer > 0.0f)
        {
            closeMarketPopUpTimer -= Time.unscaledDeltaTime;

            if(closeMarketPopUpTimer < 0.0f)
            {
                closeMarketPopUpTimer = -1.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (listPopUp.Find(obj => obj.name == "PopUpInventory") == null)
            {
                GameObject popupInventory = ResourceManager.instance.GetPopUp("PopUpInventory");
                popupInventory.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (listPopUp.Find(obj => obj.name == "PopUpCombination") == null)
            {

                GameObject popUpCombination = ResourceManager.instance.GetPopUp("PopUpCombination");
                popUpCombination.SetActive(true);
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (listPopUp.Find(obj => obj.name == "PopUpTest") == null)
            {
                GameObject popupTest = ResourceManager.instance.GetPopUp("PopUpTest");
                popupTest.GetComponent<PopUpTest>().selectObject = Instantiate(ResourceManager.instance.fieldObj["Tree_2"]) as GameObject;
                popupTest.GetComponent<PopUpTest>().selectObject.name = "Tree_2";
                popupTest.GetComponent<PopUpTest>().selectObject.SetActive(true);

                //OnEnable() 호출전에 셋팅.
                popupTest.SetActive(true);
            }
        }
        //else if (Input.GetKeyDown(KeyCode.K))
        //{
        //    if (popupList.Find(obj => obj.name == "PopUpEquipment") == null)
        //    {
        //        GameObject popupEquipment = ResourceManager.instance.GetPopUp("PopUpEquipment");
        //        popupEquipment.SetActive(true);
        //    }
        //}

    }



    public void StartPopUp(GameObject obj)
    {
        listPopUp.Add(obj);

        // 팝업 오브젝트 위치 설정.
        obj.transform.parent = this.transform;
        obj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f - listPopUp.Count * 0.7f);
        obj.transform.localScale = Vector3.zero;

        // 생성 애니메이션.
        obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true);
    }


    public void EndPopUp(GameObject obj)
    {
        obj = listPopUp.Find(_obj => _obj == obj);
        listPopUp.Remove(obj);

        // 생성 애니메이션.
        //SoundManager.PlayEffect(SoundEffect.FX_Common_Screen_Change_02);
        obj.transform.DOScale(new Vector3(0.2f, 0.2f, 1.0f), 0.3f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true).OnComplete(
            () => obj.SetActive(false)
            );
    }


    public void CloseAllPopUp()
    {
        listPopUp.Clear();
        foreach(Transform tmPopUp in this.transform)
        {
            Destroy(tmPopUp.gameObject);
        }
    }


	//public bool IsInStack(GameObject obj)
	//{
	//	return popupList.Contains(obj);
	//}


	public int GetPopUpCount()
	{
		return listPopUp.Count;
	}
}
