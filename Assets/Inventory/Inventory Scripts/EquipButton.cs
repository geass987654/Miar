using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private Inventory playerBag; //�����Ϊ��I�]
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
        //�ݭnitem�b�I�]�����s��
        itemIndex = InventoryManager.GetCurrentItemIndex();
        //Debug.Log("itemIndex" + itemIndex);

        //���o�������I�]����ơB�Q��������
        Item item = playerBag.itemList[itemIndex];
        Item switchedItem = weaponSlot.GetComponent<InventorySlot>().GetCurrentItem();
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);

        //�N�䲾��Z���˳��椤�Υ洫

        //�]�w�Z����줤���˳Ƹ�T�B�Ϥ�
        weaponSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null)
        {
            //����Q���N�����Z���n�^��I�]
            choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            playerBag.itemList[itemIndex] = switchedItem;
        }
        else
        {
            //�M������I�]�����Ϥ��B�ƶq�B����
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
