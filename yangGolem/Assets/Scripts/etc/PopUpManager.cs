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

	List<GameObject> popupList = new List<GameObject>();	// 생성된 팝업 리스트.

    public List<GameObject> listPopUp
    {
        get
        {
            return popupList;
        }
    }

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
	}



    public void StartPopUp(GameObject obj)
    {
        popupList.Add(obj);

        // 팝업 오브젝트 위치 설정.
        obj.transform.parent = this.transform;
        obj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f - popupList.Count * 0.7f);
        obj.transform.localScale = Vector3.zero;

        // 생성 애니메이션.
        obj.transform.DOScale(new Vector3(1.0f, 1.0f, 1.0f), 0.2f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true);
    }


    public void EndPopUp(GameObject obj)
    {
        obj = popupList.Find(_obj => _obj == obj);
        popupList.Remove(obj);

        // 생성 애니메이션.
        //SoundManager.PlayEffect(SoundEffect.FX_Common_Screen_Change_02);
        obj.transform.DOScale(new Vector3(0.2f, 0.2f, 1.0f), 0.3f).SetEase(Ease.OutQuad).SetUpdate(UpdateType.Normal, true).OnComplete(() => obj.SetActive(false));
    }


    public void CloseAllPopUp()
    {
        popupList.Clear();
        foreach(Transform tmPopUp in this.transform)
        {
            Destroy(tmPopUp.gameObject);
        }
    }


	public bool IsInStack(GameObject obj)
	{
		return popupList.Contains(obj);
	}


	public int GetPopUpCount()
	{
		return popupList.Count;
	}
}
