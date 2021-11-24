using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> Items = new List<Item>();
    public float CurrentCapacity { get; set; }


    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }
    
    public void AddItem(Item item)
    {
        int count = item.Count;

        bool play = false;
        
        Item v = FindContainsItemWithEmptyStack(item);

        
        for (int i = 0; i < count; i++)
        {

            play = TryToAddItem(item);
            
            if (!play)
            {
                break;
            }

            if (item.IsStack)
            {
                if (v != null)
                {
                    item.Count--;
                    v.Count++;
                }
                else
                {
                    Items.Add(item);
                    Player.Instance.UiInventory.AddItem(item);
                    break;
                }
            }
            else
            {
                Items.Add(item);
                Player.Instance.UiInventory.AddItem(item);
                break;
            }
            
        }
        
        if (play)
        {
            item.Visualize(false);
            item.transform.SetParent(transform);
        }

        if (item.Count == 0)
        {
            Destroy(item.gameObject);
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

    public Item FindContainsItemWithEmptyStack(Item item)
    {
        foreach (var i in Items)
        {
            if (i.ItemId == item.ItemId)
            {
                if (i.Count < i.MaxStack)
                {
                    return i;
                }
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
        float newItemMass = item.GetStat("Mass");

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
