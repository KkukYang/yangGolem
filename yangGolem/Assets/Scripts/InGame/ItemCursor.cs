using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCursor : MonoBehaviour
{

    void Start()
    {

    }

    public void SetUpdateImage(string _name)
    {
        if(_name == "")
        {
            return;
        }

        GetComponent<UISprite>().spriteName = _name;
        GetComponent<UISprite>().enabled = true;
    }

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x = Mathf.Clamp01(mousePos.x / Screen.width);
        mousePos.y = Mathf.Clamp01(mousePos.y / Screen.height);
        this.transform.position = UIManager.instance.transform.Find("Camera").GetComponent<Camera>().ViewportToWorldPoint(mousePos);

        //this.transform.position = new Vector3(mousePos.x, mousePos.y, 0.0f);
    }
}
