using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour, IDragHandler
{
    RectTransform currentRect;  //�I�]UI��e����m

    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta;
        //�즲�ɥHUI���������I����ǲ���(anchoredPosition)�A�����q����Ъ�����(eventData.delta)
    }

    void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
}
