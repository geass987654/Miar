using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPassword : MonoBehaviour
{
    public GameObject[] digits;
    string inputPassword;
    string realPassword;
    public GameObject messageUI;

    [SerializeField] private GameObject[] equipmentPrefab;
    [SerializeField] private GameObject[] essentialPreFab;
    private Vector2 lockBoxPos;
    private string PrefabType;
    [SerializeField] private Image question;

    private void Awake()
    {
        inputPassword = "";
        realPassword = "";
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
                transform.parent.parent.gameObject.SetActive(false);

                for(int i = 0; i < digits.Length; i++)
                {
                    digits[i].GetComponent<DialLock>().Initialize();
                }

                messageUI.GetComponent<Message>().SetMessage("恭喜答對");

                switch (PrefabType)
                {
                    case "equipment":
                        Instantiate(equipmentPrefab[Random.Range(0, 2)], lockBoxPos, Quaternion.identity);
                        break;
                    case "essential":
                        Instantiate(essentialPreFab[Random.Range(0, 2)], lockBoxPos, Quaternion.identity);
                        break;
                }

            }
            else
            {
                messageUI.GetComponent<Message>().SetMessage("密碼錯誤");
            }

            inputPassword = "";
        }
    }

    public void ExitBtn()
    {
        if (!messageUI.GetComponent<Message>().isTextShowing)
        {
            inputPassword = "";
            transform.parent.parent.gameObject.SetActive(false);

            messageUI.SetActive(true);
            messageUI.GetComponent<Message>().SetMessage("再接再勵");
        }
    }

    public void SetPassword(string password)
    {
        realPassword = password;
    }

    public void SetItemPos(Vector2 position)
    {
        lockBoxPos = position;
    }

    public void SetPrefabType(string type)
    {
        PrefabType = type;
    }

    public void SetQuestion(Sprite questionPicture)
    {
        question.sprite = questionPicture;
    }
}
