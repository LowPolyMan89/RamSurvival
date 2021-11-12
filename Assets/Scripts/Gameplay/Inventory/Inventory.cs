using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Entity
{
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private Transform inventoryStorage;
    [SerializeField] private float currentCapacity;
    [SerializeField] private Backpack inventoryBackpackPrefab;
    public float CurrentCapacity => currentCapacity;

    public virtual List<Item> GetItems()
    {
        return items;
    }

    public void EquipItem(Item item, EquipUISlot equipUISlot)
    {
        switch (item.equipType)
        {
            case EquipType.Backpack:
                
                item.transform.SetParent(PlayerStats.Instance.BackpackPoint);
                items.Remove(item);
                break;
            case EquipType.none:
                break;
        }

        EventManager.Instance.OnUpdateUI();
    }

    public virtual float CalculateCurrentCap()
    {
        float currentCap = 0f;

        foreach (var i in items)
        {
            if(i.ItemType != ItemType.Equip)
                currentCap += i.GetStat("Mass") * i.Count;
        }

        return currentCap;
    }

    /// <summary>
    /// Возвращает существующий Item, если нет, то null.
    /// </summary>
    /// <param name="newitem">Новый Item.</param>
    /// <returns>Существующий Item.</returns>
    protected virtual Item FindEqualsItem(Item newitem)
    {
        Item olditem = null;

        for (var index = 0; index < items.Count; index++)
        {
            var i = items[index];
            if (i.GetName() != newitem.GetName()) continue;
            olditem = i;
            return olditem;
        }

        return null;
    }

    private int GetEmptyCellsCount()
    {
        var c = 0;
        c = PlayerStats.Instance.GetInventoryCellsCount() - inventoryStorage.childCount;
        return c;
    }

    /// <summary>
    /// Возвращает существующий InventoryItem, если нет, то null.
    /// </summary>
    protected virtual Item FindEqualsInventoryItem(Item newitem)
    {
        Item olditem = null;

        for (var index = 0; index < items.Count; index++)
        {
            var i = items[index];
            if (i.GetName() != newitem.GetName()) continue;
            olditem = i;
            return olditem;
        }

        return null;
    }

    [ContextMenu("DropBackpack")]
    public void DropBackpack()
    {

        var i = Instantiate(inventoryBackpackPrefab);
        i.transform.position = PlayerStats.Instance.DropPoint.position;

        if(GetItems().Count > PlayerStats.Instance.GetInventoryCellsCount())
        {
            var a = PlayerStats.Instance.GetInventoryCellsCount();
            for(; a < GetItems().Count; a++)
            {
               items.Remove(DropItem(items[a]));
            }
        }

        currentCapacity = CalculateCurrentCap();
    }

    protected virtual Item DropItem(Item item)
    {
        var _item = Instantiate(item.Prefab).GetComponent<Item>();
        _item.transform.position = PlayerStats.Instance.DropPoint.position;
        _item.Count = item.Count;
        return item;
    }

    public void ChangeBackpack()
    {

    }

    public virtual Item AddEqipItem(Item item)
    {
        if (GetEmptyCellsCount() > 0)
        {
            
        }
        else
        {
            print("Can't find emty slot in enventory!");
        }

        return item;
    }

    public virtual Item AddItem(Item item, int count)
    {
        var maxCap = PlayerStats.Instance.GetInventoryCapacity();
        var currentCap = CalculateCurrentCap();

        if (maxCap < currentCap + item.GetStat("Mass") || currentCap >= maxCap)
        {
            print("Can't take, new item is bigger max mass");
            return item;
        }

        var olditem = FindEqualsItem(item);

        //если уже есть, то по возможности добавляем в кучу
        if (olditem && item.equipType == EquipType.none && item.ItemType == ItemType.Resource)
        {

            var oldInvItem = FindEqualsInventoryItem(item);
            oldInvItem.Count += count;
            currentCapacity = CalculateCurrentCap();
            Destroy(item.gameObject);
            return oldInvItem;

        }

        //если нет, то добавляем новый
        else if (GetEmptyCellsCount() > 0)
        {
            items.Add(item);
            currentCapacity = CalculateCurrentCap();
            item.Visualize(false);
            return item;
        }

        return null;
    } 


    protected override void Start()
    {
        print("Create Player Inventory");
    }
}
