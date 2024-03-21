using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    /*
        �I�]�t�� : Hierarchy��������A�ѤW�ܤU�̧Ǭ��A�I�](PlayerBag)�B���ƦC(Grid)�B���(Slot)�B
        �D��(Item)�B�D��Ϥ��μƶq(Item Image�BItem Number)�F
        �x�s��ƥΨ�ӥi�s�誫�� : Item(��ܹD��)�BInventory(��ܭI�]�A�Υi�������D�㪺�x�s�w)
    */
    static InventoryManager instance;
    public Inventory weapon, essential;                                 //�����I�]�����D��
    public GameObject weaponGrid, essentialGrid;                        //���ƦC
    public GameObject weaponSlot, essentialSlot;                        //�Ū����
    public Text itemInfo;                                               //�D���T
    public List<GameObject> weaponSlots, essentialSlots;                //�x�s�I�]�����D��
    public int currentItemIndex;
    [SerializeField] private GameObject InventoryWeaponSlot;            //�Z�����
    [SerializeField] private GameObject InventoryItemSlot;              //�D�����
    [SerializeField] private GameObject EquipButton;                    //�Z���˳ƫ��s
    [SerializeField] private GameObject UseButton;                      //�D��˳ƫ��s
    //[SerializeField] private Inventory weaponInherited, essentialInherited;
    private static int currentGold = 0;
    //private static bool haveBeenStored = false;
    //private static bool canInherit = false;
    private readonly Color32 activeColor = new Color32(166, 24, 4, 255);

    private void Awake()
    {
        /*
            singleton(��ҼҦ�)�A�T�OInventoryManager�u�|���@�ӹ��
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
        ����I������������D��item�A����D�㪺��T�A�@���ѼƶǤJ��CreateNewItem()�A
        �÷s�W��I�]���A��Instantiate()�A���ͭI�]��椤���D��slot�A
        �ѼƤ��O��:1.���󲣥����O(slot)�B2.���󲣥ͦ�m(�I�]��椤)�B3.������ਤ��(�w�])�A
        �A�Ϩ䦨���I�]�����ƦCslotGrid���l����A�H�F���ƦC���ĪG�A
        �̫�A�N�D������B�D��Ϥ��B�D��ƶq����T�A�ǰe��I�]��椤���D��(slot)
    */

    /*
        �b�C������e�AInventory���O��playerBag����itemList�A�w�ǳƦn�i�x�s�D�㪺�Ŷ�(�w�]���ƶq��18)�F
        �C���}�l��A�@������I������������D��A�Ҧp Sword �� Shoes�A�N��[�J��itemList�F
        ��I�]�}�Ү�(���U��L��'B')�A�ϥ� RefreshItem() �i��I�]���D�㪺��s : 
        �P���I�]�����Ҧ����A�ǥ� Inventory �� itemList ���s���͡A
        1.�ھ�Grid�����h�֤l����A�]�N�O�I�]�����h�֤��A�N��v���P���A�òM��slots���������
        2.�ھ�itemList�������ƶq�A�]�N�O�i�x�s�D�㪺�ƶq�A�v�����ͪťժ����A�N��[�J��slots���F
          slots�������]��Grid���l����A������檺�ƦC�F�����ťժ����s���A�H�Q����D�㪺�즲��աA
          �̫�ǰeitemList���U����ƨ�slots�U��椤�C
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
