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

        PopUpManager.instance.StartPopUp(this.gameObject);

        Invoke("SetPopUpInit", 0.1f);
    }

    private void OnDisable()
    {
        //일단 그리드 하위 오브젝트 싹 비우고. 나중에 재활용 하던지 팝업이니 그냥 이렇게 일단.
        for (int i = 0; i < itemSlotGroup.transform.childCount; i++)
        {
            Destroy(itemSlotGroup.transform.GetChild(i).gameObject);
        }

    }

    public void SetPopUpInit()
    {
        listInvenSlot.Clear();

        int idx = 0;
        int rowNum = 0;
        foreach (var item in GameInfoManager.instance.playerInventory.dicPlayerInventory)
        {
            if(idx == 0)
            {
                //한 행 생성.
                GameObject row = Instantiate(ResourceManager.instance.popup["RowSlotGroupInventory"] as GameObject) as GameObject;
                row.name = "RowSlotGroup_" + rowNum++;
                row.transform.parent = itemSlotGroup.transform;
                row.transform.localPosition = Vector3.zero;//new Vector3(0.0f, 0.0f, 0.1f);
                row.transform.localScale = Vector3.one;
                row.SetActive(true);
            }

            if (item.Value.itemCnt > 0)
            {
                itemSlotGroup.transform.Find("RowSlotGroup_" + (rowNum - 1).ToString() + "/ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemName = item.Value.itemName;
                itemSlotGroup.transform.Find("RowSlotGroup_" + (rowNum - 1).ToString() + "/ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemCnt = item.Value.itemCnt;
                itemSlotGroup.transform.Find("RowSlotGroup_" + (rowNum - 1).ToString() + "/ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemID = item.Value.itemID;

                listInvenSlot.Add(itemSlotGroup.transform.Find("RowSlotGroup_" + (rowNum - 1).ToString() + "/ItemSlot_" + idx).GetComponent<InventoryItemSlot>());
                idx++;
            }
            else
            {
                //itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemName = "";
                //itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemCnt = 0;
                //itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().itemID = 0;

                //itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>().UpdateItemInfo();
                //listInvenSlot.Add(itemSlotGroup.transform.Find("ItemSlot_" + idx).GetComponent<InventoryItemSlot>());
            }

            if(idx == 3)
            {
                idx = 0;
            }
        }

        foreach (var slot in listInvenSlot)
        {
            slot.UpdateItemInfo();
        }

        itemSlotGroup.GetComponent<UIGrid>().enabled = true;
    }

    void ExitButtonEvent()
    {
        PopUpManager.instance.EndPopUp(this.gameObject);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ExitButtonEvent();
        }

    }
}
