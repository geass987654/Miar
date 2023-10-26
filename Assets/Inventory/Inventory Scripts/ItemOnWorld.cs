using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerBag;

    /*
        �Y���aĲ�I��D��A�]�N�O�D��M���a�o�͸I���A�N�ӹD��[�J�� itemList ���A
        �ä�����ت��p:
        1.�D�㤣�b itemList ���A�h�N�D��s�W�� itemList
        2.�D��w�b itemList ���A�h�N�D��ƶq+1
        ���ۧQ�� itemList ���������A��s��ӭI�]���D������M�ƶq�A�C�o�ͤ@���I���N��s�@���A
        �̫�P���b������������A��y�X�D��Q�B�����ĪG�C
     */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(this.gameObject);
        }
    }

    private void AddNewItem()
    {
        if (!playerBag.itemList.Contains(thisItem))
        {
            //playerBag.itemList.Add(thisItem);
            //InventoryManager.CreateNewItem(thisItem);

            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i] == null)
                {
                    playerBag.itemList[i] = thisItem;
                    break;
                }
            }
        }
        else
        {
            thisItem.itemNum ++;
        }

        InventoryManager.RefreshItem();
    }
}
