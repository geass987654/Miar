using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InheritanceBox : Singleton<InheritanceBox>
{
    public Inventory weapon, essential;                                 //紀錄背包中的道具
    public GameObject weaponGrid, essentialGrid;                        //方格排列
    public GameObject weaponSlot, essentialSlot;                        //空的方格
    public List<GameObject> weaponSlots, essentialSlots;                //儲存背包中的道具
    public int currentItemIndex;
    [SerializeField] private GameObject InventoryWeaponSlot;            //武器欄位
    [SerializeField] private GameObject InventoryItemSlot;              //道具欄位

    [SerializeField] private Inventory weaponInherited, essentialInherited;
    [SerializeField] private GameObject weaponInheritedGrid, essentialInheritedGrid;
    [SerializeField] private GameObject weaponSlot_Box, essentialSlot_Box;
    public List<GameObject> weaponInheritedSlots, essentialInheritedSlots;

    private static int currentGold = 0;
    private static bool haveBeenStored = false;
    private static bool canInherit = false;

    private readonly string postfix = "_Box";                           //後綴，用來辨識背包和繼承箱

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        weaponSlots = new List<GameObject>();
        essentialSlots = new List<GameObject>();
        weaponInheritedSlots = new List<GameObject>();
        essentialInheritedSlots = new List<GameObject>();
    }

    private void OnEnable()
    {
        RefreshWeapons();
        RefreshWeaponsInherited();
        RefreshEssentials();
        RefreshEssentialsInherited();
        ActiveWeapon.Instance.canAttack = false; //開啟繼承箱時無法攻擊
        ActiveInventory.Instance.canUse = false; //開啟繼承箱時無法使用道具

        if(haveBeenStored && canInherit) //曾經儲存過資料，且角色死亡過
        {
            InheritGold();
            Debug.Log("InheritGold");
        }
        else
        {
            StoreGold();
            Debug.Log("StoreGold");
        }
    }

    private void OnDisable()
    {
        InventoryManager.RefreshWeapons();
        InventoryManager.RefreshEssentials();
        ActiveWeapon.Instance.canAttack = true;
        ActiveInventory.Instance.canUse = true;
    }

    public void RefreshWeapons()
    {
        for (int i = 0; i < weaponGrid.transform.childCount; i++)
        {
            if (weaponGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(weaponGrid.transform.GetChild(i).gameObject);

        }
        weaponSlots.Clear();

        for (int i = 0; i < weapon.itemList.Count; i++)
        {
            weaponSlots.Add(Instantiate(weaponSlot));
            weaponSlots[i].transform.SetParent(weaponGrid.transform);
            weaponSlots[i].GetComponent<Slot>().slotIndex = i;
            weaponSlots[i].GetComponent<Slot>().SetUpSlot(weapon.itemList[i]);
        }
    }

    public void RefreshEssentials()
    {
        for (int i = 0; i < essentialGrid.transform.childCount; i++)
        {
            if (essentialGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(essentialGrid.transform.GetChild(i).gameObject);


        }
        essentialSlots.Clear();

        for (int i = 0; i < essential.itemList.Count; i++)
        {
            essentialSlots.Add(Instantiate(essentialSlot));
            essentialSlots[i].transform.SetParent(essentialGrid.transform);
            essentialSlots[i].GetComponent<Slot>().slotIndex = i;
            essentialSlots[i].GetComponent<Slot>().SetUpSlot(essential.itemList[i]);
        }
    }

    public void RefreshWeaponsInherited()
    {
        for (int i = 0; i < weaponInheritedGrid.transform.childCount; i++)
        {
            if (weaponInheritedGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(weaponInheritedGrid.transform.GetChild(i).gameObject);

        }
        weaponInheritedSlots.Clear();

        for (int i = 0; i < weaponInherited.itemList.Count; i++)
        {
            weaponInheritedSlots.Add(Instantiate(weaponSlot_Box));
            weaponInheritedSlots[i].transform.SetParent(weaponInheritedGrid.transform);
            weaponInheritedSlots[i].GetComponent<Slot>().slotIndex = i;
            weaponInheritedSlots[i].GetComponent<Slot>().SetUpSlot(weaponInherited.itemList[i]);
            weaponInheritedSlots[i].transform.GetChild(0).GetChild(0).gameObject.name += postfix;
        }
    }

    public void RefreshEssentialsInherited()
    {
        for (int i = 0; i < essentialInheritedGrid.transform.childCount; i++)
        {
            if (essentialInheritedGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(essentialInheritedGrid.transform.GetChild(i).gameObject);

        }
        essentialInheritedSlots.Clear();

        for (int i = 0; i < essentialInherited.itemList.Count; i++)
        {
            essentialInheritedSlots.Add(Instantiate(essentialSlot_Box));
            essentialInheritedSlots[i].transform.SetParent(essentialInheritedGrid.transform);
            essentialInheritedSlots[i].GetComponent<Slot>().slotIndex = i;
            essentialInheritedSlots[i].GetComponent<Slot>().SetUpSlot(essentialInherited.itemList[i]);
            essentialInheritedSlots[i].transform.GetChild(0).GetChild(0).gameObject.name += postfix;
        }
    }

    public void UpdateCurrentItemIndex(int slotIndex)
    {
        currentItemIndex = slotIndex;
    }

    public int GetCurrentItemIndex()
    {
        return currentItemIndex;
    }

    public void StoreGold()
    {
        currentGold = EconomyManager.Instance.currentGold;

        haveBeenStored = true;
    }

    public void InheritGold()
    {
        EconomyManager.Instance.currentGold = currentGold;
        EconomyManager.Instance.UpdateCurrentGold();

        canInherit = false;
    }

    public void SetCanInherit(bool state)
    {
        canInherit = state;
    }
}
