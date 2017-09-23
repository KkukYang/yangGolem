using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldObject : MonoBehaviour 
{
    public EnumFieldObject objType;

	//public string objName;

	public int positionID;
    public int layerID;

    public bool isPicked = false;
    public float pickedTimer = 0.0f;

    void Awake()
	{
		//objName = this.name.Replace("(clone)", "");
	}

	void Start () 
	{
		
	}
	

	void Update () 
	{
		//Select Located Object.
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			int mask = 1 << LayerMask.NameToLayer ("FieldObject");

			if (Physics.Raycast (ray, out hit, 100.0f, mask)) {
				if (hit.transform.parent == this.transform) {
					Debug.Log ("Picked");
					isPicked = true;
					pickedTimer = 0.0f;
				}
			}
		} 
		else if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			int mask = 1 << LayerMask.NameToLayer ("FieldObject");

			if (Physics.Raycast (ray, out hit, 100.0f, mask)) 
			{
				if (hit.transform.parent == this.transform) 
				{
					if (isPicked) {
						pickedTimer += Time.deltaTime;
					}

					if (pickedTimer >= 1.0f) {
						Debug.Log ("Picking complete");




//						TileInfoManager.instance.curSelectCube = this.gameObject;
//						//						TileInfoManager.instance.curSelectCube.GetComponent<GeographyCube>().cubeType = EnumCubeType.SlopeUp;
//						isCurSelectFromTileInfo = true;
//						transform.SetChildLayer(LayerMask.NameToLayer("Default"));
//						//						TileInfoManager.instance.curSelectCube.SetActive(true);
//
//						TileInfoManager.instance.stageInfo.arrListCubeInStage[positionID].Remove((int)cubeType);
//						TileInfoManager.instance.listGeoCube.Remove(this);
//
//						{
//							int layerMax = 0;
//							List<GeographyCube> _listCube = TileInfoManager.instance.listGeoCube.FindAll (_cube => _cube.positionID == positionID);
//
//							foreach (var _cube in _listCube) {
//								if (_cube.layerID > layerMax) {
//									layerMax = _cube.layerID;
//								}
//							}
//							GeographyCube cube = TileInfoManager.instance.listGeoCube.Find (_cube => _cube.layerID == layerMax);
//							cube.isExistOnCube = false;
//						}
					}
				}
			}
		} 
		else if (Input.GetMouseButtonUp (0)) 
		{
			isPicked = false;
			pickedTimer = 0.0f;
		}
	}
}
