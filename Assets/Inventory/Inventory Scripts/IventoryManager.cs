using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IventoryManager : MonoBehaviour
{
    /*
        背包系統 : Hierarchy中的物件，由上至下依序為，背包(PlayerBag)、方格排列(Grid)、方格(Slot)、
        道具(Item)、道具圖片及數量(Item Image、Item Number)；
        儲存資料用兩個可編輯物件 : Item(表示道具)、Iventory(表示背包，或可解釋為道具的儲存庫)
    */
    static IventoryManager instance;

    public Iventory playerBag;      //紀錄背包中的道具
    public GameObject slotGrid;     //方格排列
    public GameObject emptySlot;    //空的方格
    public Text itemInfo;           //道具資訊
    public List<GameObject> slots = new List<GameObject>(); //儲存背包中的道具

    //public Slot slotPrefab;         //背包中的方格裡的道具

    
    private void Awake()
    {
        /*
            singleton(單例模式)，確保IventoryManager只會有一個實例
         */
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void OnEnable()
    {
        RefreshItem();
        instance.itemInfo.text = "";
    }

    /*
        角色碰撞到場景中的道具item，獲取道具的資訊，作為參數傳入到CreateNewItem()，
        並新增到背包中，用Instantiate()，產生背包方格中的道具slot，
        參數分別為:1.物件產生類別(slot)、2.物件產生位置(背包方格中)、3.物件旋轉角度(預設)，
        再使其成為背包中方格排列slotGrid的子物件，以達成排列的效果，
        最後，將道具種類、道具圖片、道具數量等資訊，傳送到背包方格中的道具(slot)
    */
    /*
    public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate
            (
                instance.slotPrefab,
                instance.slotGrid.transform.position,
                Quaternion.identity
            );
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemNum.ToString();
    }
    //*/

    /*
        在遊戲執行前，Iventory類別的playerBag中的itemList，已準備好可儲存道具的空間(預設方格數量為18)；
        遊戲開始後，一旦角色碰撞到場景中的道具，例如 Sword 或 Shoes，將其加入到itemList；
        當背包開啟時(按下鍵盤的'B')，使用 RefreshItem() 進行背包中道具的更新 : 
        銷毀背包中的所有方格，藉由 Iventory 的 itemList 重新產生，
        1.根據Grid中有多少子物件，也就是背包中有多少方格，將其逐項銷毀，並清除slots原先的紀錄
        2.根據itemList的元素數量，也就是可儲存道具的數量，逐項產生空白的方格，將其加入到slots中；
          slots的元素設為Grid的子物件，完成方格的排列；給予空白的方格編號，以利後續道具的拖曳對調，
          最後傳送itemList的各項資料到slots各方格中。
    */

    public static void RefreshItem()
    {
        for (int i = 0; i < instance.slotGrid.transform.childCount; i++)
        {
            if (instance.slotGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
            instance.slots.Clear();
        }

        for (int i = 0; i < instance.playerBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.playerBag.itemList[i]);
            instance.slots.Add(Instantiate(instance.emptySlot));
            instance.slots[i].transform.SetParent(instance.slotGrid.transform);
            instance.slots[i].GetComponent<Slot>().slotIndex = i;
            instance.slots[i].GetComponent<Slot>().SetupSlot(instance.playerBag.itemList[i]);
        }
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfo.text = itemDescription;
    }
}
