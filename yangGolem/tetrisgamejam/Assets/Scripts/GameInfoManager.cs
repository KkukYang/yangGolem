using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoManager : MonoBehaviour 
{
	private static GameInfoManager s_instance = null;
	public static GameInfoManager instance
	{
		get
		{
			s_instance = FindObjectOfType(typeof(GameInfoManager)) as GameInfoManager;
			return (s_instance != null) ? s_instance : null;
		}
	}

	public int[, ,] shapes = new int[,,] { 
		{ 
			//#
			//###
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{1, 0, 0, 0},
			{1, 1, 1, 0}
		}, 
		{ 
			//  #
			//###
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 1},
			{0, 1, 1, 1}
		}, 
		{ 
			//##
			//##
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{1, 1, 0, 0},
			{1, 1, 0, 0}
		}, 
		{ 
			//####
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{1, 1, 1, 1}
		}, 
		{ 
			// ##
			//##
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 1, 1, 0},
			{1, 1, 0, 0}
		}, 
		{ 
			//##
			// ##
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{1, 1, 0, 0},
			{0, 1, 1, 0}
		}, 
		{ 
			// #
			//###
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 1, 0, 0},
			{1, 1, 1, 0}
		} 
	};


	void Start () 
	{
		
	}
	

	void Update () 
	{
		
	}
}
