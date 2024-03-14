using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject essestialSlot;
    [SerializeField] private Inventory essential; //�����Ϊ��I�]
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = essestialSlot.transform.GetChild(1);
    }

    public void UseItem()
    {
        //�ݭnitem�b�I�]�����s��
        itemIndex = InventoryManager.GetCurrentItemIndex();

        //���o�������I�]����ơB�Q��������
        Item item = essential.itemList[itemIndex];
        Item switchedItem = essestialSlot.GetComponent<InventorySlot>().GetCurrentItem();
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);
        string itemName = item.itemName;

        //�N�䲾��D��˳��椤�Υ洫

        //�]�w�D����줤���D���T�B�Ϥ�
        essestialSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null)
        {
            //����Q���N�����Z���n�^��I�]
            choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            essential.itemList[itemIndex] = switchedItem;
        }
        else
        {
            //�M������I�]�����Ϥ��B�ƶq�B����
            choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
            choosedItem.GetChild(1).GetComponent<Text>().text = "";
            choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
            choosedItem.gameObject.SetActive(false);
            essential.itemList[itemIndex] = null;
        }

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeItem(itemName);
    }
}
