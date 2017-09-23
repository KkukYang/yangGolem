using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIItemMgr : MonoBehaviour {

	private static UIItemMgr n_instance = null;
	public static UIItemMgr instance {
		get {
			if (null == n_instance) {
				n_instance = FindObjectOfType (typeof(UIItemMgr)) as UIItemMgr;
				if (null == n_instance) {

				}
			}
			return n_instance;
		}
	}

	private Transform[] arraySlot;
	public bool[] slotEmpty;
	public LayerMask layMask;
	public int slotCount = 6;
	void Start()
	{
		arraySlot = new Transform[slotCount];
		slotEmpty = new bool[arraySlot.Length];
		for (int i = 0; i < arraySlot.Length; i++) {
			arraySlot [i] = GameObject.Find ("Slot_" + i).transform;
			slotEmpty [i] = false;
		}

	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, Mathf.Infinity,layMask)) {
				if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Item")) {
					for (int i = 0; i < arraySlot.Length; i++) {
						if (slotEmpty [i] == false) {
							GameObject item = Instantiate(
								Resources.Load("Items/"+hit.collider.gameObject.name+"Item")) as GameObject;
							item.name = hit.collider.name+"Item";
							item.transform.SetParent (arraySlot [i]);
							item.transform.localPosition = Vector3.zero;
							item.transform.localScale = Vector3.one;
							Destroy(hit.collider.gameObject);
							return;
						}
					}
					hit.collider.gameObject.transform.position += new Vector3 (0, 1, 0);
				}
			}
		}
	}
}
