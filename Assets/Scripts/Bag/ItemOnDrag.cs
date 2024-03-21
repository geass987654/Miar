using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;   //道具拖曳前在哪一個方格
    public int currentItemIndex;       //記錄用的背包中(originalBag)道具的編號
    public Inventory originalBag;        //記錄用的背包
    public Inventory newBag;


    //當用游標拖曳方格中的道具時，可將其移動到其他空的方格，若其他方格已有道具，則兩者互換；

    /*
        Item有一個元件Canvas Group，裡面的Blocks Raycasts，會發出一道射線，從游標射往螢幕的方向，
        並可用 pointerCurrentRaycast，回傳碰到的第一個 UI，若非UI則回傳null；而eventData在這邊代表滑鼠游標
    */

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;  //紀錄Slot為originalParent
        currentItemIndex = originalParent.GetComponent<Slot>().slotIndex;   //取得方格編號

        GameObject itemCopy = Instantiate(transform.gameObject, originalParent.position, Quaternion.identity);
        itemCopy.transform.SetParent(originalParent);
        //itemCopy.name = transform.name;
        itemCopy.SetActive(false);

        transform.SetParent(originalParent.parent.parent.parent);    //開始拖曳時的Item設為僅次於canvas的子物件
        transform.position = eventData.position;                     //開始拖曳時的Item位置設為游標位置
        GetComponent<CanvasGroup>().blocksRaycasts = false;          //關閉blocksRaycasts功能，避免被拖曳中的UI阻擋
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;     //將拖曳中的Item位置設為游標位置
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    /*
        若結束拖曳時，底下的UI物件為Item Image，也就是要擺放的方格有其他道具，
        1.當前的道具換到底下的方格，成為其子物件，位置也換成底下的位置；
        2.在紀錄中(originalBag)的道具資訊也要互換，後續RefreshItem()時，才會更新成對的順序
        3.底下的道具變為原本方格的子物件，其位置也換成原本的位置
        
        若結束拖曳時，底下的UI物件為slot(Clone)，也就是空的方格，
        1.當前的道具直接放到空的方格，成為其子物件，當前的道具位置也是空的方格位置
        2.在紀錄中(originalBag)，空的方格放入當前道具的資訊，而原本存放道具的方格設為null

        最後，開啟blocksRaycasts功能，關閉射線
    */

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject pointedGameObject = eventData.pointerCurrentRaycast.gameObject;

        //偵測到UI物件，亦即回傳值不為null
        if (pointedGameObject != null)
        {
            //偵測到底下是另一個道具
            if(pointedGameObject.name == "Item Image")
            {
                //從繼承箱無法拖曳到冷卻中的item上
                if(originalBag.name == "WeaponInherited" || originalBag.name == "EssentialInherited")
                {    
                    if (newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] != null
                        && newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex].isCooldown)
                    {
                        //從繼承箱拖到冷卻中的item上、或是冷卻中的item拖進繼承箱，將道具回歸到原本的位置
                        transform.SetParent(originalParent);
                        transform.position = originalParent.position;

                        //銷毀備用的itemCopy
                        Destroy(originalParent.GetChild(0).gameObject);

                        GetComponent<CanvasGroup>().blocksRaycasts = true;

                        return;
                    }
                }

                //底下要被交換的item
                Transform switchedItem = pointedGameObject.transform.parent;

                //拖曳的item和底下的item交換內容物，包含圖片、顏色、數量、資訊描述、是否裝備
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

                if (originalParent.gameObject.name.Contains("_Box")) //繼承箱格子拖到背包格子
                {
                    if (pointedGameObject.GetComponentInParent<Slot>().equiped)
                    {
                        pointedGameObject.GetComponentInParent<Slot>().equiped = false;

                        newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex].equiped = false;

                        choosedImage.color = Color.white;
                        switchedImage.color = Color.white;

                        if (originalBag.name == "WeaponInherited")
                        {
                            ActiveInventory.Instance.RemoveWeapon();
                        }
                        else
                        {
                            ActiveInventory.Instance.RemoveItem();
                        }
                    }

                    //互換記錄用背包(originalBag)的道具資訊，利用slotIndex = itemList的元素編號
                    //1.當前正在拖曳的道具   2.想拖曳過去的方格內的道具，將兩者互換
                    var temp = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex];
                    newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] = temp;
                }
                else //背包格子拖到背包格子
                {
                    bool tempEquiped = originalParent.GetComponent<Slot>().equiped;
                    originalParent.GetComponent<Slot>().equiped = switchedItem.parent.GetComponent<Slot>().equiped;
                    switchedItem.parent.GetComponent<Slot>().equiped = tempEquiped;

                    //互換記錄用背包(originalBag)的道具資訊，利用slotIndex = itemList的元素編號
                    //1.當前正在拖曳的道具   2.想拖曳過去的方格內的道具，將兩者互換
                    var temp = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = originalBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex];
                    originalBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] = temp;
                }

                //拖曳的item回到原本的slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //銷毀備用的itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                InventoryManager.CleanItemInfo();
                InventoryManager.SetEquipBtnState(false);
                InventoryManager.SetUseBtnState(false);

                return;
            }

            //偵測到底下是空的方格
            if (pointedGameObject.name == "WeaponSlot(Clone)" || pointedGameObject.name == "EssentialSlot(Clone)")
            {
                //底下方格空的item
                Transform emptyItem = pointedGameObject.transform.GetChild(0);

                //拖曳的item和底下方格空的item交換內容物，包含圖片、顏色、數量、資訊描述、是否裝備
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

                if (originalParent.gameObject.name.Contains("_Box")) //繼承箱格子拖到背包格子
                {
                    //在itemList新增這項道具，元素編號 = 該道具所處方格的編號
                    newBag.itemList[pointedGameObject.GetComponent<Slot>().slotIndex] = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = null;

                    //底下方格空的item放入內容物，而原先的item變成空的
                    emptyItem.gameObject.SetActive(true);
                    transform.gameObject.SetActive(false);
                }
                else //背包格子拖到背包格子
                {
                    bool tempEquiped = originalParent.GetComponent<Slot>().equiped;
                    originalParent.GetComponent<Slot>().equiped = emptyItem.parent.GetComponent<Slot>().equiped;
                    emptyItem.parent.GetComponent<Slot>().equiped = tempEquiped;

                    //在itemList新增這項道具，元素編號 = 該道具所處方格的編號
                    originalBag.itemList[pointedGameObject.GetComponent<Slot>().slotIndex] = originalBag.itemList[currentItemIndex];

                    //若道具拖曳後再放回原先的方格，會導致道具消失，因此只有當方格不同時，才執行以下程式碼
                    if (pointedGameObject.GetComponent<Slot>().slotIndex != currentItemIndex)
                    {
                        originalBag.itemList[currentItemIndex] = null;

                        //底下方格空的item放入內容物，而原先的item變成空的
                        emptyItem.gameObject.SetActive(true);
                        transform.gameObject.SetActive(false);
                    }
                }

                //拖曳的item回到原本的slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //銷毀備用的itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;
                
                InventoryManager.CleanItemInfo();
                InventoryManager.SetEquipBtnState(false);
                InventoryManager.SetUseBtnState(false);

                return;
            }

            if (pointedGameObject.name == "Item Image_Box")
            {
                if (originalBag.itemList[currentItemIndex].isCooldown)
                {
                    //冷卻中拖進繼承箱，將道具回歸到原本的位置
                    transform.SetParent(originalParent);
                    transform.position = originalParent.position;
                    
                    //銷毀備用的itemCopy
                    Destroy(originalParent.GetChild(0).gameObject);

                    GetComponent<CanvasGroup>().blocksRaycasts = true;

                    return;
                }

                //底下要被交換的item
                Transform switchedItem = pointedGameObject.transform.parent;

                //拖曳的item和底下的item交換內容物，包含圖片、顏色、數量、資訊描述、是否裝備
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

                if (originalParent.gameObject.name.Contains("_Box")) //繼承箱格子拖到繼承箱格子
                {
                    //互換記錄用背包(originalBag)的道具資訊，利用slotIndex = itemList的元素編號
                    //1.當前正在拖曳的道具   2.想拖曳過去的方格內的道具，將兩者互換
                    var temp = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = originalBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex];
                    originalBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] = temp;
                }
                else //背包格子拖到繼承箱格子
                {
                    if (originalParent.GetComponent<Slot>().equiped)
                    {
                        originalParent.GetComponent<Slot>().equiped = false;
                        switchedItem.parent.GetComponent<Slot>().equiped = false;

                        originalBag.itemList[currentItemIndex].equiped = false;

                        choosedImage.color = Color.white;
                        switchedImage.color = Color.white;

                        if (originalBag.name == "Weapon")
                        {
                            ActiveInventory.Instance.RemoveWeapon();
                        }
                        else
                        {
                            ActiveInventory.Instance.RemoveItem();
                        }
                    }

                    //互換記錄用背包(originalBag)的道具資訊，利用slotIndex = itemList的元素編號
                    //1.當前正在拖曳的道具   2.想拖曳過去的方格內的道具，將兩者互換
                    var temp = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex];
                    newBag.itemList[pointedGameObject.GetComponentInParent<Slot>().slotIndex] = temp;
                }

                //拖曳的item回到原本的slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //銷毀備用的itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                return;
            }

            if (pointedGameObject.name == "WeaponSlot_Box(Clone)" || pointedGameObject.name == "EssentialSlot_Box(Clone)")
            {
                if (originalBag.itemList[currentItemIndex].isCooldown)
                {
                    //冷卻中拖進繼承箱，將道具回歸到原本的位置
                    transform.SetParent(originalParent);
                    transform.position = originalParent.position;

                    //銷毀備用的itemCopy
                    Destroy(originalParent.GetChild(0).gameObject);

                    GetComponent<CanvasGroup>().blocksRaycasts = true;

                    return;
                }

                //底下方格空的item
                Transform emptyItem = pointedGameObject.transform.GetChild(0);

                //拖曳的item和底下方格空的item交換內容物，包含圖片、顏色、數量、資訊描述、是否裝備
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

                if (originalParent.gameObject.name.Contains("_Box")) //繼承箱格子拖到繼承箱格子
                {
                    //在itemList新增這項道具，元素編號 = 該道具所處方格的編號
                    originalBag.itemList[pointedGameObject.GetComponent<Slot>().slotIndex] = originalBag.itemList[currentItemIndex];

                    //若道具拖曳後再放回原先的方格，會導致道具消失，因此只有當方格不同時，才執行以下程式碼
                    if (pointedGameObject.GetComponent<Slot>().slotIndex != currentItemIndex)
                    {
                        originalBag.itemList[currentItemIndex] = null;

                        //底下方格空的item放入內容物，而原先的item變成空的
                        emptyItem.gameObject.SetActive(true);
                        transform.gameObject.SetActive(false);
                    }
                }
                else //背包格子拖到繼承箱格子
                {
                    if (originalParent.GetComponent<Slot>().equiped)
                    {
                        originalParent.GetComponent<Slot>().equiped = false;
                        emptyItem.parent.GetComponent<Slot>().equiped = false;

                        originalBag.itemList[currentItemIndex].equiped = false;

                        choosedImage.color = Color.white;
                        switchedImage.color = Color.white;

                        if(originalBag.name == "Weapon")
                        {
                            ActiveInventory.Instance.RemoveWeapon();
                        }
                        else
                        {
                            ActiveInventory.Instance.RemoveItem();
                        }
                    }

                    //在itemList新增這項道具，元素編號 = 該道具所處方格的編號
                    newBag.itemList[pointedGameObject.GetComponent<Slot>().slotIndex] = originalBag.itemList[currentItemIndex];
                    originalBag.itemList[currentItemIndex] = null;

                    //底下方格空的item放入內容物，而原先的item變成空的
                    emptyItem.gameObject.SetActive(true);
                    transform.gameObject.SetActive(false);
                }

                //拖曳的item回到原本的slot
                transform.SetParent(originalParent);
                transform.position = originalParent.position;

                //銷毀備用的itemCopy
                Destroy(originalParent.GetChild(0).gameObject);

                GetComponent<CanvasGroup>().blocksRaycasts = true;

                return;
            }

            //if (pointedItem.name == "CooldownMaskWeapon")
            //{
            //    //Debug.Log("success");

                //    Transform inventorySlot = pointedItem.transform.parent;
                //    Transform equipedItem = inventorySlot.GetChild(1);
                //    Item item = originalBag.itemList[currentItemIndex];
                //    Item switchedItem = inventorySlot.GetComponent<InventorySlot>().GetCurrentItem();

                //    //設定拖曳中物件的父物件為原先的方格(Slot)，位置為原先的位置
                //    transform.SetParent(originalParent);
                //    transform.position = originalParent.position;

                //    //設定武器欄位中的裝備資訊、圖片
                //    inventorySlot.GetComponent<InventorySlot>().SetCurrentItem(item);
                //    equipedItem.GetComponent<Image>().sprite = item.itemImage;
                //    equipedItem.gameObject.SetActive(true);

                //    if(switchedItem != null)
                //    {
                //        //原先被取代掉的武器要回到背包
                //        transform.GetChild(0).GetComponent<Image>().sprite = switchedItem.itemImage;
                //        transform.GetChild(1).GetComponent<Text>().text = switchedItem.itemNum.ToString();
                //        transform.transform.parent.GetComponent<Slot>().slotInfo = switchedItem.itemInfo;
                //        originalBag.itemList[currentItemIndex] = switchedItem;
                //    }
                //    else
                //    {
                //        //清除原先背包中的圖片、數量、紀錄
                //        transform.GetChild(0).GetComponent<Image>().sprite = null;
                //        transform.GetChild(1).GetComponent<Text>().text = "";
                //        transform.transform.parent.GetComponent<Slot>().slotInfo = "";
                //        transform.gameObject.SetActive(false);
                //        originalBag.itemList[currentItemIndex] = null;
                //    }

                //    ActiveInventory.Instance.ChangeWeapon();

                //    GetComponent<CanvasGroup>().blocksRaycasts = true;

                //    InventoryManager.CleanItemInfo();
                //    InventoryManager.SetEquipBtnState(false);

                //    //Debug.Log("done");
                //    return;
                //}
        }

        //其他情況，則將道具回歸到原本的位置
        transform.SetParent(originalParent);
        transform.position = originalParent.position;
        
        //銷毀備用的itemCopy
        Destroy(originalParent.GetChild(0).gameObject);

        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
