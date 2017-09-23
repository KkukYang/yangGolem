 using UnityEngine;
using System.Collections;

// 애니메이션에 이벤트를 정의하기의해 애니메이션이 있는 오브젝트에 넣는 스크립트. 
public class AnimationEvent : MonoBehaviour {

	public delegate void Add ();
	public Add add = null;
	public delegate void End (GameObject obj);
	public End end = null;

	void EndEvent()
	{
		if(end != null)
			end(this.gameObject);
	}

	void AddEvent()
	{
		if(add != null)
			add();
	}

}
