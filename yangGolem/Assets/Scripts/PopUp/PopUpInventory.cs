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
        foreach (Transform tm in itemSlotGroup.transform)
        {
            tm.Find("Image").gameObject.SetActive(false);
            tm.Find("Label").gameObject.SetActive(false);
        }


        PopUpManager.instance.StartPopUp(this.gameObject);

        Invoke("SetPopUpInit", 0.1f);
    }


    public void SetPopUpInit()
    {
        foreach(Transform tm in itemSlotGroup.transform)
        {
            tm.Find("Image").gameObject.SetActive(false);
            tm.Find("Label").gameObject.SetActive(false);
        }

        listInvenSlot.Clear();

        int idx = 0;

        foreach (var item in GameInfoManager.instance.playerInventory.dicPlayerInventory)
        {
            if(itemSlotGroup.transform.Find("ItemSlot_" + idx) == null)
            {
                break;
            }

            if (item.Value.itemCnt <= 0)
            {
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemName = "";
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemCnt = 0;
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemID = 0;

                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().UpdateItemInfo();
                listInvenSlot.Add(itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>());
            }
            else
            {
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemName = item.Value.itemName;
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemCnt = item.Value.itemCnt;
                itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemID = item.Value.itemID;

                listInvenSlot.Add(itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>());
                idx++;
            }
        }

        foreach (var slot in listInvenSlot)
        {
            slot.UpdateItemInfo();
        }
    }

    void ExitButtonEvent()
    {
        PopUpManager.instance.EndPopUp(this.gameObject);
    }


    void Update()
    {

    }
}
