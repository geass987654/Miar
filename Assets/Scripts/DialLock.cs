using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialLock : MonoBehaviour
{
    public Text upText;
    public Text middleText;
    public Text downText;
    public int num = 0;
    public float movingPeriod = 0.5f;
    public bool isMoving = false;


    IEnumerator MoveNumber(Vector2 targetPos, float duration)
    {
        isMoving = true;
        float time = 0f;
        Vector2 startPos = transform.position;
            
        while(time < duration)
        {
            transform.position = Vector2.Lerp(startPos, targetPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        middleText.text = num.ToString();
        transform.position = startPos;
        isMoving = false;
    }

    public void PageUp()
    {
        if(!isMoving)
        {
            num = (num + 1) % 10;

            StartCoroutine(MoveNumber(upText.transform.position, movingPeriod));
            downText.text = num.ToString();
        }
    }

    public void PageDown()
    {
        if (!isMoving)
        {
            num = (num + 9) % 10;

            StartCoroutine(MoveNumber(downText.transform.position, movingPeriod));
            upText.text = num.ToString();
        }
    }
}
