using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Interaction_Box : MonoBehaviour
{
    [SerializeField] private GameObject f;

    private void Update()
    {
        if (f.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            //if (InventoryManager.CanInheritFromBox())
            //{
            //    InventoryManager.Inherit();
            //    InventoryManager.RefreshWeapons();
            //    InventoryManager.RefreshEssentials();
            //    Debug.Log("Inherit");
            //}
            //else
            //{
            //    InventoryManager.Store();
            //    Debug.Log("Store");
            //}

            f.SetActive(false);
            enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            f.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            f.SetActive(false);
            enabled = true;
        }
    }
}
