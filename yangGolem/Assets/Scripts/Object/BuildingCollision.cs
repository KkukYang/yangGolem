using UnityEngine;
using System.Collections;

/// <summary>
/// Building collision.
/// 
/// This script is attached to the buildings.
/// </summary>

public class BuildingCollision : MonoBehaviour {
	
    private bool isCollided = false;
	public bool Collided()
    {
        return isCollided;
    }
    /*
    private BuildManager buildMan = null;

    void Start()
    {
        buildMan = GameObject.Find("BuildMgr").GetComponent<BuildManager>();
    }
	*/
	void OnTriggerStay(Collider coll)
	{
        if (coll.gameObject.CompareTag("Floor"))
        {
            isCollided = false;
        }
        else
        {
            isCollided = true;
        }
	}
}
