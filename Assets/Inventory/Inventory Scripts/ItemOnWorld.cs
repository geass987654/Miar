using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Iventory playerInventory;

    /*
        �Y���aĲ�I��D��A�]�N�O�D��M���a�o�͸I���A�N�ӹD��[�J�� itemList ���A
        �ä�����ت��p:
        1.�D�㤣�b itemList ���A�h�N�D��s�W�� itemList
        2.�D��w�b itemList ���A�h�N�D��ƶq+1
        ���ۧQ�� itemList ���������A��s��ӭI�]���D������M�ƶq�A�C�o�ͤ@���I���N��s�@���A
        �̫�P���b������������A��y�X�D��Q�B�����ĪG�C
     */

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(this.gameObject);
        }
    }

    private void AddNewItem()
    {
        if (!playerInventory.itemList.Contains(thisItem))
        {
            //playerInventory.itemList.Add(thisItem);
            //IventoryManager.CreateNewItem(thisItem);

            for(int i = 0; i < playerInventory.itemList.Count; i++)
            {
                if (playerInventory.itemList[i] == null)
                {
                    playerInventory.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemNum ++;
        }

        IventoryManager.RefreshItem();
    }
}
