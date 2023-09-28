using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;    //道具拖曳前在哪一個方格
    public Iventory playerBag;          //記錄用的背包
    public int currentItemIndex;        //記錄用的背包中(playerBag)道具的編號

    //當用游標拖曳方格中的道具時，可將其移動到其他空的方格，若其他方格已有道具，則兩者互換；

    /*
        Item有一個元件Canvas Group，關閉裡面的Blocks Raycasts，會發出一道射線，從游標射往螢幕的方向，
        並可用 pointerCurrentRaycast，回傳碰到的第一個 UI；而eventData在這邊代表滑鼠游標
    */

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //紀錄Slot為originalParent
        currentItemIndex = originalParent.GetComponent<Slot>().slotIndex;   //取得方格編號
        transform.SetParent(transform.parent.parent);   //開始拖曳時的Item設為Grid的子物件
        transform.position = eventData.position;        //開始拖曳時的Item位置設為游標位置
        GetComponent<CanvasGroup>().blocksRaycasts = false; //關閉blocksRaycasts功能，開啟射線
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;     //將拖曳中的Item位置設為游標位置
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    /*
        若結束拖曳時，底下的UI物件為Item Image，也就是要擺放的方格有其他道具，
        1.當前的道具換到底下的方格，成為其子物件，位置也換成底下的位置；
        2.在紀錄中(playerBag)的道具資訊也要互換，後續RefreshItem()時，才會更新成對的順序
        3.底下的道具變為原本方格的子物件，其位置也換成原本的位置
        
        若結束拖曳時，底下是空的方格，
        1.當前的道具直接放到空的方格，成為其子物件，當前的道具位置也是空的方格位置
        2.在紀錄中(playerBag)，空的方格放入當前道具的資訊，而原本存放道具的方格設為null

        最後，開啟blocksRaycasts功能，關閉射線
    */

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "Item Image")
        {
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;

            var temp = playerBag.itemList[currentItemIndex];
            playerBag.itemList[currentItemIndex] = playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotIndex];
            playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().slotIndex] = temp;

            eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            return;
        }

        transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
        transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;

        playerBag.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponent<Slot>().slotIndex] = playerBag.itemList[currentItemIndex];
        playerBag.itemList[currentItemIndex] = null;


        GetComponent<CanvasGroup>().blocksRaycasts = true;

    }
}
