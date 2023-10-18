using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollect : MonoBehaviour
{
    public ItemType itemType;
    public int itemAmount;
    public string nameItem;
    public Sprite itemImage;
}
public enum ItemType
{
    Battery,
    List,
    Item2,
    Item3,
    Item4
}
