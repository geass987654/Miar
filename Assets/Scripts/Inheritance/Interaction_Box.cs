using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Interaction_Box : MonoBehaviour
{
    [SerializeField] private GameObject f;
    [SerializeField] private GameObject inheritanceBox;

    private void Update()
    {
        if (f.activeSelf && Input.GetKeyDown(KeyCode.F))
        {

            inheritanceBox.SetActive(true);

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
