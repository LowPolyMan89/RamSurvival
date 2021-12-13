using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Inventory : Entity
{
    public List<ItemView> Items = new List<ItemView>();
    public bool IsPlayerInventory;
    public float CurrentCapacity { get; set; }
    public int Sloots;
    public float MaxMass;

    public void RemoveItem(string item)
    {
        ItemView i = GetItem(item);
        Items.Remove(i);
    }

    public void RemoveItem(ItemView item)
    {
        Items.Remove(item);
    }

    public void AddEqipItem(string id)
    {
        bool play = false;

        ItemDataSO data = DatabaseManager.GetItemData(id);

        play = TryToAddItem(id);

        if (!play)
        {
            return;
        }

        ItemView newitemv = new ItemView(id, 1);
        Items.Add(newitemv);
        Player.Instance.UiInventory.AddItem(newitemv);

    }

    public void MassRemoveItem(string id, int value)
    {
        int capacity = value;

        ItemView[] stacks = GetItemsStacks(id).ToArray();
        
        for (int i = 0; i < stacks.Length; i++)
        {
            if (capacity < 1)
            {
                break;
            }
            
            for (int j = capacity; j > 0;)
            {
                stacks[i].Count--;
                capacity--;
                j--;
                
                if (stacks[i].Count < 1)
                {
                    RemoveItem(stacks[i]);
                    break;
                }
            }
        }

    }

    public List<ItemView> GetItemsStacks(string id)
    {
        return Items.Where(item => item.ItemId == id).ToList();
    }

    public virtual void AddItem(string ItemId, int value)
    {
        int count = value;

        bool play = false;

        ItemView v = FindContainsItem(ItemId);

        ItemDataSO data = DatabaseManager.GetItemData(ItemId);

    
            if (v != null)
            {
                v.Count += value;
            }
            else
            {
                ItemView newitemv = new ItemView(ItemId, count);
                Items.Add(newitemv);
                if (IsPlayerInventory)
                {
                    Player.Instance.UiInventory.AddItem(newitemv);
                }
            }

        
    }
    
    public void AddItem(Item item)
    {
        int count = item.Count;

        bool play = false;

        ItemView v = FindContainsItem(item.ItemId);

        ItemDataSO data = DatabaseManager.GetItemData(item.ItemId);

        for (int i = 0; i < count; i++)
        {

            //play = TryToAddItem(item.ItemId);

           // if (!play)
           // {
              //  break;
           // }
            
            if (v != null)
            {
                item.Count--;
                v.Count++;
            }
            else
            {
                ItemView newitemv = new ItemView(item.ItemId, item.Count);
                Items.Add(newitemv);
                if (IsPlayerInventory)
                {
                    Player.Instance.UiInventory.AddItem(newitemv);
                    break;
                }
            }

        }

        if (play)
        {
            Destroy(item.gameObject);
        }

        if (item.Count == 0)
        {
            Destroy(item.gameObject);
        }

    }

    public ItemView FindContainsItem(string id)
    {
        foreach (var i in Items)
        {
            if (i.ItemId == id)
            {
                return i;
            }
        }

        return null;
    }
    

    public ItemView GetItem(string id)
    {
        foreach (var i in Items)
        {
            if (i.ItemId == id)
            {
                return i;
            }
        }

        return null;
    }

    public int GetContainsItemCount(string id)
    {
        int value = 0;

        foreach (var i in Items)
        {
            if (i.ItemId == id)
            {
                value += i.Count;
            }
        }

        return value;
    }


    public float GetCurrentInventoryMass()
    {
        float val = 0;
        Items.ForEach(x => { val += DatabaseManager.GetItemData(x.ItemId).GetStat("Mass") * x.Count; });
        return val;
    }

    public bool TryToAddItem(string id)
    {
        bool check = true;

        float curentMass = GetCurrentInventoryMass();
        float newItemMass = DatabaseManager.GetItemData(id).GetStat("Mass");

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

    public List<ItemView> GetItems()
    {
        return Items;
    }

    public float GetMaxInventoryMass()
    {
        if (IsPlayerInventory)
        {
            float value = Player.Instance.PlayerStats.MinimumMass + (Player.Instance.EqippedBackpack != null
                ? Player.Instance.EqippedBackpack.GetStat("MaxMass")
                : 0);
            return value;
        }
        else
        {
            return MaxMass;
        }
    }


}
