using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemView> Items = new List<ItemView>();
    public float CurrentCapacity { get; set; }


    public void RemoveItem(string item)
    {
        ItemView i = GetItem(item);
        Items.Remove(i);
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


    public void AddItem(Item item)
    {
        int count = item.Count;

        bool play = false;

        ItemView v = FindContainsItemWithEmptyStack(item.ItemId);

        ItemDataSO data = DatabaseManager.GetItemData(item.ItemId);

        for (int i = 0; i < count; i++)
        {

            play = TryToAddItem(item.ItemId);

            if (!play)
            {
                break;
            }

            if (data.IsStack)
            {
                if (v != null)
                {
                    item.Count--;
                    v.Count++;
                }
                else
                {
                    ItemView newitemv = new ItemView(item.ItemId, item.Count);
                    Items.Add(newitemv);
                    Player.Instance.UiInventory.AddItem(newitemv);
                    break;
                }
            }
            else
            {
                ItemView newitemv = new ItemView(item.ItemId, item.Count);
                Items.Add(newitemv);
                Player.Instance.UiInventory.AddItem(newitemv);
                break;
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

    public void PickItem(string id, int valueToRemove)
    {
        int cashvalue = valueToRemove;
        ItemView item = GetItem(id);

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

    public ItemView FindContainsItemWithEmptyStack(string id)
    {
        foreach (var i in Items)
        {
            if (i.ItemId == id)
            {
                if (i.Count < DatabaseManager.GetItemData(id).MaxStack)
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
        float value = Player.Instance.PlayerStats.MinimumMass + (Player.Instance.EqippedBackpack != null
            ? Player.Instance.EqippedBackpack.GetStat("MaxMass")
            : 0);
        return value;
    }


}
