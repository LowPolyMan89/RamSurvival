using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Item item;
    [SerializeField] private int count;
    [SerializeField] ItemType ItemType;
    public Item Item { get => item; }
    public int Count { get => count; }

    public InventoryItem Set(Item initem, ItemType type, int incount)
    {
        ItemType = type;
        item = initem;
        count = incount;
        return this;
    }

    public void Add(int _count)
    {
        count += _count;
    }
}
