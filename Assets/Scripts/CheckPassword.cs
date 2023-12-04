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
                messageUI.GetComponent<Message>().SetMessage("居然讓你答對了!");
            }
            else
            {
                messageUI.GetComponent<Message>().SetMessage("蛤?四位數密碼也不會喔?這麼肉咖!");
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
            messageUI.GetComponent<Message>().SetMessage("提示:'唐伯虎'的終身代號");
        }
    }
}
