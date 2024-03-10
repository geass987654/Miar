using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scenechange_fade : MonoBehaviour
{

    [SerializeField] private GameObject fade_image;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        fade_image.SetActive(true);
    }

}
