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
    static InventoryManager instance;
    public Inventory weapon, essential;                                 //紀錄背包中的道具
    public GameObject weaponGrid, essentialGrid;                        //方格排列
    public GameObject weaponSlot, essentialSlot;                        //空的方格
    public Text itemInfo;                                               //道具資訊
    public List<GameObject> weaponSlots, essentialSlots;                //儲存背包中的道具
    public int currentItemIndex;
    [SerializeField] private GameObject InventoryWeaponSlot;            //武器欄位
    [SerializeField] private GameObject InventoryItemSlot;              //道具欄位
    [SerializeField] private GameObject EquipButton;                    //武器裝備按鈕
    [SerializeField] private GameObject UseButton;                      //道具裝備按鈕
    //[SerializeField] private Inventory weaponInherited, essentialInherited;
    private static int currentGold = 0;
    //private static bool haveBeenStored = false;
    //private static bool canInherit = false;
    private readonly Color32 activeColor = new Color32(166, 24, 4, 255);

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
        //RefreshWeapons();
        //RefreshEssentials();
        instance.EquipButton.SetActive(false);
        instance.UseButton.SetActive(false);
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
        在遊戲執行前，Inventory類別的playerBag中的itemList，已準備好可儲存道具的空間(預設方格數量為18)；
        遊戲開始後，一旦角色碰撞到場景中的道具，例如 Sword 或 Shoes，將其加入到itemList；
        當背包開啟時(按下鍵盤的'B')，使用 RefreshItem() 進行背包中道具的更新 : 
        銷毀背包中的所有方格，藉由 Inventory 的 itemList 重新產生，
        1.根據Grid中有多少子物件，也就是背包中有多少方格，將其逐項銷毀，並清除slots原先的紀錄
        2.根據itemList的元素數量，也就是可儲存道具的數量，逐項產生空白的方格，將其加入到slots中；
          slots的元素設為Grid的子物件，完成方格的排列；給予空白的方格編號，以利後續道具的拖曳對調，
          最後傳送itemList的各項資料到slots各方格中。
    */

    public static void Initialize()
    {
        for(int i = 0; i < instance.weapon.itemList.Count; i++)
        {
            if (instance.weapon.itemList[i] != null)
            {
                instance.weapon.itemList[i].equiped = false;
                instance.weapon.itemList[i].isCooldown = false;
            }
        }

        for (int i = 0; i < instance.essential.itemList.Count; i++)
        {
            if (instance.essential.itemList[i] != null)
            {
                instance.essential.itemList[i].equiped = false;
                instance.essential.itemList[i].isCooldown = false;
            }
        }
    }

    public static void RefreshWeapons()
    {
        for (int i = 0; i < instance.weaponGrid.transform.childCount; i++)
        {
            if (instance.weaponGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.weaponGrid.transform.GetChild(i).gameObject);
            
        }
        instance.weaponSlots.Clear();

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


        }
        instance.essentialSlots.Clear();

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

    public static void CleanItemInfo()
    {
        instance.itemInfo.text = "";
    }

    public static void UpdateCurrentItemIndex(int slotIndex)
    {
        instance.currentItemIndex = slotIndex;
    }

    public static int GetCurrentItemIndex()
    {
        return instance.currentItemIndex;
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

        if (instance.EquipButton.GetComponent<Button>().enabled)
        {
            instance.EquipButton.GetComponent<Image>().color = instance.activeColor;
        }
        else
        {
            instance.EquipButton.GetComponent<Image>().color = Color.gray;
        }
    }
    public static void SetUseBtnComponent(bool state)
    {
        instance.UseButton.GetComponent<Button>().enabled = state;

        if(instance.UseButton.GetComponent<Button>().enabled)
        {
            instance.UseButton.GetComponent<Image>().color = instance.activeColor;
        }
        else
        {
            instance.UseButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public static void Clear()
    {
        for (int i = 0; i < instance.weapon.itemList.Count; i++)
        {
            instance.weapon.itemList[i] = null;
        }
        for (int i = 0; i < instance.essential.itemList.Count; i++)
        {
            instance.essential.itemList[i] = null;
        }
    }

    //public static void Store()
    //{
    //    for (int i = 0; i < instance.weaponInherited.itemList.Count; i++)
    //    {
    //        instance.weaponInherited.itemList[i] = instance.weapon.itemList[i];
    //    }
    //    for (int i = 0; i < instance.essentialInherited.itemList.Count; i++)
    //    {
    //        instance.essentialInherited.itemList[i] = instance.essential.itemList[i];
    //    }

    //    Item weaponInSlot = instance.InventoryWeaponSlot.GetComponent<InventorySlot>().GetCurrentItem();

    //    if(weaponInSlot != null)
    //    {
    //        for(int i = 0; i < instance.weaponInherited.itemList.Count; i++)
    //        {
    //            if (instance.weaponInherited.itemList[i] == null)
    //            {
    //                instance.weaponInherited.itemList[i] = weaponInSlot;
    //                break;
    //            }
    //        }
    //    }

    //    Item essentialInSlot = instance.InventoryItemSlot.GetComponent<InventorySlot>().GetCurrentItem();

    //    if (essentialInSlot != null)
    //    {
    //        for (int i = 0; i < instance.essentialInherited.itemList.Count; i++)
    //        {
    //            if (instance.essentialInherited.itemList[i] == null)
    //            {
    //                instance.essentialInherited.itemList[i] = essentialInSlot;
    //                break;
    //            }
    //        }
    //    }

    //    currentGold = EconomyManager.Instance.currentGold;

    //    haveBeenStored = true;
    //}

    //public static void Inherit()
    //{
    //    for(int i = 0; i < instance.weapon.itemList.Count; i++)
    //    {
    //        instance.weapon.itemList[i] = instance.weaponInherited.itemList[i];
    //    }
    //    for (int i = 0; i < instance.essential.itemList.Count; i++)
    //    {
    //        instance.essential.itemList[i] = instance.essentialInherited.itemList[i];
    //    }

    //    EconomyManager.Instance.currentGold = currentGold;
    //    EconomyManager.Instance.UpdateCurrentGold();

    //    canInherit = false;
    //}

    //public static void SetCanInherit(bool state)
    //{
    //    canInherit = state;
    //}

    //public static bool CanInheritFromBox()
    //{
    //    return haveBeenStored && canInherit;
    //}
}
