using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interaction_Lock : MonoBehaviour
{
    [SerializeField] private GameObject f;
    [SerializeField] private GameObject lockUI;
    [SerializeField] private GameObject column_4;
    [SerializeField] private string password;
    [SerializeField] private string type;
    [SerializeField] private Sprite question;

    private void Update()
    {
        if (f.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            lockUI.SetActive(true);

            column_4.GetComponent<CheckPassword>().SetPassword(password);

            column_4.GetComponent<CheckPassword>().SetQuestion(question);

            Vector2 location = GetRandomVector2(0f, 360f) * 3;
            column_4.GetComponent<CheckPassword>().SetItemPos((Vector2)transform.position + location);

            column_4.GetComponent<CheckPassword>().SetPrefabType(type);

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

    private Vector2 GetRandomVector2(float startDegree, float endDegree)
    {
        float angle = Random.Range(startDegree, endDegree);
        float xPos = Mathf.Cos(angle);
        float yPos = Mathf.Sin(angle);

        return new Vector2(xPos, yPos).normalized;
    }
}
