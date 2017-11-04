using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurSelectShape : MonoBehaviour {


	public int[,] shape;// = new int[4,4];
	public int[,] tempShape;// = new int[4,4];

	void Awake()
	{
		shape = new int[4,4];
		tempShape = new int[4,4];
	}

	void Start () 
	{
		//paste to curselectshape.
		for (int i = 0; i < 4; i++) 
		{
			for (int j = 0; j < 4; j++) 
			{
				shape[i,j] = GameInfoManager.instance.shapes [0, i, j];
			}
		}

		Debug.Log (shape);

		//90degree rotation.
		for(int i=0;i<4; i++)
		{
			for(int j=0; j<4; j++)
			{
				tempShape[j,3-i] = shape[i,j];
			}
		}

		Debug.Log (shape);
	}

	public void Init()
	{
		shape = new int[4,4];
		tempShape = new int[4,4];


	}

	void Update () 
	{
		
	}
}
