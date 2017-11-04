using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InGame : MonoBehaviour 
{
	public int[,] activeArea;// = new int[18, 12];
	public int[,] worldArea;

	public int colWorld; //horizontal length.
	public int rowWorld; //vertical length.

	public int colActive; //activeArea horizontal length.
	public int activeAreaPos; // activeAreaVirtual update.

	public int curSelectShapePosX = 0;
	public int curSelectShapePosY = 0;	//init 0 after line check complete.  add 1 per seconds.

	public GameObject gameArea;

	public GameObject curSelectShape;
	public int[,] shape = new int[4,4];
	public int[,] tempShape = new int[4,4];
	public List<GameObject> listCurBlock = new List<GameObject> ();

	int[,] activeAreaVirtual; //same activeArea. update!!
	//public GameObject testSquare;

	bool IsReachButton = false;

	void Awake()
	{
		WorldMapLoad ();
		worldArea = new int[rowWorld, colWorld];
		activeArea = new int[rowWorld, colActive];
		activeAreaVirtual = new int[rowWorld, colActive];

		activeAreaPos = 0;

		//set world data.
		SetDataWorldArea();

		//paste date to activeArea.
		SetDataActiveArea();

		curSelectShape = Instantiate (Resources.Load("Prefabs/Shape") as GameObject) as GameObject;
		//curSelectShape.GetComponent<CurSelectShape> ().Init ();
		curSelectShape.transform.parent = gameArea.transform;
		GetShape ();
	}

	IEnumerator Start () 
	{

		yield return new WaitForSeconds (1.0f);
		Debug.Log ("GameStart!!");


		StartCoroutine (DownShapePerSecond());
	
	}

	void SetDataWorldArea()
	{
		TextAsset _txtFile = (TextAsset)Resources.Load("_CSVs/aaa") as TextAsset;
		string fileFullPath = _txtFile.text;
		Debug.Log (fileFullPath);

		string[] stringList = fileFullPath.Split('\n');
		rowWorld = stringList.Length;

		int rowPos = 0;
		int colPos = 0;


		foreach (var str in stringList) 
		{
			string[] stringLine = str.Split (',');
			colWorld = stringLine.Length;

			foreach (string str2 in stringLine) 
			{
				worldArea [rowPos, colPos] = int.Parse (str2);

				colPos++;
			}

			colPos = 0;
			rowPos++;
		}

		Debug.Log (rowPos + " " + colPos);
	}


	void WorldMapLoad()
	{
		TextAsset _txtFile = (TextAsset)Resources.Load("_CSVs/aaa") as TextAsset;
		string fileFullPath = _txtFile.text;
		Debug.Log (fileFullPath);

		string[] stringList = fileFullPath.Split('\n');
		rowWorld = stringList.Length;

		int rowPos = 0;
		int colPos = 0;


		foreach (var str in stringList) 
		{
			string[] stringLine = str.Split (',');
			colWorld = stringLine.Length;

			foreach (string str2 in stringLine) 
			{
				if(int.Parse(str2) == 1)
				{
					GameObject block = Instantiate (Resources.Load ("Prefabs/Block") as GameObject) as GameObject;
					block.name = String.Format ("Block_{0}_{1}", rowPos, colPos);
					block.transform.parent = gameArea.transform.Find("ShapeGroup").transform;
					block.transform.localPosition = new Vector3 (colPos * 0.5f, rowPos * 0.5f, 0.0f);
					block.transform.localScale = Vector3.one;
				}

				colPos++;
			}

			colPos = 0;
			rowPos--;
		}
	}

	void SetDataActiveArea()
	{
		
		for (int i = 0; i < rowWorld; i++) 
		{
			for (int j = 0; j < colActive; j++) 
			{
				activeArea [i, j] = worldArea [i, j + activeAreaPos];
				activeAreaVirtual [i, j] = worldArea [i, j + activeAreaPos];
			}
		}

		Debug.Log ("SetDataActiveArea");
	}

	void GetShape()
	{
		int shapeNum = UnityEngine.Random.Range (0, 7);
		curSelectShape.transform.localPosition = new Vector3 (activeAreaPos, 0.0f, 0.0f);
		listCurBlock.Clear ();

		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				Debug.Log (i + " " + j);
				Debug.Log (shapeNum);
				//curSelectShape.GetComponent<CurSelectShape>().
				shape[i, j] = GameInfoManager.instance.shapes [6, i, j];

				//block Load.
				if (shape[i, j] == 1) {
					GameObject block = Instantiate (Resources.Load ("Prefabs/Block") as GameObject) as GameObject;
					block.name = String.Format ("Block_{0}_{1}", i, j);
					block.transform.parent = curSelectShape.transform.Find("BlockGroup").transform;
					block.transform.localPosition = new Vector3 (j * 0.5f, -i * 0.5f, 0.0f);
					block.transform.localScale = Vector3.one;
					block.SetActive (true);
					listCurBlock.Add (block);

				}
			}
		}

		curSelectShape.SetActive (true);
	}

	IEnumerator DownShapePerSecond()
	{
		while (true) 
		{
			if (IsDownMove () && !IsReachButton) 
			{
				//curpos update.
				if (curSelectShapePosY+1 < rowWorld) 
				{
					curSelectShapePosY++;
				}

				//curShape update.
				curSelectShape.transform.localPosition = new Vector3 (curSelectShape.transform.localPosition.x
					, curSelectShape.transform.localPosition.y - 0.5f
					, 0.0f);
				
			} 
			else 
			{
				//line check.
				Debug.Log("Buttom");
				IsReachButton = true;

				//insert data to world.
				for (int i = 0; i < 4; i++) 
				{
					for (int j = 0; j < 4; j++) 
					{
						if (shape [i, j] == 1) 
						{
							worldArea [curSelectShapePosY + i, curSelectShapePosX + j] = shape [i, j];
						}
					}
				}
			}

			if (IsReachButton) 
			{
				IsReachButton = false;
				curSelectShapePosY = 0;
				curSelectShapePosX = 0;

				GetShape ();
			}


			yield return new WaitForSeconds(1.0f);
		}


	}


	void RotationCurSelectShape()
	{
		//90degree rotation.
		for(int i=0;i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				tempShape[j,3-i] = shape[i,j];

//				if (curSelectShape.transform.Find ("BlockGroup/Block_" + i + "_" + j) != null) {
//					Transform tmBlock = curSelectShape.transform.Find ("BlockGroup/Block_" + i + "_" + j);
//					tmBlock.name = String.Format ("_Block_{0}_{1}", 3-i, j);
//					tmBlock.localPosition = new Vector3 (j * 0.5f, (3-i) * 0.5f, 0.0f);
//				}



			}
		}

		int idx = 0;

		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				Debug.Log (i + " " + j);
				shape [i, j] = tempShape [i, j];

