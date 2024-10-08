using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Inventory playerBag;

    /*
        若玩家觸碰到道具，也就是道具和玩家發生碰撞，將該道具加入到 itemList 中，
        並分為兩種狀況:
        1.道具不在 itemList 中，則將道具新增到 itemList
        2.道具已在 itemList 中，則將道具數量+1
        接著利用 itemList 中的紀錄，更新整個背包的道具種類和數量，每發生一次碰撞就更新一次，
        最後銷毀在場景中的物件，營造出道具被拾取的效果。
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
