using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	private Camera UICam;
	bool mPress = false;
	UISprite uiSprite;
	Collider _collider;
	public string itemName;
	public GameObject mySlot = null;
	public int slotNum;
	void Start()
	{
		UICam = GameObject.Find ("UICam").GetComponent<Camera>();
		uiSprite = GetComponent<UISprite> ();
		_collider = GetComponent<Collider> ();
		for (int i = 0; i < UIItemMgr.instance.slotEmpty.Length; i++) {
			if (UIItemMgr.instance.slotEmpty [i] == false) {
				slotNum = i;
				UIItemMgr.instance.slotEmpty [i] = true;
				mySlot = transform.parent.gameObject;
				break;
			}
		}
	}

	void OnPress(bool pressed){
		_collider.enabled = !pressed;
		mPress = pressed;

		if (Input.GetMouseButton (1)) {
			if (PlayerHp.instance.die == false) {
				PlayerHp.instance.hp += 5;
				mPress = false;
				Destroy (gameObject);
			}
		} else {


			if (!pressed) {
				uiSprite.depth = 10;
				GameObject dropped = UICamera.lastHit.collider.gameObject;

				if (dropped.CompareTag ("Slot")) {

					if (mySlot != dropped) {
						UIItemMgr.instance.slotEmpty [slotNum] = false;
						slotNum = dropped.GetComponent<Slot> ().slotNumber;
						UIItemMgr.instance.slotEmpty [slotNum] = true;
						transform.SetParent (dropped.transform);
						transform.localPosition = Vector3.zero;

						mySlot = dropped;
					} else {
						transform.localPosition = Vector3.zero;
					}
					
				} else if (dropped.CompareTag ("Item")) {
					int temp = slotNum;
					slotNum = dropped.GetComponent<Item> ().slotNum;
					dropped.GetComponent<Item> ().slotNum = temp;
					GameObject tempGameObject = mySlot;
					mySlot = dropped.GetComponent<Item> ().mySlot;
					dropped.GetComponent<Item> ().mySlot = tempGameObject;

					Transform myParent = transform.parent;
					transform.SetParent (dropped.transform.parent);
					transform.localPosition = Vector3.zero;
					dropped.transform.SetParent (myParent);
					dropped.transform.localPosition = Vector3.zero;
				} else if (dropped.CompareTag ("Floor")) {
					UIItemMgr.instance.slotEmpty [slotNum] = false;

					GameObject item = Instantiate (Resources.Load ("Items/"+itemName)) as GameObject;
					item.name = itemName;
					Transform player = GameObject.FindWithTag ("Player").transform;
					item.transform.position = player.position + player.forward + new Vector3 (0, 2, 0);
					//new Vector3(Random.Range (-1f, 1f), 3, Random.Range (-1f, 1f));
					item.transform.rotation = Quaternion.Euler (new Vector3 (
						Random.Range (-90f, 90f),
						Random.Range (-90f, 90f),
						Random.Range (-90f, 90f)));
					Destroy (gameObject);

				} else {
					UIItemMgr.instance.slotEmpty [slotNum] = true;
					transform.localPosition = Vector3.zero;
				}
			}
		}
	}
	void OnDragStart(){
		uiSprite.depth=20;
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
/*
	OnHover (isOver) is sent when the mouse hovers over a collider or moves away.
	OnPress (isDown) is sent when a mouse button gets pressed on the collider.
	OnSelect (selected) is sent when a mouse button is first pressed on a game object. Repeated presses on the same object won't result in a new OnSelect.
	OnClick () is sent with the same conditions as OnSelect, with the added check to see if the mouse has not moved much. UICamera.currentTouchID tells you which button was clicked.
	OnDoubleClick () is sent when the click happens twice within a fourth of a second. UICamera.currentTouchID tells you which button was clicked.
	OnDragStart () is sent to a game object under the touch just before the OnDrag() notifications begin.
	OnDrag (delta) is sent to an object that's being dragged.
	OnDragOver (draggedObject) is sent to a game object when another object is dragged over its area.
	OnDragOut (draggedObject) is sent to a game object when another object is dragged out of its area.
	OnDragEnd () is sent to a dragged object when the drag event finishes.
	OnInput (text) is sent when typing (after selecting a collider by clicking on it).
	OnTooltip (show) is sent when the mouse hovers over a collider for some time without moving.
	OnScroll (float delta) is sent out when the mouse scroll wheel is moved.
	sOnKey (KeyCode key) is sent when keyboard or controller input is used.
*/