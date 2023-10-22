using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveButton : MonoBehaviour
{
    public GameObject button;
    public GameObject displayUI;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            button.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            button.SetActive(false);

        }
    }

    private void Update()
    {
        if (button.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            displayUI.SetActive(true);
        }
    }
}