//				if (curSelectShape.transform.Find ("BlockGroup/_Block_" + i + "_" + j) != null) {
//					Transform tmBlock = curSelectShape.transform.Find ("BlockGroup/_Block_" + i + "_" + j);
//
//					//block update Position.
//					//tmBlock.localPosition = new Vector3 (j * 0.5f, -i * 0.5f, 0.0f);
//					tmBlock.name = String.Format ("Block_{0}_{1}", i, j);
//				}
				if (shape[i, j] == 1) {
					listCurBlock[idx].name = String.Format ("Block_{0}_{1}", i, j);
					//listCurBlock[idx].transform.parent = curSelectShape.transform.Find("BlockGroup").transform;
					listCurBlock[idx].transform.localPosition = new Vector3 (j * 0.5f, -i * 0.5f, 0.0f);
					//listCurBlock[idx].transform.localScale = Vector3.one;
					idx++;
				}
			}
		}
	}
	

	void Update () 
	{
		//move after check.!!
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (IsRotationPossible ()) 
			{
				//rotation curSelectShape.
				RotationCurSelectShape();
			}
		}
		else if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			if (IsDownMove () && !IsReachButton) 
			{
				if (curSelectShapePosY +1 < rowWorld) 
				{
					curSelectShapePosY++;
				}
				curSelectShape.transform.localPosition = new Vector3 (curSelectShape.transform.localPosition.x
					, curSelectShape.transform.localPosition.y - 0.5f
					, 0.0f);
				
			}
		}
		else if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			if (IsLeftMove ()) 
			{
				if (curSelectShapePosX-1 >= 0) 
				{
					curSelectShapePosX--;
				}

				curSelectShape.transform.localPosition = new Vector3 (curSelectShape.transform.localPosition.x - 0.5f
					, curSelectShape.transform.localPosition.y
					, 0.0f);
			}
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			if (IsRightMove ()) 
			{
				if (curSelectShapePosX+1 < colActive) 
				{
					curSelectShapePosX++;
				}

				curSelectShape.transform.localPosition = new Vector3 (curSelectShape.transform.localPosition.x + 0.5f
					, curSelectShape.transform.localPosition.y
					, 0.0f);
				
			}
		}

	}

	bool IsRotationPossible()
	{
		//90degree rotation.
		for(int i=0;i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				tempShape[j,3-i] = shape[i,j];

			}
		}

		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				if (tempShape [i, j] == 1) 
				{
					if (curSelectShapePosX + j < colActive && curSelectShapePosY + i < colActive
						&& curSelectShapePosX + j >= 0 && curSelectShapePosY + i >= 0) //wall check.
					{
						if (activeArea [curSelectShapePosY + i, curSelectShapePosX + j] == 1) 
						{
							return false;
						}
					} 
					else 
					{
						return false;
					}
				}
			}
		}

		return true;
	}

	bool IsRightMove()
	{
		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				if (shape [i, j] == 1) 
				{
					if (curSelectShapePosX + j +1 < colActive && curSelectShapePosY + i < colActive) //wall check.
					{
						if (activeArea [curSelectShapePosY + i, curSelectShapePosX + j +1] == 1) 
						{
							return false;
						}
					} 
					else 
					{
						return false;
					}
				}
			}
		}

		return true;
	}

	bool IsLeftMove()
	{
		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				if (shape [i, j] == 1) 
				{
					if (curSelectShapePosX + j -1 >= 0 && curSelectShapePosY + i >= 0) //wall check.
					{
						if (activeArea [curSelectShapePosY + i, curSelectShapePosX + j -1] == 1) 
						{
							return false;
						}
					} 
					else 
					{
						return false;
					}
				}
			}
		}

		return true;
	}

	bool IsDownMove()
	{
		//int maxY = 0;
		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				if (shape [i, j] == 1) 
				{
					if (curSelectShapePosX + j >= 0 && curSelectShapePosY + i +1 >= 0
						&& curSelectShapePosY + i + 1 < rowWorld) //wall check.
					{
						if (activeArea [curSelectShapePosY + i +1, curSelectShapePosX + j] == 1) 
						{
							return false;
						}
					} 
					else 
					{
						return false;
					}
				}
			}
		}


		return true;
	}
}
