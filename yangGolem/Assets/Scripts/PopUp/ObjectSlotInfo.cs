using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//버튼에 박혀져 있다.
public class ObjectSlotInfo : MonoBehaviour
{

    public int count;
    public string objectName;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ShowPopUpTest()
    {
        Debug.Log("ShowPopUpTest()");

        GameObject objPopupTest = ResourceManager.instance.GetPopUp("PopUpTest");
        objPopupTest.GetComponent<PopUpTest>().selectObject = Instantiate(ResourceManager.instance.fieldObj[objectName]) as GameObject;
        objPopupTest.GetComponent<PopUpTest>().selectObject.name = objectName;
        objPopupTest.GetComponent<PopUpTest>().selectObject.SetActive(true);

        //OnEnable() 호출전에 셋팅.
        objPopupTest.SetActive(true);
    }
}
