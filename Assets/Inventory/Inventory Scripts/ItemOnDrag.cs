using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;    //�D��즲�e�b���@�Ӥ��
    public Iventory playerBag;          //�O���Ϊ��I�]
    public int currentItemIndex;        //�O���Ϊ��I�]��(playerBag)�D�㪺�s��

    //��δ�Щ즲��椤���D��ɡA�i�N�䲾�ʨ��L�Ū����A�Y��L���w���D��A�h��̤����F

    /*
        Item���@�Ӥ���Canvas Group�A�����̭���Blocks Raycasts�A�|�o�X�@�D�g�u�A�q��Юg���ù�����V�A
        �åi�� pointerCurrentRaycast�A�^�ǸI�쪺�Ĥ@�� UI�A�Y�DUI�h�^��null�F��eventData�b�o��N��ƹ����
    */

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //����Slot��originalParent
        currentItemIndex = originalParent.GetComponent<Slot>().slotIndex;   //���o���s��
        transform.SetParent(transform.parent.parent);   //�}�l�즲�ɪ�Item�]��Grid���l����
        transform.position = eventData.position;        //�}�l�즲�ɪ�Item��m�]����Ц�m
        GetComponent<CanvasGroup>().blocksRaycasts = false; //����blocksRaycasts�\��A�}�Үg�u
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
        //������UI����A��Y�^�ǭȤ���null
        if (eventData.pointerCurrentRaycast.gameObject != null)
        {
            //�����쩳�U�O�t�@�ӹD��
            if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
            {
                //�]�w�D�㪺�����󬰤��(Slot)�A��m����檺��m
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

                //�����O���έI�](playerBag)���D���T�A�Q��slotIndex = itemList�������s��
                //1.��e���b�즲���D��   2.�Q�즲�L�h����椺���D��A�N��̤���
                var temp = playerBag.itemList[currentItemIndex];
                playerBag.itemList[currentItemIndex] = playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotIndex];
                playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotIndex] = temp;

                //�]�w���U���D�㪺�����󬰭쥻�����(Slot)�A��m���쥻��檺��m
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }

            //�����쩳�U�O�Ū����
            if (eventData.pointerCurrentRaycast.gameObject.name == "Slot(Clone)")
            {
                //�]�w�D�㪺�����󬰪Ū����(Slot)�A��m���Ū���檺��m
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

                //�bitemList�s�W�o���D��A�����s�� = �ӹD��ҳB��檺�s��
                playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotIndex] = playerBag.itemList[currentItemIndex];

                //�Y�D��즲��A��^��������A�|�ɭP�D������A�]���u�����椣�P�ɡA�~����H�U�{���X
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotIndex != currentItemIndex)
                {
                    playerBag.itemList[currentItemIndex] = null;
                }

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
        }

        //��L���p�A�h�N�D��^�k��쥻����m
        transform.SetParent(originalParent);
        transform.position = originalParent.position;

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
