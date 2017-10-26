using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnLand : MonoBehaviour
{


    public ItemOnLandInfo itemOnLandInfo = new ItemOnLandInfo();
    public GeographyCube curCubeUnderItem = new GeographyCube();
    float rayLenth = 1.5f;


    void Start()
    {

    }

    private void OnEnable()
    {
        RaycastHit hitTileCheck;
        int mask = 1 << LayerMask.NameToLayer("Cube");
        Debug.DrawRay(this.transform.position,
            transform.TransformDirection(-Vector3.up * rayLenth),
            Color.red);

        if (Physics.Raycast(this.transform.position
            , -Vector3.up //transform.TransformDirection(-Vector3.up)
            , out hitTileCheck, rayLenth, mask))
        {
            if (hitTileCheck.transform.parent.GetComponent<GeographyCube>() != null)
            {
                curCubeUnderItem = hitTileCheck.transform.parent.GetComponent<GeographyCube>();

                itemOnLandInfo.positionID = curCubeUnderItem.positionID;
                itemOnLandInfo.layerID = curCubeUnderItem.layerID;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name == "ItemCollider")
        {
            //먹어지는거.
            if (Input.GetKeyDown(KeyCode.F))
            {
                //if(GameInfoManager.instance.playerInventory.dicPlayerInventory == null)
                //{
                //    GameInfoManager.instance.playerInventory.dicPlayerInventory = new Dictionary<int, int>();
                //}

                //if(GameInfoManager.instance.playerInventory.dicPlayerInventory.ContainsKey(this.name))
                //{
                //    GameInfoManager.instance.playerInventory.dicPlayerInventory[this.name]++;
                //}
                //else
                //{
                //    GameInfoManager.instance.playerInventory.dicPlayerInventory.Add(this.name, 1);
                //}

                //this.transform.parent = ResourceManager.instance.itemBox.transform;
                //this.gameObject.SetActive(false);

                //if(PopUpManager.instance.listPopUp.Find(obj => obj.name == "PopUpInventory") != null)
                //{
                //    PopUpInventory popup = PopUpManager.instance.listPopUp.Find(obj => obj.name == "PopUpInventory").GetComponent<PopUpInventory>();

                //    popup.SetPopUpInit();
                //}
            }
        }
    }
}
