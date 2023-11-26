using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialLock : MonoBehaviour
{
    public Transform moveObj;
    public Transform upPos;
    public Transform startPos;
    public Transform downPos;
    public Transform targetPos;
    public Text upText;
    public Text middleText;
    public Text downText;
    public int num = 0;
    public float moveSpeed = 5f;
    public bool isMoving = false;

    private void Update()
    {
        MoveNumber();
    }


    public void MoveNumber()
    {
        if (isMoving)
        {
            moveObj.position = Vector2.Lerp(moveObj.position, targetPos.position, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(moveObj.position, targetPos.position) < 0.5f)
            {
                moveObj.position = targetPos.position;
                middleText.text = num.ToString();
                moveObj.position = startPos.position;
                isMoving = false;
            }
        }
    }

    public void PageUp()
    {
        num = (num + 1) % 10;

        targetPos = upPos;
        downText.text = num.ToString();
        isMoving = true;
    }

    public void PageDown()
    {
        num = (num + 9) % 10;

        targetPos = downPos;
        upText.text = num.ToString();
        isMoving = true;
    }
}
