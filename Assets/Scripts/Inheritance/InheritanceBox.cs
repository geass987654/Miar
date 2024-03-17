using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InheritanceBox : MonoBehaviour
{
    public Inventory weapon, essential;                                 //�����I�]�����D��
    public GameObject weaponGrid, essentialGrid;                        //���ƦC
    public GameObject weaponSlot, essentialSlot;                        //�Ū����
    public Text itemInfo;                                               //�D���T
    public List<GameObject> weaponSlots, essentialSlots;                //�x�s�I�]�����D��
    public int currentItemIndex;
    [SerializeField] private GameObject InventoryWeaponSlot;            //�Z�����
    [SerializeField] private GameObject InventoryItemSlot;              //�D�����
    [SerializeField] private Inventory weaponInherited, essentialInherited;
    private static int currentGold = 0;
    private static bool haveBeenStored = false;
    private static bool canInherit = false;
    private readonly Color32 activeColor = new Color32(166, 24, 4, 255);

    private void Awake()
    {

    }

    private void Start()
    {
        weaponSlots = new List<GameObject>();
        essentialSlots = new List<GameObject>();
    }

    private void OnEnable()
    {
        //RefreshWeapons();
        //RefreshEssentials();
        itemInfo.text = "";
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

    public void UpdateItemInfo(string itemDescription)
    {
        itemInfo.text = itemDescription;
    }

    public void CleanItemInfo()
    {
        itemInfo.text = "";
    }

    public void UpdateCurrentItemIndex(int slotIndex)
    {
        currentItemIndex = slotIndex;
    }

    public int GetCurrentItemIndex()
    {
        return currentItemIndex;
    }

    public void Clear()
    {
        for (int i = 0; i < weapon.itemList.Count; i++)
        {
            weapon.itemList[i] = null;
        }
        for (int i = 0; i < essential.itemList.Count; i++)
        {
            essential.itemList[i] = null;
        }
    }

    public void Store()
    {
        for (int i = 0; i < weaponInherited.itemList.Count; i++)
        {
            weaponInherited.itemList[i] = weapon.itemList[i];
        }
        for (int i = 0; i < essentialInherited.itemList.Count; i++)
        {
            essentialInherited.itemList[i] = essential.itemList[i];
        }

        //Item weaponInSlot = InventoryWeaponSlot.GetComponent<InventorySlot>().GetCurrentItem();

        //if (weaponInSlot != null)
        //{
        //    for (int i = 0; i < weaponInherited.itemList.Count; i++)
        //    {
        //        if (weaponInherited.itemList[i] == null)
        //        {
        //            weaponInherited.itemList[i] = weaponInSlot;
        //            break;
        //        }
        //    }
        //}

        //Item essentialInSlot = InventoryItemSlot.GetComponent<InventorySlot>().GetCurrentItem();

        //if (essentialInSlot != null)
        //{
        //    for (int i = 0; i < essentialInherited.itemList.Count; i++)
        //    {
        //        if (essentialInherited.itemList[i] == null)
        //        {
        //            essentialInherited.itemList[i] = essentialInSlot;
        //            break;
        //        }
        //    }
        //}

        currentGold = EconomyManager.Instance.currentGold;

        haveBeenStored = true;
    }

    public void Inherit()
    {
        for (int i = 0; i < weapon.itemList.Count; i++)
        {
            weapon.itemList[i] = weaponInherited.itemList[i];
        }
        for (int i = 0; i < essential.itemList.Count; i++)
        {
            essential.itemList[i] = essentialInherited.itemList[i];
        }

        EconomyManager.Instance.currentGold = currentGold;
        EconomyManager.Instance.UpdateCurrentGold();

        canInherit = false;
    }

    public void SetCanInherit(bool state)
    {
        canInherit = state;
    }

    public bool CanInheritFromBox()
    {
        return haveBeenStored && canInherit;
    }
}
