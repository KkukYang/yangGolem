using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RendererMgr : MonoBehaviour {

	private Transform player;
	public GameObject tileCube;
	public int colSize = 30;
	int[,] array;
	GameObject[,] gameArray;
	int x =10,z = -10;
	public int width = 2;
	List<GameObject> tileList = new List<GameObject> ();
	void Start()
	{
		x = width+1;
		z = -width-1;
		player = PlayerAnim.instance.transform;
		array = new int[colSize,colSize];
		gameArray = new GameObject[colSize, colSize];
		for (int i = 0; i < colSize; i++) {
			for (int j = 0; j < colSize; j++) {
				array [i, j] = 0;
			}
		}

	}
	void FixedUpdate()
	{
		Debug.DrawRay(transform.position+new Vector3(0,5,0),
			transform.TransformDirection (-Vector3.up)*10,
			Color.red);
		RaycastHit hit;
		if (Physics.Raycast (transform.position+new Vector3(0,5,0),
			    transform.TransformDirection (-Vector3.up),
			out hit, 10f,~LayerMask.NameToLayer("Player"))) {
			if (hit.collider.CompareTag ("Floor")) {
				if (x == (int)hit.collider.transform.position.x && z == (int)hit.collider.transform.position.z) {
					return;
				}
				for (int i = -width-1; i <= width+1; i++) {
					for (int j = -width-1; j <= width+1; j++) {
						if (i == -width - 1 || i == width + 1 || j == -width - 1 || j == width + 1) {
							Destroy (gameArray [-z + i, x + j]);
							array [-z + i, x + j] = 0;
						} 
					}
				}
				for (int i = -width; i <= width; i++) {
					for (int j = -width; j <= width; j++) {
						if (-(int)hit.collider.transform.position.z + i > 0 &&
							(int)hit.collider.transform.position.x + j > 0) {

							if (array [-(int)hit.collider.transform.position.z + i, 
								    (int)hit.collider.transform.position.x + j] == 0) {
								gameArray [-(int)hit.collider.transform.position.z + i, 
									(int)hit.collider.transform.position.x + j] = Instantiate (tileCube) as GameObject;
								gameArray [-(int)hit.collider.transform.position.z + i, 
									(int)hit.collider.transform.position.x + j].transform.position 
								= new Vector3 (hit.collider.transform.position.x + j, 0,
									hit.collider.transform.position.z - i);
								array [-(int)hit.collider.transform.position.z + i, 
									(int)hit.collider.transform.position.x + j] = 1;
							} 
						}
					}
				}
				x = (int)hit.collider.transform.position.x;
				z = (int)hit.collider.transform.position.z;
			}
		}
		/*
		
		RaycastHit[] hit = new RaycastHit[colSize*colSize];
		for (int i =0; i < colSize*colSize; i++) 
		{
			Debug.DrawRay(transform.position+new Vector3 (-colSize/2 + i%colSize, 3, colSize/2 - Mathf.Floor (i / colSize)),
				transform.TransformDirection (-Vector3.up)*10,
				Color.red);
			
			if (Physics.Raycast (transform.position+new Vector3 (-colSize/2 + i%colSize, 3, colSize/2 - Mathf.Floor (i / colSize)),
				transform.TransformDirection (-Vector3.up),
				    out hit [i], 10f)) 
			{
				if (array [i] == 0) {
					if (hit [i].collider.gameObject.CompareTag ("Floor")) {
						continue;
					} else if (hit [i].collider.gameObject.CompareTag ("Finish")) {
						//Debug.Log (hit [i].collider.name);
						GameObject clone = tile.Find(obj => obj.activeSelf == false);
						clone.SetActive (true);
						tile.Remove (clone);
						clone.transform.position = transform.position + new Vector3 (-colSize / 2 + i % colSize, 0, colSize / 2 - Mathf.Floor (i / colSize));
					}
				} else {
					if (hit [i].collider.gameObject.CompareTag ("Floor")) {
						hit [i].collider.gameObject.SetActive (false);
						tile.Add (hit [i].collider.gameObject);
					}
				}
				
			}

		}
	*/
	}

}
