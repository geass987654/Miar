using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    [SerializeField] private GameObject weaponSlot;
    [SerializeField] private Inventory weapon;          //�����Ϊ��I�]
    [SerializeField] private GameObject slotGrid;
    [SerializeField] private Text itemInfo;
    [SerializeField] private int equipedItemIndex = -1;

    private Transform equipedItem;

    private void Awake()
    {
        equipedItem = weaponSlot.transform.GetChild(1);
    }

    public void EquipWeapon()
    {
        itemIndex = InventoryManager.GetCurrentItemIndex();                                   //�ݭnitem�b�I�]�����s��
        Item item = weapon.itemList[itemIndex];                                               //���o�������I�]�����
        Item switchedItem = weaponSlot.GetComponent<InventorySlot>().GetCurrentItem();        //��줤���Z��
        Transform choosedItem = slotGrid.transform.GetChild(itemIndex).GetChild(0);           //�I�]�����Z��

        //�]�w�Z����줤���˳Ƹ�T�B�Ϥ�
        weaponSlot.GetComponent<InventorySlot>().SetCurrentItem(item);
        equipedItem.GetComponent<Image>().sprite = item.itemImage;
        equipedItem.gameObject.SetActive(true);

        if (switchedItem != null) //���w���Z���A�N��I�]������Q��w���Z��
        {
            //�M��Q��w���Z���b���@��椤
            for(int i = 0; i < weapon.itemList.Count; i++)
            {
                if (weapon.itemList[i] != null && weapon.itemList[i].equiped)
                {
                    equipedItemIndex = i;
                    break;
                }
            }

            //�쥻���Z���Ѱ���w
            Transform lockedItem = slotGrid.transform.GetChild(equipedItemIndex).GetChild(0);     //�I�]���Q��w���Z��
            lockedItem.GetChild(0).GetComponent<Image>().color = Color.white;
            lockedItem.transform.parent.GetComponent<Slot>().equiped = false;
            weapon.itemList[equipedItemIndex].equiped = false;

            ////����Q���N�����Z���n�^��I�]
            //choosedItem.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            //choosedItem.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            //choosedItem.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            //weapon.itemList[itemIndex] = switchedItem;
        }
        //else
        //{
        //    //�M������I�]�����Ϥ��B�ƶq�B����
        //    choosedItem.GetChild(0).GetComponent<Image>().sprite = null;
        //    choosedItem.GetChild(1).GetComponent<Text>().text = "";
        //    choosedItem.transform.parent.GetComponent<Slot>().slotInfo = "";
        //    choosedItem.gameObject.SetActive(false);
        //    weapon.itemList[itemIndex] = null;
        //}
        
        //�˳ƤW�h���Z���n��w�A�]�t�ӪZ�������M�������I�]
        choosedItem.GetChild(0).GetComponent<Image>().color = Color.gray;
        choosedItem.transform.parent.GetComponent<Slot>().equiped = true;
        weapon.itemList[itemIndex].equiped = true;

        itemInfo.text = "";
        gameObject.SetActive(false);

        ActiveInventory.Instance.ChangeWeapon();
    }
}
