using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Iventory", menuName ="Iventory/New Iventory")]

public class Iventory : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
