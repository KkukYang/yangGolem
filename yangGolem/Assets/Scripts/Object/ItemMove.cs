using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMove : MonoBehaviour, IDragHandler, IPointerDownHandler, IEndDragHandler
{
    Image dragItem;
    public Transform nowParent;
    public string itemName;
    private ItemMgr _itemMgr;
    int slotNum;
    void Start()
    {
        dragItem = GetComponent<Image>();
        nowParent = transform.parent.transform;
        _itemMgr = GameObject.Find("ItemMgr").GetComponent<ItemMgr>();
        slotNum = transform.parent.GetComponent<SlotElement>().num;
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            dragItem.raycastTarget = false;
            transform.SetParent(GameObject.Find("dragItemList").transform);
            dragItem.rectTransform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        }
    }
    public void OnPointerDown(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {

        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            _itemMgr._slot[slotNum] = false;
            Transform newSlot = null;
            if (data.pointerEnter != null)
                newSlot = data.pointerEnter.transform;
            if (newSlot != null)
            {
                //item position change
                if (newSlot.gameObject.CompareTag("Item"))
                {
                    transform.SetParent(data.pointerEnter.transform.parent.transform);
                    transform.localPosition = Vector3.zero;
                    data.pointerEnter.transform.SetParent(nowParent);
                    data.pointerEnter.transform.localPosition = Vector3.zero;
                    data.pointerEnter.GetComponent<ItemMove>().nowParent = nowParent;
                    nowParent = transform.parent.transform;
                }
                else if (newSlot.gameObject.CompareTag("Slot"))
                {
                    transform.SetParent(data.pointerEnter.transform);
                    transform.localPosition = Vector3.zero;
                    nowParent = transform.parent.transform;
                }
                else
                {
                    transform.SetParent(nowParent);
                    transform.localPosition = Vector3.zero;
                }
                slotNum = transform.parent.GetComponent<SlotElement>().num;
                _itemMgr._slot[slotNum] = true;
            }
            else
            {
                //drop
                GameObject itemClone = Instantiate(Resources.Load<GameObject>(itemName)) as GameObject;
                itemClone.transform.position = Vector3.zero;
                DestroyImmediate(gameObject);
                
            }

            //Debug.Log(data.pointerEnter.name);
            dragItem.raycastTarget = true;
        }
        //transform.SetParent(GameObject.Find("space1").transform);
        //transform.position = GameObject.Find("space1").transform.position;
    }
}
