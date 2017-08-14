using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemElement : MonoBehaviour {

    public int num = -1;
    public bool rayTargetCheck = true;
    private Image imgThis;

	void Start () {
        imgThis = GetComponent<Image>();
	}
	
	void Update () {
        imgThis.raycastTarget = rayTargetCheck;
	}
}
