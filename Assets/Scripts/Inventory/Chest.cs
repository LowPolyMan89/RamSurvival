using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Inventory, IInteractive<Chest>
{
    public Chest Use(Chest value)
    {
        UIController.Instance.OpenChestUi(value);
        return this;
    }

    public override void AddItem(string ItemId, int value)
    {
        int count = value;

        bool play = false;

        ItemView v = FindContainsItem(ItemId);

        ItemDataSO data = DatabaseManager.Instance.GetItemData(ItemId);

    
        if (v != null)
        {
            v.Count += value;
        }
        else
        {
            ItemView newitemv = new ItemView(ItemId, count);
            Items.Add(newitemv);
            UIController.Instance.ChestInventoryUI.AddItem(newitemv, this);
        }

    }

    public override bool CheckToAdd(ItemView item)
    {
        bool check = false;
        
        if (FindContainsItem(item.ItemId) != null)
        {
            check = true;
            return check;
        }

        if (Sloots > Items.Count)
        {
            check = true;
            return check;
        }

        return check;
    }
}
