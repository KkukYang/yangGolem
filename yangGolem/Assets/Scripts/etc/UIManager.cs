using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager s_instance = null;

    public static UIManager instance
    {
        get
        {
            if (null == s_instance)
            {
                s_instance = FindObjectOfType(typeof(UIManager)) as UIManager;
                if (null == s_instance)
                {
                    Debug.Log("UIManager null");
                }
            }
            return s_instance;
        }
    }

    public GameObject tempObj;


    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    void Start()
    {

    }

    void Update()
    {

    }
}
