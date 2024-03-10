using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainController : MonoBehaviour
{
    public static UIMainController _ins;
    
    private GameObject[] allPages;
    

    void Awake()
    {
        _ins = this;
        allPages = GameObject.FindGameObjectsWithTag("UI_Pages");
    }

    void Start()
    {
        switchUIPage("gaming");
    }


    public void switchUIPage(string pageName)
    {
        foreach (GameObject page in allPages)
        {
            if(pageName == page.name)
            {
                page.GetComponent<Canvas>().enabled = true;
            } else
            {
                page.GetComponent<Canvas>().enabled = false;

            }
        }
        
    }
}
