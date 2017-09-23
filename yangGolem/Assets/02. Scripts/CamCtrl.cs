using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrl : MonoBehaviour {

	public float camMoveSpeed = 100;

	private Transform player;

	void Awake()
	{
		player = Player.instance.transform;

	}


	void LateUpdate () {
		Vector3 dis = (player.position - transform.position).normalized;
		dis.y = 0;
		//transform.position = new Vector3(player.position.x,player.position.y+11.54f,-10 +player.position .z);
		transform.Translate(dis*camMoveSpeed *Time.deltaTime);

	}
}
