using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private Inventory playerBag; //紀錄用的背包
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = weaponSlot.transform.GetChild(1);
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void EquipItem()
    {
        //需要item在背包中的編號
        itemIndex = InventoryManager.GetCurrentItemIndex();
        //Debug.Log("itemIndex" + itemIndex);

        //取得紀錄中背包的資料、被選取的方格
        Item item = playerBag.itemList[itemIndex];
        Item switchedItem = weaponSlot.GetComponent<InventorySlot>().GetCurrentItem();
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);

        //將其移到武器裝備欄中或交換

        //設定武器欄位中的裝備資訊、圖片
        weaponSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null)
        {
            //原先被取代掉的武器要回到背包
            choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            playerBag.itemList[itemIndex] = switchedItem;
        }
        else
        {
            //清除原先背包中的圖片、數量、紀錄
            choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
            choosedItem.GetChild(1).GetComponent<Text>().text = "";
            choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
            choosedItem.gameObject.SetActive(false);
            playerBag.itemList[itemIndex] = null;
        }

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeWeapon();
    }
}
