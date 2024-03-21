using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBox : MonoBehaviour
{
    [SerializeField] private GameObject inheritanceBox;

    public void OnCloseBox()
    {
        inheritanceBox.SetActive(false);
    }
}
