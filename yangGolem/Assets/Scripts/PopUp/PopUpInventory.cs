using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInventory : MonoBehaviour
{
    public GameObject itemSlotGroup;

    public UIButton exitButton;

    public List<InventoryItemSlot> listInvenSlot = new List<InventoryItemSlot>();

    private void Awake()
    {
        EventDelegate eventButton = new EventDelegate(this, "ExitButtonEvent");
        EventDelegate.Add(exitButton.onClick, eventButton);
    }

    private void OnEnable()
    {
        listInvenSlot.Clear();
        string[] arrStrItemName = new string[3] { "sword_1", "sword_2", "sword_3" };
        int idx = 0;

        foreach(Transform tm in itemSlotGroup.transform)
        {
            tm.GetComponent<InventoryItemSlot>().popupName = this.transform.name;
            tm.GetComponent<InventoryItemSlot>().itemName = (idx < arrStrItemName.Length) ? arrStrItemName[idx] : "";
            tm.GetComponent<InventoryItemSlot>().UpdateItemInfo();
            listInvenSlot.Add(tm.GetComponent<InventoryItemSlot>());
            idx++;
        }

        PopUpManager.instance.StartPopUp(this.gameObject);
    }


    void ExitButtonEvent()
    {
        PopUpManager.instance.EndPopUp(this.gameObject);
    }


    void Update()
    {

    }
}
