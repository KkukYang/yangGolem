﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{

    static private InGameManager s_instance = null;
    static public InGameManager instance
    {
        get
        {
            s_instance = FindObjectOfType(typeof(InGameManager)) as InGameManager;
            return (s_instance != null) ? s_instance : null;
        }
    }

    public GameObject InGameUIRoot;
    //public UIButton testButton;
    public GameObject selectedObjectToPlace;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        foreach (Transform tm in InGameUIRoot.transform.Find("Camera/InGameUI"))
        {
            EventDelegate eventTestButton = new EventDelegate(tm.GetComponent<ObjectSlotInfo>(), "ShowPopUpTest");
            EventDelegate.Add(tm.GetComponent<UIButton>().onClick, eventTestButton);
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
