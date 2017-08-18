using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemSlot : MonoBehaviour
{
    public string popupName;
    public string itemName;
    UISprite itemImage;
    public GameObject cursor = null;


    private void Awake()
    {
        itemImage = this.transform.Find("Image").GetComponent<UISprite>();    
    }


    private void OnEnable()
    {
        
    }


    void Start()
    {

    }

    public void OnPress(bool _flag) //누르면 true 떼면 false.
    {
        Debug.Log("InventoryItemSlot OnPress" + _flag);

        if(_flag)
        {
            if(UIManager.instance.tempObj.transform.Find("ItemCursor") != null)
            {
                cursor = UIManager.instance.tempObj.transform.Find("ItemCursor").gameObject;
            }
            else
            {
                cursor = Instantiate(Resources.Load("Prefabs/ItemCursor")) as GameObject;
            }

            cursor.GetComponent<ItemCursor>().SetUpdateImage(itemName);
            cursor.name = "ItemCursor";
            cursor.transform.parent = UIManager.instance.tempObj.transform;
            cursor.transform.localScale = Vector3.one;
            cursor.SetActive(true);
        }
    }

    public void OnDragEnd()
    {
        Debug.Log("OnDragEnd()");
        UIManager.instance.tempObj.transform.Find("ItemCursor").gameObject.SetActive(false);
    }

    public void OnDrop(GameObject _obj)
    {
        Debug.Log("InventoryItemSlot OnDrop" + _obj.GetComponent<InventoryItemSlot>().itemName);

        itemName = _obj.GetComponent<InventoryItemSlot>().itemName;
        UpdateItemInfo();

        if (_obj != this.gameObject)
        {
            _obj.GetComponent<InventoryItemSlot>().itemName = "";
            _obj.GetComponent<InventoryItemSlot>().UpdateItemInfo();

        }

        _obj.GetComponent<InventoryItemSlot>().cursor.SetActive(false);

    }


    public void UpdateItemInfo()
    {
        if(itemImage == null)
        {
            itemImage = this.transform.Find("Image").GetComponent<UISprite>();
        }

        itemImage.spriteName = itemName;

        if (itemName == "")
        {
            itemImage.enabled = false;
        }
        else
        {
            itemImage.enabled = true;
        }

        itemImage.gameObject.SetActive(true);
    }

    void Update()
    {

    }
}


/// This script should be attached to each camera that's used to draw the objects with
/// UI components on them. This may mean only one camera (main camera or your UI camera),
/// or multiple cameras if you happen to have multiple viewports. Failing to attach this
/// script simply means that objects drawn by this camera won't receive UI notifications:
/// 
/// * OnHover (isOver) is sent when the mouse hovers over a collider or moves away.
/// * OnPress (isDown) is sent when a mouse button gets pressed on the collider.
/// * OnSelect (selected) is sent when a mouse button is first pressed on an object. Repeated presses won't result in an OnSelect(true).
/// * OnClick () is sent when a mouse is pressed and released on the same object.
///   UICamera.currentTouchID tells you which button was clicked.
/// * OnDoubleClick () is sent when the click happens twice within a fourth of a second.
///   UICamera.currentTouchID tells you which button was clicked.
/// 
/// * OnDragStart () is sent to a game object under the touch just before the OnDrag() notifications begin.
/// * OnDrag (delta) is sent to an object that's being dragged.
/// * OnDragOver (draggedObject) is sent to a game object when another object is dragged over its area.
/// * OnDragOut (draggedObject) is sent to a game object when another object is dragged out of its area.
/// * OnDragEnd () is sent to a dragged object when the drag event finishes.
/// 
/// * OnTooltip (show) is sent when the mouse hovers over a collider for some time without moving.
/// * OnScroll (float delta) is sent out when the mouse scroll wheel is moved.
/// * OnNavigate (KeyCode key) is sent when horizontal or vertical navigation axes are moved.
/// * OnPan (Vector2 delta) is sent when when horizontal or vertical panning axes are moved.
/// * OnKey (KeyCode key) is sent when keyboard or controller input is used.