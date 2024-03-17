using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private Inventory weapon;          //紀錄用的背包
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;
    [SerializeField] private int equipedItemIndex = -1;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = weaponSlot.transform.GetChild(1);
    }

    public void EquipWeapon()
    {
        itemIndex = InventoryManager.GetCurrentItemIndex();                                   //需要item在背包中的編號
        Item item = weapon.itemList[itemIndex];                                               //取得紀錄中背包的資料
        Item switchedItem = weaponSlot.GetComponent<InventorySlot>().GetCurrentItem();        //欄位中的武器
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);           //背包中的武器

        //設定武器欄位中的裝備資訊、圖片
        weaponSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null) //欄位已有武器，代表背包有原先被鎖定的武器
        {
            //尋找被鎖定的武器在哪一方格中
            for(int i = 0; i < weapon.itemList.Count; i++)
            {
                if (weapon.itemList[i] != null && weapon.itemList[i].equiped)
                {
                    equipedItemIndex = i;
                    break;
                }
            }

            //原本的武器解除鎖定
            Transform lockedItem = slotGrid.transform.GetChild(equipedItemIndex).GetChild(0);     //背包中被鎖定的武器
            lockedItem.GetChild(0).GetComponent<Image>().color = Color.white;
            lockedItem.transform.parent.GetComponent<Slot>().equiped = false;
            weapon.itemList[equipedItemIndex].equiped = false;

            ////原先被取代掉的武器要回到背包
            //choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            //choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            //choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            //weapon.itemList[itemIndex] = switchedItem;
        }
        //else
        //{
        //    //清除原先背包中的圖片、數量、紀錄
        //    choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
        //    choosedItem.GetChild(1).GetComponent<Text>().text = "";
        //    choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
        //    choosedItem.gameObject.SetActive(false);
        //    weapon.itemList[itemIndex] = null;
        //}
        
        //裝備上去的武器要鎖定，包含該武器的方格和紀錄的背包
        choosedItem.GetChild(0).GetComponent<Image>().color = Color.gray;
        choosedItem.transform.parent.GetComponent<Slot>().equiped = true;
        weapon.itemList[itemIndex].equiped = true;

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeWeapon();
    }
}
