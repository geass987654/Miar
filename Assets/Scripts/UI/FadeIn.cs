using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    [SerializeField] private GameObject fade_img;

    public GameObject level1;
    public GameObject level2;

    private void Open_()
    {
        if(level1.gameObject.activeSelf == true)
        {
            level2.SetActive(true);
            level1.SetActive(false);
        }
        else
        {
            level1.SetActive(true);
            level2.SetActive(false);
        }
    }

    private void Close_()
    {
        fade_img.SetActive(false);
    }

}
