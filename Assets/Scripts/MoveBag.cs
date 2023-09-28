using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBag : MonoBehaviour, IDragHandler
{
    RectTransform currentRect;  //背包UI當前的位置

    public void OnDrag(PointerEventData eventData)
    {
        currentRect.anchoredPosition += eventData.delta;
        //拖曳時以UI的中心錨點為基準移動(anchoredPosition)，偏移量為游標的移動(eventData.delta)
    }

    void Awake()
    {
        currentRect = GetComponent<RectTransform>();
    }
}
