using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class map_controller : MonoBehaviour
{
    [SerializeField] private Canvas Map_parent;
    [SerializeField] private Canvas map_1;
    [SerializeField] private Canvas map_2;
    [SerializeField] private GameObject level_1;
    [SerializeField] private GameObject level_2;


    // Update is called once per frame
    void Update()
    {
        if(Map_parent.enabled == true)
        {
            if(level_1.activeSelf == true)
            {
                map_1.enabled = true;
                map_2.enabled =false;
            }
            else if(level_2.activeSelf == true)
            {
                map_2.enabled = true;
                map_1.enabled =false;
            }
        }        
    }
}
