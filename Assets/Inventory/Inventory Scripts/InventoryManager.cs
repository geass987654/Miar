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
    //static InventoryManager instance;
    //public Inventory playerBag;      //�����I�]�����D��
    //public GameObject slotGrid;     //���ƦC
    //public GameObject emptySlot;    //�Ū����
    //public Text itemInfo;           //�D���T
    //public List<GameObject> slots = new List<GameObject>(); //�x�s�I�]�����D��

    static InventoryManager instance;
    public Inventory equipmentBag, essentialBag, chipBag;              //�����I�]�����D��
    public GameObject equipment, essential, chip;                      //���ƦC
    public GameObject emptySlot;                                       //�Ū����
    public Text itemInfo;                                              //�D���T
    public List<GameObject> equipmentSlots = new List<GameObject>();   //�x�s�I�]�����D��
    public List<GameObject> essentialSlots = new List<GameObject>();   //�x�s�I�]�����D��
    public List<GameObject> chipSlots = new List<GameObject>();        //�x�s�I�]�����D��

    public Inventory shortCutBarBag;
    public GameObject shortCutBar;
    public List<GameObject> shortCutBarSlots = new List<GameObject>();

    //static InventoryManager instance;
    //public Inventory[] playerBag = new Inventory[3];             //�����I�]�����D��
    //public GameObject[] slotGrid = new GameObject[3];            //���ƦC
    //public GameObject emptySlot;                                 //�Ū����
    //public Text itemInfo;                                        //�D���T
    //public List<GameObject>[] slots = new List<GameObject>[3];   //�x�s�I�]�����D��

    //public Slot slotPrefab;         //�I�]�������̪��D��


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

    private void OnEnable()
    {
        RefreshItem();
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
        �b�C������e�AInventory���O��playerBag����itemList�A�w�ǳƦn�i�x�s�D�㪺�Ŷ�(�w�]���ƶq��18)�F
        �C���}�l��A�@������I������������D��A�Ҧp Sword �� Shoes�A�N��[�J��itemList�F
        ��I�]�}�Ү�(���U��L��'B')�A�ϥ� RefreshItem() �i��I�]���D�㪺��s : 
        �P���I�]�����Ҧ����A�ǥ� Inventory �� itemList ���s���͡A
        1.�ھ�Grid�����h�֤l����A�]�N�O�I�]�����h�֤��A�N��v���P���A�òM��slots���������
        2.�ھ�itemList�������ƶq�A�]�N�O�i�x�s�D�㪺�ƶq�A�v�����ͪťժ����A�N��[�J��slots���F
          slots�������]��Grid���l����A������檺�ƦC�F�����ťժ����s���A�H�Q����D�㪺�즲��աA
          �̫�ǰeitemList���U����ƨ�slots�U��椤�C
    */

    public static void RefreshItem()
    {
        RefreshItemOnEquipment();
        RefreshItemOnEssential();
        RefreshItemOnChip();
        RefreshItemOnShortCurBar();
    }

    public static void RefreshItemOnEquipment()
    {
        //Debug.Log("equipment start");
        for (int i = 0; i < instance.equipment.transform.childCount; i++)
        {
            if (instance.equipment.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.equipment.transform.GetChild(i).gameObject);
            //Debug.Log("equipment destroy" + i);
            instance.equipmentSlots.Clear();
        }

        for (int i = 0; i < instance.equipmentBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.playerBag.itemList[i]);
            instance.equipmentSlots.Add(Instantiate(instance.emptySlot));
            instance.equipmentSlots[i].transform.SetParent(instance.equipment.transform);
            instance.equipmentSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.equipmentSlots[i].GetComponent<Slot>().SetupSlot(instance.equipmentBag.itemList[i]);
            //Debug.Log("equipment instantiate" + i);

            instance.equipment.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.equipmentBag;
        }
    }
    public static void RefreshItemOnEssential()
    {
        //Debug.Log("essential start");
        for (int i = 0; i < instance.essential.transform.childCount; i++)
        {
            if (instance.essential.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.essential.transform.GetChild(i).gameObject);
            //Debug.Log("essential destroy" + i);
            instance.essentialSlots.Clear();
        }

        for (int i = 0; i < instance.essentialBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.playerBag.itemList[i]);
            instance.essentialSlots.Add(Instantiate(instance.emptySlot));
            instance.essentialSlots[i].transform.SetParent(instance.essential.transform);
            instance.essentialSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.essentialSlots[i].GetComponent<Slot>().SetupSlot(instance.essentialBag.itemList[i]);
            //Debug.Log("essential instantiate" + i);
            instance.essential.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.essentialBag;
        }
    }
    public static void RefreshItemOnChip()
    {
        for (int i = 0; i < instance.chip.transform.childCount; i++)
        {
            if (instance.chip.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.chip.transform.GetChild(i).gameObject);
            instance.chipSlots.Clear();
        }

        for (int i = 0; i < instance.chipBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.playerBag.itemList[i]);
            instance.chipSlots.Add(Instantiate(instance.emptySlot));
            instance.chipSlots[i].transform.SetParent(instance.chip.transform);
            instance.chipSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.chipSlots[i].GetComponent<Slot>().SetupSlot(instance.chipBag.itemList[i]);
            instance.chip.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.chipBag;
        }
    }
    public static void RefreshItemOnShortCurBar()
    {
        Debug.Log("ShortCutBar start");
        for (int i = 0; i < instance.shortCutBar.transform.childCount; i++)
        {
            if (instance.shortCutBar.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.shortCutBar.transform.GetChild(i).gameObject);
            Debug.Log("ShortCutBar destroy" + i);
            instance.shortCutBarSlots.Clear();
        }

        for (int i = 0; i < instance.shortCutBarBag.itemList.Count; i++)
        {
            //CreateNewItem(instance.playerBag.itemList[i]);
            instance.shortCutBarSlots.Add(Instantiate(instance.emptySlot));
            instance.shortCutBarSlots[i].transform.SetParent(instance.shortCutBar.transform);
            instance.shortCutBarSlots[i].GetComponent<Slot>().slotIndex = i;
            instance.shortCutBarSlots[i].GetComponent<Slot>().SetupSlot(instance.shortCutBarBag.itemList[i]);
            Debug.Log("ShortCutBar instantiate" + i);

            instance.shortCutBar.transform.GetChild(i).transform.GetChild(0).GetComponent<ItemOnDrag>().playerBag = instance.shortCutBarBag;
        }
    }

    public static void UpdateItemInfo(string itemDescription)
    {
        instance.itemInfo.text = itemDescription;
    }
}
