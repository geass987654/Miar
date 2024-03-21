using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Bag : MonoBehaviour
{
    [SerializeField] private GameObject ActiveInventory; //�Z����D�����
    [SerializeField] private GameObject InventorySystem; //�I�]
    [SerializeField] private GameObject Canvas_UI;       //�C��UI

    private RectTransform currentRect;
    private Vector2 originalPosition;
    private Vector2 newPosition;

    private void Awake()
    {
        currentRect = ActiveInventory.transform.GetComponent<RectTransform>();
        originalPosition = currentRect.anchoredPosition;
        newPosition = new Vector2(-900, 150);
    }

    private void OnEnable()
    {
        ActiveInventory.transform.SetParent(InventorySystem.transform);
        //Debug.Log(currentRect.anchoredPosition);
        currentRect.anchoredPosition = newPosition;
        //Debug.Log(currentRect.anchoredPosition);
    }

    private void OnDisable()
    {
        ActiveInventory.transform.SetParent(Canvas_UI.transform);
        currentRect.anchoredPosition = originalPosition;
    }
}
