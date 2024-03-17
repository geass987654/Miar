using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject essestialSlot;
    [SerializeField] private Inventory essential;           //�����Ϊ��I�]
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;
    [SerializeField] private int equipedItemIndex = -1;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = essestialSlot.transform.GetChild(1);
    }

    public void UseItem()
    {
        itemIndex = InventoryManager.GetCurrentItemIndex();                                   //�ݭnitem�b�I�]�����s��
        Item item = essential.itemList[itemIndex];                                            //���o�������I�]�����
        Item switchedItem = essestialSlot.GetComponent<InventorySlot>().GetCurrentItem();     //��줤���D��
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);           //�I�]�����D��
        string itemName = item.itemName;

        //�]�w�D����줤���D���T�B�Ϥ�
        essestialSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null)
        {
            //�M��Q��w���D��b���@��椤
            for (int i = 0; i < essential.itemList.Count; i++)
            {
                if (essential.itemList[i] != null && essential.itemList[i].equiped)
                {
                    equipedItemIndex = i;
                    break;
                }
            }

            //�쥻���D��Ѱ���w
            Transform lockedItem = slotGrid.transform.GetChild(equipedItemIndex).GetChild(0);     //�I�]���Q��w���D��
            lockedItem.GetChild(0).GetComponent<Image>().color = Color.white;
            lockedItem.transform.parent.GetComponent<Slot>().equiped = false;
            essential.itemList[equipedItemIndex].equiped = false;

            ////����Q���N�����D��n�^��I�]
            //choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            //choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            //choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            //essential.itemList[itemIndex] = switchedItem;
        }
        //else
        //{
        //    //�M������I�]�����Ϥ��B�ƶq�B����
        //    choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
        //    choosedItem.GetChild(1).GetComponent<Text>().text = "";
        //    choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
        //    choosedItem.gameObject.SetActive(false);
        //    essential.itemList[itemIndex] = null;
        //}
        
        //�˳ƤW�h���D��n��w�A�]�t�ӹD�㪺���M�������I�]
        choosedItem.GetChild(0).GetComponent<Image>().color = Color.gray;
        choosedItem.transform.parent.GetComponent<Slot>().equiped = true;
        essential.itemList[itemIndex].equiped = true;

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeItem(itemName);
    }
}
