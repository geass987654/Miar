using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPassword : MonoBehaviour
{
    public GameObject[] digits;
    string inputPassword;
    string realPassword;
    public GameObject messageUI;

    private void Start()
    {
        inputPassword = "";
        realPassword = "9527";
    }

    public void CheckBtn()
    {
        if (!messageUI.GetComponent<Message>().isTextShowing)
        {
            for(int i = 0; i < digits.Length; i++)
            {
                inputPassword += digits[i].GetComponent<DialLock>().num.ToString();
            }

            messageUI.SetActive(true);

            if (inputPassword == realPassword)
            {
                transform.parent.gameObject.SetActive(false);
                messageUI.GetComponent<Message>().SetMessage("�~�M���A����F!");
            }
            else
            {
                messageUI.GetComponent<Message>().SetMessage("��?�|��ƱK�X�]���|��?�o��ש@!");
            }

            inputPassword = "";
        }
    }

    public void ExitBtn()
    {
        if (!messageUI.GetComponent<Message>().isTextShowing)
        {
            inputPassword = "";
            transform.parent.gameObject.SetActive(false);

            messageUI.SetActive(true);
            messageUI.GetComponent<Message>().SetMessage("����:'��B��'���ר��N��");
        }
    }
}
