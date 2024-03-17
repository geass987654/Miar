using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;   //�D��즲�e�b���@�Ӥ��
    public int currentItemIndex;       //�O���Ϊ��I�]��(playerBag)�D�㪺�s��
    public Inventory playerBag;        //�O���Ϊ��I�]

    //��δ�Щ즲��椤���D��ɡA�i�N�䲾�ʨ��L�Ū����A�Y��L���w���D��A�h��̤����F

    /*
        Item���@�Ӥ���Canvas Group�A�̭���Blocks Raycasts�A�|�o�X�@�D�g�u�A�q��Юg���ù�����V�A
        �åi�� pointerCurrentRaycast�A�^�ǸI�쪺�Ĥ@�� UI�A�Y�DUI�h�^��null�F��eventData�b�o��N��ƹ����
    */

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //����Slot��originalParent
        currentItemIndex = originalParent.GetComponent<Slot>().slotIndex;   //���o���s��

        GameObject itemCopy = Instantiate(transform.gameObject, originalParent.position, Quaternion.identity);
        itemCopy.transform.SetParent(originalParent);
        //itemCopy.name = transform.name;
        itemCopy.SetActive(false);

        transform.SetParent(originalParent.parent);   //�}�l�즲�ɪ�Item�]��Grid���l����
        transform.position = eventData.position;        //�}�l�즲�ɪ�Item��m�]����Ц�m
        GetComponent<CanvasGroup>().blocksRaycasts = false; //����blocksRaycasts�\��A�קK�Q�즲����UI����
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;     //�N�즲����Item��m�]����Ц�m
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    /*
        �Y�����즲�ɡA���U��UI����Item Image�A�]�N�O�n�\�񪺤�榳��L�D��A
        1.��e���D�㴫�쩳�U�����A������l����A��m�]�������U����m�F
        2.�b������(playerBag)���D���T�]�n�����A����RefreshItem()�ɡA�~�|��s���諸����
        3.���U���D���ܬ��쥻��檺�l����A���m�]�����쥻����m
        
        �Y�����즲�ɡA���U��UI����slot(Clone)�A�]�N�O�Ū����A
        1.��e���D�㪽�����Ū����A������l����A��e���D���m�]�O�Ū�����m
        2.�b������(playerBag)�A�Ū�����J��e�D�㪺��T�A�ӭ쥻�s��D�㪺���]��null

        �̫�A�}��blocksRaycasts�\��A�����g�u
    */

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject pointedGameObject = eventData.pointerCurrentRaycast.gameObject;

        //������UI����A��Y�^�ǭȤ���null
        if (pointedGameObject != null)
        {
            //�����쩳�U�O�t�@�ӹD��
            if(pointedGameObject.name == "Item Image")
            {
                //���U�n�Q�洫��item
                Transform switchedItem = pointedGameObject.transform.parent;

                //�즲��item�M���U��item�洫���e���A�]�t�Ϥ��B�C��B�ƶq�B��T�y�z�B�O�_�˳�
                Image choosedImage = transform.GetChild(0).GetComponent<Image>();
                Image switchedImage = pointedGameObject.GetComponent<Image>();

                Sprite tempSprite = choosedImage.sprite;
                choosedImage.sprite = switchedImage.sprite;
                switchedImage.sprite = tempSprite;

                Color tempColor = choosedImage.color;
                choosedImage.color = switchedImage.color;
                switchedImage.color = tempColor;

                Text choosedText = transform.GetChild(1).GetComponent<Text>();
                Text switchedText = switchedItem.GetChild(1).GetComponent<Text>();

                string tempNum = choosedText.text;
                choosedText.text = switchedText.text;
                switchedText.text = tempNum;

                Slot choosedSlot = originalParent.GetComponent<Slot>();
                Slot switchedSlot = switchedItem.parent.GetComponent<Slot>();

                string tempInfo = choosedSlot.slotInfo;
                choosedSlot.slotInfo = switchedSlot.slotInfo;
                switchedSlot.slotInfo = tempInfo;

                bool tempEquiped = originalParent.GetComponent<Slot>().equiped;
                originalParent.GetComponent<Slot>().equiped = switchedItem.parent.GetComponent<Slot>().equiped;
                switchedItem.parent.GetComponent<Slot>().equiped = tempEquiped;

                //�����O���έI�](playerBag)���D���T�A�Q��slotIndex = itemList�������s��
                //1.��e���b�즲���D��   2.�Q�즲�L�h����椺���D��A�N��̤���
                var temp = playerBag.itemList[currentItemIndex];
                playerBag.itemList[currentItemIndex] = playerBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex];
                playerBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] = temp;

                //�즲��item�^��쥻��slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //�P���ƥΪ�itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                InventoryManager.CleanItemInfo();
                InventoryManager.SetEquipBtnState(false);
                InventoryManager.SetUseBtnState(false);

                return;
            }

            //�����쩳�U�O�Ū����
            if (pointedGameObject.name == "WeaponSlot(Clone)" || pointedGameObject.name == "EssentialSlot(Clone)")
            {
                //���U���Ū�item
                Transform emptyItem = pointedGameObject.transform.GetChild(0);

                //�즲��item�M���U���Ū�item�洫���e���A�]�t�Ϥ��B�C��B�ƶq�B��T�y�z�B�O�_�˳�
                Image choosedImage = transform.GetChild(0).GetComponent<Image>();
                Image switchedImage = emptyItem.GetChild(0).GetComponent<Image>();

                Sprite tempSprite = choosedImage.sprite;
                choosedImage.sprite = switchedImage.sprite;
                switchedImage.sprite = tempSprite;

                Color tempColor = choosedImage.color;
                choosedImage.color = switchedImage.color;
                switchedImage.color = tempColor;

                Text choosedText = transform.GetChild(1).GetComponent<Text>();
                Text switchedText = emptyItem.GetChild(1).GetComponent<Text>();

                string tempNum = choosedText.text;
                choosedText.text = switchedText.text;
                switchedText.text = tempNum;

                Slot choosedSlot = originalParent.GetComponent<Slot>();
                Slot switchedSlot = emptyItem.parent.GetComponent<Slot>();

                string tempInfo = choosedSlot.slotInfo;
                choosedSlot.slotInfo = switchedSlot.slotInfo;
                switchedSlot.slotInfo = tempInfo;

                bool tempEquiped = originalParent.GetComponent<Slot>().equiped;
                originalParent.GetComponent<Slot>().equiped = emptyItem.parent.GetComponent<Slot>().equiped;
                emptyItem.parent.GetComponent<Slot>().equiped = tempEquiped;

                //�bitemList�s�W�o���D��A�����s�� = �ӹD��ҳB��檺�s��
                playerBag.itemList[pointedGameObject.GetComponent<Slot>().slotIndex] = playerBag.itemList[currentItemIndex];

                //�Y�D��즲��A��^��������A�|�ɭP�D������A�]���u�����椣�P�ɡA�~����H�U�{���X
                if (pointedGameObject.GetComponent<Slot>().slotIndex != currentItemIndex)
                {
                    playerBag.itemList[currentItemIndex] = null;

                    //���U���Ū�item��J���e���A�ӭ����item�ܦ��Ū�
                    emptyItem.gameObject.SetActive(true);
                    transform.gameObject.SetActive(false);
                }

                //�즲��item�^��쥻��slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //�P���ƥΪ�itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                
                InventoryManager.CleanItemInfo();
                InventoryManager.SetEquipBtnState(false);
                InventoryManager.SetUseBtnState(false);

                return;
            }

            //if (pointedItem.name == "CooldownMaskWeapon")
            //{
            //    //Debug.Log("success");

            //    Transform inventorySlot = pointedItem.transform.parent;
            //    Transform equipedItem = inventorySlot.GetChild(1);
            //    Item item = playerBag.itemList[currentItemIndex];
            //    Item switchedItem = inventorySlot.GetComponent<InventorySlot>().GetCurrentItem();

            //    //�]�w�즲�����󪺤����󬰭�������(Slot)�A��m���������m
            //    transform.SetParent(originalParent);
            //    transform.position = originalParent.position;

            //    //�]�w�Z����줤���˳Ƹ�T�B�Ϥ�
            //    inventorySlot.GetComponent<InventorySlot>().SetCurrentItem(item);
            //    equipedItem.GetComponent<Image>().sprite = item.itemImage;
            //    equipedItem.gameObject.SetActive(true);

            //    if(switchedItem != null)
            //    {
            //        //����Q���N�����Z���n�^��I�]
            //        transform.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
            //        transform.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
            //        transform.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
            //        playerBag.itemList[currentItemIndex] = switchedItem;
            //    }
            //    else
            //    {
            //        //�M������I�]�����Ϥ��B�ƶq�B����
            //        transform.GetChild(0).GetComponent<Image>().sprite = null;
            //        transform.GetChild(1).GetComponent<Text>().text = "";
            //        transform.transform.parent.GetComponent<Slot>().slotInfo = "";
            //        transform.gameObject.SetActive(false);
            //        playerBag.itemList[currentItemIndex] = null;
            //    }

            //    ActiveInventory.Instance.ChangeWeapon();

            //    GetComponent<CanvasGroup>().blocksRaycasts = true;

            //    InventoryManager.CleanItemInfo();
            //    InventoryManager.SetEquipBtnState(false);

            //    //Debug.Log("done");
            //    return;
            //}
        }

        //��L���p�A�h�N�D��^�k��쥻����m
        transform.SetParent(originalParent);
        transform.position = originalParent.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
