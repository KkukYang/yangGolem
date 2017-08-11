using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTile : MonoBehaviour
{
    public SelectPoint localPoint;
    public SelectPoint underPoint;
    public bool isEnable = false;
    public GameObject rootSelectTile;

    public Material matGreen;
    public Material matRed;

    private void OnEnable()
    {
        
    }

    void Update()
    {
        if(this.gameObject != rootSelectTile)
        {
            underPoint = localPoint + rootSelectTile.GetComponent<SelectTile>().underPoint;
        }

        if(underPoint._x >= 0 && underPoint._x < CheckLocationByClick.instance.row 
            && underPoint._y >= 0 && underPoint._y < CheckLocationByClick.instance.col)
        {
            isEnable = true;
        }
        else
        {
            isEnable = false;
        }

        if(isEnable)
        {
            //this.GetComponent<MeshRenderer>().material.color = Color.green;
            this.GetComponent<MeshRenderer>().material = matGreen;

        }
        else
        {
            //this.GetComponent<MeshRenderer>().material.color = Color.red;
            this.GetComponent<MeshRenderer>().material = matRed;

        }
    }
}
