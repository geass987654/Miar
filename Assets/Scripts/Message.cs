using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private Queue<string> messages = new Queue<string>();
    [SerializeField] private Text messageText;
    private bool isTextShowing= false;
    private float textDelay = 0.05f;
    private float textShowingTime = 2f;


    private void Enable()
    {
        messageText.text = "";
    }

    private void Update()
    {
        if (messages.Count > 0 && !isTextShowing)
        {
            StartCoroutine(ShowMessage());
        }
    }


    IEnumerator ShowMessage()
    {
        isTextShowing = true;
        messageText.text = "";

        string message = messages.Dequeue();
        int i = 0;

        while (i < message.Length)
        {
            messageText.text += message[i];
            i++;
            yield return new WaitForSeconds(textDelay);
        }

        yield return new WaitForSeconds(textShowingTime);
        gameObject.SetActive(false);
        isTextShowing = false;
    }

    public void SetMessage(string s)
    {
        messages.Enqueue(s);
    }
}
