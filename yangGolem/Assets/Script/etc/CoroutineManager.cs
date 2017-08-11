using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// 코루틴 함수 내부의 WaitForSecond를 관리.
public class CoroutineManager : MonoBehaviour {

	private static CoroutineManager s_instance = null;
	
	public static CoroutineManager instance
	{
		get
		{
			if (null == s_instance)
			{
				s_instance = FindObjectOfType(typeof(CoroutineManager)) as CoroutineManager;
				if (null == s_instance)
				{
					////DebugLogCustomize.instance.Log(DebugLogCustomize.LOGTYPE.DB, "Fail to get Manager Instance");
				}
			}
			return s_instance;
		}
	}

	Dictionary<float, WaitForSeconds> waitTimeDic = new Dictionary<float, WaitForSeconds>();

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void SetWaitForSeconds(float second)
	{
		if(!waitTimeDic.ContainsKey(second))
		{
			WaitForSeconds ws = new WaitForSeconds(second);
			waitTimeDic.Add(second, ws);
		}
	}

	public WaitForSeconds GetWaitForSeconds(float second)
	{
		if(!waitTimeDic.ContainsKey(second))
			SetWaitForSeconds(second);

		return waitTimeDic[second];
	}
}
