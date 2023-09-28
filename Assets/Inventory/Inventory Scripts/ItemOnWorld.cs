using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Iventory playerInventory;

    /*
        若玩家觸碰到道具，也就是道具和玩家發生碰撞，將該道具加入到 itemList 中，
        並分為兩種狀況:
        1.道具不在 itemList 中，則將道具新增到 itemList
        2.道具已在 itemList 中，則將道具數量+1
        接著利用 itemList 中的紀錄，更新整個背包的道具種類和數量，每發生一次碰撞就更新一次，
        最後銷毀在場景中的物件，營造出道具被拾取的效果。
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
