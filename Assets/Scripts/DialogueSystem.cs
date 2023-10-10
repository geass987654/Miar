using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    [Header("UI����")]
    public Text textLabel;  //��ܮؤ���ܪ���r���e
    public Image faceImage; //���b���ܪ������Y��

    [Header("�奻")]
    public TextAsset textFile;  //��r�ɮ�
    public int textIndex;       //��r�b�ĴX��A�q0�}�l
    public float textSpeed;     //��r��X�t��

    [Header("�Y��")]
    public Sprite face1, face2; //face1�N��D���Aface2�N��v��

    public bool isTextFinished, isTextCanceled; //��r�O�_���b��X�A��r�O�_�����v�r��X

    public List<string> textList = new List<string>();  //�x�s�C�@���r

    private void Awake()
    {
        GetTextFromFile(textFile);
    }

    private void OnEnable()
    {
        isTextFinished = true;
        StartCoroutine(SetTextUI());
    }

    private void Update()
    {
        //���U'F'�i�椬�ʡA��Ҧ���r��X�����A������ܮ�
        if (Input.GetKeyDown(KeyCode.F) && textIndex == textList.Count)
        {
            gameObject.SetActive(false);
            textIndex = 0;
            return;
        }

        /*
            �w�]���U'F'�A��X��g��r : 
            ���b��X��r�B�S��������X�ɡA�ϥ�Coroutine�v�r��X�F
            ���b��X��r�B������X�ɡA�N��r��g������X
            �S����X��r�ɡA�����B�z
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

    void GetTextFromFile(TextAsset file)
    {
        textList.Clear();
        textIndex = 0;

        //�ھڤ��P���t�ΡA�N�쥻����r�ɮסA�H����Ÿ����Φ��r��A�}�C���C�Ӥ����O�@��r��
        string[] lines = file.text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        //�N�r��}�C�����C�@��r��A�[�J��C���A�H�x�s�C�@���r
        foreach (string line in lines)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI()
    {
        isTextFinished = false; //��r���b��X
        textLabel.text = "";    //�M�Ź�ܮ�

        //�ھڤ��P����������Y���A�ø���U�@���r
        switch (textList[textIndex])
        {
            case "Player":
                Debug.Log(textList[textIndex]);
                faceImage.sprite = face1;
                textIndex++;
                break;
            case "Sensei":
                Debug.Log(textList[textIndex]);
                faceImage.sprite = face2;
                textIndex++;
                break;
        }

        /*
            i�N��C�@�檺�ĴX�Ӧr���A�q0�}�l�A�@����Ӧ�r�����-1�FtextIndex�N��ĴX��
            �N�Ӧ檺��i�Ӧr���A�v�r�W�[���ܮؤ��A�õ��ݤ@�w���ɶ��A�w�]��0.05��F
            ������r�v�r��X�ɡA�N����r�W�[���ܮ�
        */
        int i = 0;
        while (!isTextCanceled && i < textList[textIndex].Length)
        {
            textLabel.text += textList[textIndex][i];
            i++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[textIndex];

        isTextCanceled = false; //��r�S��������X
        isTextFinished = true;  //��r��X����
        textIndex++;            //���W��U�@����
    }
}
