using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnlargePicture : MonoBehaviour, IPointerClickHandler
{
    private bool isClicked;
    private RectTransform rectTransform;
    private Vector2 originalPos;

    private void Awake()
    {
        isClicked = false;
        rectTransform = GetComponent<RectTransform>();
        originalPos = rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void OnDisable()
    {
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("down" + eventData.pointerCurrentRaycast.gameObject.name);
        if(!isClicked)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            transform.localScale = new Vector3(4f, 4f, 1f);
            isClicked = true;
        }
        else
        {
            rectTransform.anchoredPosition = originalPos;
            transform.localScale = new Vector3(1f, 1f, 1f);
            isClicked = false;
        }
    }

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("up" + eventData.pointerCurrentRaycast.gameObject.name);
    //    //transform.localScale = new Vector3(1f, 1f, 1f);
    //}
}
