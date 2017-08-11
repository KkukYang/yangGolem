using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMgr : MonoBehaviour {

    public Transform[] slot = new Transform[9];
    public bool[] _slot;
    int slotNum = 0;

    void Start()
    {
        _slot = new bool[slot.Length];
        for (int i = 0; i < _slot.Length; i++)
        {
            _slot[i] = false;
        }
    }
    public bool OnItem(string itemName)
    {
        for (int i = 0; i < _slot.Length; i++)
        {
            if (_slot[i] == false)
            {
                _slot[i] = true;
                slotNum = i;
                break;
            }
            else if (i == _slot.Length - 1)
            {
                return false;
            }
        }
        GameObject itemClone = Instantiate(Resources.Load<GameObject>(itemName)) as GameObject;
        itemClone.name = itemName;
        itemClone.transform.SetParent(slot[slotNum]);
        itemClone.transform.localScale = Vector3.one;
        itemClone.transform.localPosition = Vector3.zero;
        return true;
        //item[TotalData.item].SetActive(true);
    }
}
