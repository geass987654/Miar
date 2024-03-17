using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject essestialSlot;
    [SerializeField] private Inventory essential;           //紀錄用的背包
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;
    [SerializeField] private int equipedItemIndex = -1;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = essestialSlot.transform.GetChild(1);
    }

    public void UseItem()
    {
        itemIndex = InventoryManager.GetCurrentItemIndex();                                   //需要item在背包中的編號
        Item item = essential.itemList[itemIndex];                                            //取得紀錄中背包的資料
        Item switchedItem = essestialSlot.GetComponent<InventorySlot>().GetCurrentItem();     //欄位中的道具
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);           //背包中的道具
        string itemName = item.itemName;

        //設定道具欄位中的道具資訊、圖片
        essestialSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null)
        {
            //尋找被鎖定的道具在哪一方格中
            for (int i = 0; i < essential.itemList.Count; i++)
            {
                if (essential.itemList[i] != null && essential.itemList[i].equiped)
                {
                    equipedItemIndex = i;
                    break;
                }
            }

            //原本的道具解除鎖定
            Transform lockedItem = slotGrid.transform.GetChild(equipedItemIndex).GetChild(0);     //背包中被鎖定的道具
            lockedItem.GetChild(0).GetComponent<Image>().color = Color.white;
            lockedItem.transform.parent.GetComponent<Slot>().equiped = false;
            essential.itemList[equipedItemIndex].equiped = false;

            ////原先被取代掉的道具要回到背包
            //choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            //choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            //choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            //essential.itemList[itemIndex] = switchedItem;
        }
        //else
        //{
        //    //清除原先背包中的圖片、數量、紀錄
        //    choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
        //    choosedItem.GetChild(1).GetComponent<Text>().text = "";
        //    choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
        //    choosedItem.gameObject.SetActive(false);
        //    essential.itemList[itemIndex] = null;
        //}
        
        //裝備上去的道具要鎖定，包含該道具的方格和紀錄的背包
        choosedItem.GetChild(0).GetComponent<Image>().color = Color.gray;
        choosedItem.transform.parent.GetComponent<Slot>().equiped = true;
        essential.itemList[itemIndex].equiped = true;

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeItem(itemName);
    }
}
