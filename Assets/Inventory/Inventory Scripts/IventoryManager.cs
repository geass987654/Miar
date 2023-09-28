using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IventoryManager : MonoBehaviour
{
    /*
        �I�]�t�� : Hierarchy��������A�ѤW�ܤU�̧Ǭ��A�I�](PlayerBag)�B���ƦC(Grid)�B���(Slot)�B
        �D��(Item)�B�D��Ϥ��μƶq(Item Image�BItem Number)�F
        �x�s��ƥΨ�ӥi�s�誫�� : Item(��ܹD��)�BIventory(��ܭI�]�A�Υi�������D�㪺�x�s�w)
    */
    static IventoryManager instance;

    public Iventory playerBag;      //�����I�]�����D��
    public GameObject slotGrid;     //���ƦC
    public GameObject emptySlot;    //�Ū����
    public Text itemInfo;           //�D���T
    public List<GameObject> slots = new List<GameObject>(); //�x�s�I�]�����D��

    //public Slot slotPrefab;         //�I�]�������̪��D��

    
    private void Awake()
    {
        /*
            singleton(��ҼҦ�)�A�T�OIventoryManager�u�|���@�ӹ��
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
        �b�C������e�AIventory���O��playerBag����itemList�A�w�ǳƦn�i�x�s�D�㪺�Ŷ�(�w�]���ƶq��18)�F
        �C���}�l��A�@������I������������D��A�Ҧp Sword �� Shoes�A�N��[�J��itemList�F
        ��I�]�}�Ү�(���U��L��'B')�A�ϥ� RefreshItem() �i��I�]���D�㪺��s : 
        �P���I�]�����Ҧ����A�ǥ� Iventory �� itemList ���s���͡A
        1.�ھ�Grid�����h�֤l����A�]�N�O�I�]�����h�֤��A�N��v���P���A�òM��slots���������
        2.�ھ�itemList�������ƶq�A�]�N�O�i�x�s�D�㪺�ƶq�A�v�����ͪťժ����A�N��[�J��slots���F
          slots�������]��Grid���l����A������檺�ƦC�F�����ťժ����s���A�H�Q����D�㪺�즲��աA
          �̫�ǰeitemList���U����ƨ�slots�U��椤�C
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
