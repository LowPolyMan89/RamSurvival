using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items = new List<Item>();
    public float CurrentCapacity { get; set; }

    public void AddItem(Item item)
    {
        Item containsItem = FindContainsItem(item);
        
        if (TryToAddItem(item))
        {

            if (containsItem != null)
            {
                containsItem.Count += item.Count;
                print($@"Add new item to stack count: {item.Count} . Item: {item.ItemId}");
                Destroy(item.gameObject);
               
            }
            else
            {
                Items.Add(item);
                UIController.Instance.UiInventory.AddItem(item);
                item.Visualize(false);
                item.transform.SetParent(transform);
                print($@"Add new item count: {item.Count}. Item: {item.ItemId}");
            }
            
        }
        else
        {
            int value = GetItemCountToAdd(item);
            item.Count -= value;
            
            if (containsItem != null)
            {
                containsItem.Count += value;
            }
            else
            {
                Item newItem = Instantiate(item.Prefab).GetComponent<Item>();
                Items.Add(newItem);
                newItem.Count = value;
                UIController.Instance.UiInventory.AddItem(newItem);
                newItem.Visualize(false);
                newItem.transform.SetParent(transform);
            }
        }

    }

    public Item FindContainsItem(Item item)
    {
        foreach (var i in Items)
        {
            if (i.ItemId == item.ItemId)
            {
                return i;
            }
        }

        return null;
    }


    public float GetCurrentInventoryMass()
    {
        float val = 0;
        Items.ForEach(x => { val += x.GetStat("Mass") * x.Count; });
        return val;
    }
    
    public bool TryToAddItem(Item item)
    {
        bool check = true;
        
        float curentMass = GetCurrentInventoryMass();
        float newItemMass = item.GetStat("Mass") * item.Count;

        if (curentMass + newItemMass > GetMaxInventoryMass())
        {
            check = false;
        }
        
        return check;
    }
    

    public int GetItemCountToAdd(Item item)
    {

        float curentMass = GetCurrentInventoryMass();
        float newItemMass = 0;
        
        for (int i = 1; i < item.Count; i++)
        {
            newItemMass += item.GetStat("Mass");

            if (curentMass <= newItemMass)
                return i;
        }

        return 0;
    }

    public List<Item> GetItems()
    {
        return Items;
    }

    public float GetMaxInventoryMass()
    {
        float value = Player.Instance.PlayerStats.MinimumMass + (Player.Instance.EqippedBackpack != null ? Player.Instance.EqippedBackpack.GetStat("MaxMass") : 0);
        return value;
    }
}
