using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Interaction_NPC : MonoBehaviour
{
    [SerializeField] private GameObject f;
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextAsset textFile;

    private void Update()
    {
        if (f.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            dialogueUI.GetComponent<DialogueSystem>().GetTextFromFile(textFile);
            dialogueUI.SetActive(true);

            f.SetActive(false);
            enabled = false;

            if (gameObject.tag == "Scroll")
            {
                Destroy(gameObject);
            }
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
