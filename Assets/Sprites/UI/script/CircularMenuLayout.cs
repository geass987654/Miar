using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenuLayout : MonoBehaviour
{
    public RectTransform[] buttons;
    public float radius = 100f;

    void Start()
    {
        ArrangeButtonsOnCircle();
    }

    void ArrangeButtonsOnCircle()
    {
        float angleDelta = 360f / buttons.Length;
        float angle = 0f;

        for (int i = 0; i < buttons.Length; i++)
        {
            float radian = Mathf.Deg2Rad * angle;
            float x = Mathf.Cos(radian) * radius;
            float y = Mathf.Sin(radian) * radius;

            buttons[i].anchoredPosition = new Vector2(x, y);

            angle += angleDelta;
        }
    }
}