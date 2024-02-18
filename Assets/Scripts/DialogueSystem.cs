using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI元件")]
    public Text textContent;  //對話框中顯示的文字內容
    public Image faceImage; //正在說話的角色頭像

    [Header("文本")]
    public TextAsset textFile;  //文字檔案
    public int textIndex;       //文字在第幾行，從0開始
    public float textDelay = 0.05f;     //文字輸出時，每個字的延遲時間

    [Header("頭像")]
    public Sprite facePlayer;
    public Sprite faceSensei;
    public Sprite faceSalesman;
    public Sprite faceUnknown;   //代表主角、師傅、商店、未知

    public bool isTextFinished, isTextCanceled; //文字是否正在輸出，文字是否取消逐字輸出

    public List<string> textList = new List<string>();  //儲存每一行文字

    private void OnEnable()
    {
        isTextFinished = true;
        StartCoroutine(SetTextUI());
    }

    private void Update()
    {
        //按下'F'進行互動，當所有文字輸出完畢，關閉對話框
        if (Input.GetKeyDown(KeyCode.F) && textIndex == textList.Count)
        {
            gameObject.SetActive(false);
            textIndex = 0;
            return;
        }

        /*
            預設按下'F'，輸出整篇文字 : 
            正在輸出文字且沒有取消輸出時，使用Coroutine逐字輸出；
            正在輸出文字且取消輸出時，將文字整篇直接輸出
            沒有輸出文字時，不做處理
        */

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isTextFinished && !isTextCanceled)
            {
                StartCoroutine(SetTextUI());
            }
            else if (!isTextFinished)
            {
                isTextCanceled = !isTextCanceled;
            }
        }
    }

    public void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        textIndex = 0;

        //根據不同的系統，將原本的文字檔案，以換行符號分割成字串，陣列的每個元素是一行字串
        string[] lines = file.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        //將字串陣列中的每一行字串，加入到列表中，以儲存每一行文字
        foreach (string line in lines)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        isTextFinished = false; //文字正在輸出
        textContent.text = "";    //清空對話框

        //根據不同的角色切換頭像，並跳到下一行文字
        switch (textList[textIndex])
        {
            case "Player":
                //Debug.Log(textList[textIndex]);
                faceImage.sprite = facePlayer;
                textIndex++;
                break;
            case "Sensei":
                //Debug.Log(textList[textIndex]);
                faceImage.sprite = faceSensei;
                textIndex++;
                break;
            case "Salesman":
                faceImage.sprite = faceSalesman;
                textIndex++;
                break;
            default:
                faceImage.sprite = faceUnknown;
                textIndex++;
                break;
        }

        /*
            i代表每一行的第幾個字元，從0開始，一直到該行字串長度-1；textIndex代表第幾行
            將該行的第i個字元，逐字增加到對話框中，並等待一定的時間，預設為0.05秒；
            取消文字逐字輸出時，將整行文字增加到對話框
        */
        int i = 0;
        while (!isTextCanceled && i < textList[textIndex].Length)
        {
            textContent.text += textList[textIndex][i];
            i++;
            yield return new WaitForSeconds(textDelay);
        }
        textContent.text = textList[textIndex];

        isTextCanceled = false; //文字沒有取消輸出
        isTextFinished = true;  //文字輸出完畢
        textIndex++;            //遞增到下一行對話
    }

    //public void SetText(string text)
    //{
    //    textList.Clear();
    //    textIndex = 0;

    //    string[] lines = text.Split("\n");

    //    foreach (string line in lines)
    //    {
    //        textList.Add(line);
    //    }
    //}
}
