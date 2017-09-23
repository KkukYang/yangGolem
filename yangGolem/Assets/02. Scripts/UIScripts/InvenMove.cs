using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenMove : MonoBehaviour {

	private Camera UICam;
	bool mPress = false;
	Collider _collider;
	void Start()
	{
		UICam = GameObject.Find ("UICam").GetComponent<Camera>();
		_collider = GetComponent<Collider> ();
	}

	void OnPress(bool pressed){
		_collider.enabled = !pressed;
		mPress = pressed;
	}
	void OnDragStart(){
		mPress = true;
	}
	void OnDrag(Vector2 delta){
		if (mPress == true) {
			Vector3 pos = Input.mousePosition;
			pos.x = Mathf.Clamp01 (pos.x / Screen.width);
			pos.y = Mathf.Clamp01 (pos.y / Screen.height);
			transform.position = UICam.ViewportToWorldPoint (pos);
		}
	}
}
