using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    /*
        背包系統 : Hierarchy中的物件，由上至下依序為，背包(PlayerBag)、方格排列(Grid)、方格(Slot)、
        道具(Item)、道具圖片及數量(Item Image、Item Number)；
        儲存資料用兩個可編輯物件 : Item(表示道具)、Inventory(表示背包，或可解釋為道具的儲存庫)
    */
    //static InventoryManager instance;
    //public Inventory playerBag;      //紀錄背包中的道具
    //public GameObject slotGrid;     //方格排列
    //public GameObject emptySlot;    //空的方格
    //public Text itemInfo;           //道具資訊
    //public List<GameObject> slots = new List<GameObject>(); //儲存背包中的道具

    static InventoryManager instance;
    //public Inventory equipmentBag, essentialBag, chipBag;              //紀錄背包中的道具
    public Inventory weapon, essential;
    public GameObject weaponGrid, essentialGrid;
    //public GameObject equipment, essential, chip;                    //方格排列
    public GameObject weaponSlot, essentialSlot;                       //空的方格
    public Text itemInfo;                                              //道具資訊
    public List<GameObject> weaponSlots, essentialSlots;
    public int currentItemIndex;
    [SerializeField] private GameObject EquipButton;
    [SerializeField] private GameObject UseButton;
    //public List<GameObject> equipmentSlots = new List<GameObject>();   //儲存背包中的道具
    //public List<GameObject> essentialSlots = new List<GameObject>();   //儲存背包中的道具
    //public List<GameObject> chipSlots = new List<GameObject>();        //儲存背包中的道具

    //static InventoryManager instance;
    //public Inventory[] playerBag = new Inventory[3];             //紀錄背包中的道具
    //public GameObject[] slotGrid = new GameObject[3];            //方格排列
    //public GameObject emptySlot;                                 //空的方格
    //public Text itemInfo;                                        //道具資訊
    //public List<GameObject>[] slots = new List<GameObject>[3];   //儲存背包中的道具

    //public Slot slotPrefab;         //背包中的方格裡的道具


    private void Awake()
    {
        /*
            singleton(單例模式)，確保InventoryManager只會有一個實例
         */
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        instance.weaponSlots = new List<GameObject>();
        instance.essentialSlots = new List<GameObject>();
    }

    private void OnEnable()
    {
        RefreshWeapons();
        RefreshEssentials();
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
        在遊戲執行前，Inventory類別的playerBag中的itemList，已準備好可儲存道具的空間(預設方格數量為18)；
        遊戲開始後，一旦角色碰撞到場景中的道具，例如 Sword 或 Shoes，將其加入到itemList；
        當背包開啟時(按下鍵盤的'B')，使用 RefreshItem() 進行背包中道具的更新 : 
        銷毀背包中的所有方格，藉由 Inventory 的 itemList 重新產生，
        1.根據Grid中有多少子物件，也就是背包中有多少方格，將其逐項銷毀，並清除slots原先的紀錄
        2.根據itemList的元素數量，也就是可儲存道具的數量，逐項產生空白的方格，將其加入到slots中；
          slots的元素設為Grid的子物件，完成方格的排列；給予空白的方格編號，以利後續道具的拖曳對調，
          最後傳送itemList的各項資料到slots各方格中。
    */

    public static void RefreshWeapons()
    {
        for (int i = 0; i < instance.weaponGrid.transform.childCount; i++)
        {
            if (instance.weaponGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.weaponGrid.transform.GetChild(i).gameObject);
            
            instance.weaponSlots.Clear();
        }

        for (int i = 0; i < instance.weapon.itemList.Count; i++)
        {
            instance.weaponSlots.Add(Instantiate(instance.weaponSlot));
            instance.weaponSlots[i].transform.SetParent(instance.weaponGrid.transform);
            instance.weaponSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.weaponSlots[i].GetComponent<Slot>().SetUpSlot(instance.weapon.itemList[i]);
        }
    }

    public static void RefreshEssentials()
    {
        for (int i = 0; i < instance.essentialGrid.transform.childCount; i++)
        {
            if (instance.essentialGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.essentialGrid.transform.GetChild(i).gameObject);

            instance.essentialSlots.Clear();
        }

        for (int i = 0; i < instance.essential.itemList.Count; i++)
        {
            instance.essentialSlots.Add(Instantiate(instance.essentialSlot));
            instance.essentialSlots[i].transform.SetParent(instance.essentialGrid.transform);
            instance.essentialSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.essentialSlots[i].GetComponent<Slot>().SetUpSlot(instance.essential.itemList[i]);
        }
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfo.text = itemDescription;
    }

    public static void UpdateCurrentItemIndex(int slotIndex)
    {
        instance.currentItemIndex = slotIndex;
    }

    public static int GetCurrentItemIndex()
    {
        return instance.currentItemIndex;
    }

    public static void CleanItemInfo()
    {
        instance.itemInfo.text = "";
    }

    public static void SetEquipBtnState(bool state)
    {
        instance.EquipButton.SetActive(state);
    }

    public static void SetUseBtnState(bool state)
    {
        instance.UseButton.SetActive(state);
    }

    public static void SetEquipBtnComponent(bool state)
    {
        instance.EquipButton.GetComponent<Button>().enabled = state;
    }
    public static void SetUseBtnComponent(bool state)
    {
        instance.UseButton.GetComponent<Button>().enabled = state;
    }

    //public static void RefreshItemOnEquipment()
    //{
    //    //Debug.Log("equipment start");
    //    for (int i = 0; i < instance.equipment.transform.childCount; i++)
    //    {
    //        if (instance.equipment.transform.childCount == 0)
    //        {
    //            break;
    //        }
    //        Destroy(instance.equipment.transform.GetChild(i).gameObject);
    //        //Debug.Log("equipment destroy" + i);
    //        instance.equipmentSlots.Clear();
    //    }

    //    for (int i = 0; i < instance.equipmentBag.itemList.Count; i++)
    //    {
    //        //CreateNewItem(instance.playerBag.itemList[i]);
    //        instance.equipmentSlots.Add(Instantiate(instance.emptySlot));
    //        instance.equipmentSlots[i].transform.SetParent(instance.equipment.transform);
    //        instance.equipmentSlots[i].GetComponent<Slot>().slotIndex = i;
    //        instance.equipmentSlots[i].GetComponent<Slot>().SetupSlot(instance.equipmentBag.itemList[i]);
    //        //Debug.Log("equipment instantiate" + i);

    //        instance.equipment.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.equipmentBag;
    //    }
    //}
    //public static void RefreshItemOnEssential()
    //{
    //    //Debug.Log("essential start");
    //    for (int i = 0; i < instance.essential.transform.childCount; i++)
    //    {
    //        if (instance.essential.transform.childCount == 0)
    //        {
    //            break;
    //        }
    //        Destroy(instance.essential.transform.GetChild(i).gameObject);
    //        //Debug.Log("essential destroy" + i);
    //        instance.essentialSlots.Clear();
    //    }

    //    for (int i = 0; i < instance.essentialBag.itemList.Count; i++)
    //    {
    //        //CreateNewItem(instance.playerBag.itemList[i]);
    //        instance.essentialSlots.Add(Instantiate(instance.emptySlot));
    //        instance.essentialSlots[i].transform.SetParent(instance.essential.transform);
    //        instance.essentialSlots[i].GetComponent<Slot>().slotIndex = i;
    //        instance.essentialSlots[i].GetComponent<Slot>().SetupSlot(instance.essentialBag.itemList[i]);
    //        //Debug.Log("essential instantiate" + i);
    //        instance.essential.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.essentialBag;
    //    }
    //}
    //public static void RefreshItemOnChip()
    //{
    //    for (int i = 0; i < instance.chip.transform.childCount; i++)
    //    {
    //        if (instance.chip.transform.childCount == 0)
    //        {
    //            break;
    //        }
    //        Destroy(instance.chip.transform.GetChild(i).gameObject);
    //        instance.chipSlots.Clear();
    //    }

    //    for (int i = 0; i < instance.chipBag.itemList.Count; i++)
    //    {
    //        //CreateNewItem(instance.playerBag.itemList[i]);
    //        instance.chipSlots.Add(Instantiate(instance.emptySlot));
    //        instance.chipSlots[i].transform.SetParent(instance.chip.transform);
    //        instance.chipSlots[i].GetComponent<Slot>().slotIndex = i;
    //        instance.chipSlots[i].GetComponent<Slot>().SetupSlot(instance.chipBag.itemList[i]);
    //        instance.chip.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.chipBag;
    //    }
    //}
}
