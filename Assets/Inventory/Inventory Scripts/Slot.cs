using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public string slotInfo;

    public GameObject itemInSlot;

    public int slotIndex;   //�x�s���I�]��(slots)��檺�s�� = �O���Ϊ��I�]��(playerBag)�D�㪺�s��

    public void ItemOnClicked()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
        InventoryManager.UpdateCurrentItemIndex(slotIndex);
        InventoryManager.SetEquipBtnState(true);
    }

    public void SetUpSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false); //�Y�S���D��A���I�]�����̪��w�]�ťդ��n�X�{
            return;
        }

        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemNum.ToString();
        slotInfo = item.itemInfo;
    }
}
