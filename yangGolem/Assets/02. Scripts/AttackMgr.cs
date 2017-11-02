using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMgr : MonoBehaviour {

	private static AttackMgr n_instance = null;
    public static AttackMgr instance
    {
        get
        {
            if (null == n_instance)
            {
                n_instance = FindObjectOfType(typeof(AttackMgr)) as AttackMgr;
                if (null == n_instance)
                {

                }
            }
            return n_instance;
        }
    }
 	bool isAttack = false;
	public Transform[] attackCollider;
    int count=0;
	private GameObject cam;
    Vector3 tempPos;
    void Awake()
    {
        cam = GameObject.FindWithTag("MainCamera");
        tempPos = cam.transform.localPosition;
    }
	public void AttackAction()
	{
		if(!isAttack)
            StartCoroutine(AttackShake());
	}
	void Update()
	{
		attackCollider[count].localPosition = Vector3.zero;
        attackCollider[count].localRotation = Quaternion.identity;
        attackCollider[count].localScale = Vector3.one;
        count++;
        if(count >=attackCollider.Length) count = 0;
	}
	IEnumerator AttackShake()
    {
		float a = 0.25f;
        isAttack = true;
    	cam.transform.localPosition = tempPos;
       	//cam.transform.localPosition = tempPos + new Vector3(a,a,0);
        //Time.timeScale = 0;
        //yield return new WaitForSecondsRealtime(0.02f);
        //Time.timeScale = 1;
		//isAttack = false;
        //cam.transform.localPosition = tempPos + new Vector3(0,-0.15f,0);
        yield return new WaitForEndOfFrame();
        cam.transform.localPosition = tempPos;
        isAttack = false;
    }
}
