using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpEquipment : MonoBehaviour
{

    public UIButton exitButton;

    private void Awake()
    {
        EventDelegate eventButton = new EventDelegate(this, "ExitButtonEvent");
        EventDelegate.Add(exitButton.onClick, eventButton);
    }

    private void OnEnable()
    {
        PopUpManager.instance.StartPopUp(this.gameObject);
    }


    void ExitButtonEvent()
    {
        PopUpManager.instance.EndPopUp(this.gameObject);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
