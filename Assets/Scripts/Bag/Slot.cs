using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //public Item slotItem;
    public Image slotImage;
    public Text slotNum;
    public string slotInfo;
    public GameObject itemInSlot;
    public int slotIndex;   //儲存的背包中(slots)方格的編號 = 記錄用的背包中(playerBag)道具的編號
    public bool equiped;

    public void ItemOnClickedInWeapons()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
        InventoryManager.UpdateCurrentItemIndex(slotIndex);
        InventoryManager.SetEquipBtnState(true);
        if (ActiveInventory.Instance.weaponCoolDown || equiped)
        {
            InventoryManager.SetEquipBtnComponent(false);
        }
        else
        {
            InventoryManager.SetEquipBtnComponent(true);
        }

        if(InheritanceBox.Instance != null && InheritanceBox.Instance.gameObject.activeSelf)
        {
            InheritanceBox.Instance.UpdateCurrentItemIndex(slotIndex);
        }
    }
    public void ItemOnClickedInEssentials()
    {
        InventoryManager.UpdateItemInfo(slotInfo);
        InventoryManager.UpdateCurrentItemIndex(slotIndex);
        InventoryManager.SetUseBtnState(true);
        if(ActiveInventory.Instance.itemCoolDown || equiped)
        {
            InventoryManager.SetUseBtnComponent(false);
        }
        else
        {
            InventoryManager.SetUseBtnComponent(true);
        }

        if (InheritanceBox.Instance != null && InheritanceBox.Instance.gameObject.activeSelf)
        {
            InheritanceBox.Instance.UpdateCurrentItemIndex(slotIndex);
        }
    }
    public void SetUpSlot(Item item)
    {
        if (item == null)
        {
            itemInSlot.SetActive(false); //若沒有道具，讓背包中方格裡的預設空白不要出現
            return;
        }

        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemNum.ToString();
        slotInfo = item.itemInfo;
        equiped = item.equiped;

        if(equiped)
        {
            slotImage.color = Color.gray;
        }
    }
}
