using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Inventory/Item")]

public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public int itemNum;
    public string itemAttribute;
    [TextArea]
    public string itemInfo;
    public bool equip;
    public WeaponInfo weaponInfo;
}
