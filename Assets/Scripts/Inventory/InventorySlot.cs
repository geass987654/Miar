using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponInfo;
    [SerializeField] private Item currentItem;

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public Item GetCurrentItem()
    {
        return currentItem;
    }

    public void SetCurrentItem(Item item)
    {
        if(item == null)
        {
            currentItem = null;
            weaponInfo = null;
            return;
        }

        currentItem = item;
        weaponInfo = item.weaponInfo;
    }
}
