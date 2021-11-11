using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Entity
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    [SerializeField] private List<InventoryItem> eqipedItems = new List<InventoryItem>();
    [SerializeField] private Transform inventoryStorage;
    [SerializeField] private float currentCapacity;
    [SerializeField] private Backpack inventoryBackpackPrefab;
    public float CurrentCapacity { get => currentCapacity; }

    public virtual List<InventoryItem> GetItems()
    {
        return items;
    }

    public void EquipItem(InventoryItem item, EquipUISlot equipUISlot)
    {
        switch (item.Item.equipType)
        {
            case EquipType.Backpack:
                PlayerStats.instance.PlayerBackpackData = item.GetComponent<Backpack>().playerBackpackData;
                item.transform.SetParent(PlayerStats.instance.BackpackPoint);
                items.Remove(item);
                eqipedItems.Add(item);
                break;
            case EquipType.none:
                break;
        }

        EventManager.instance.OnUpdateUI();
    }

    public virtual float CalculateCurrentCap()
    {
        float currentCap = 0f;

        foreach (var i in items)
        {
            if(i.Item.ItemType != ItemType.Equip)
                currentCap += i.Item.itemDataSO.Capacity * i.Count;
        }

        return currentCap;
    }

    /// <summary>
    /// Возвращает существующий Item, если нет, то null.
    /// </summary>
    /// <param name="newitem">Новый Item.</param>
    /// <returns>Существующий Item.</returns>
    public virtual Item FindEqualsItem(Item newitem)
    {
        Item olditem = null;

        foreach (var i in items)
        {
            if (i.Item.itemDataSO == newitem.itemDataSO)
            {
                olditem = i.Item;
                return olditem;
            }
        }
        return olditem;
    }

    public int GetEmptyCellsCount()
    {
        int c = 0;
        c = PlayerStats.instance.GetInventoryCellsCount() - inventoryStorage.childCount;
        return c;
    }

    /// <summary>
    /// Возвращает существующий InventoryItem, если нет, то null.
    /// </summary>
    public virtual InventoryItem FindEqualsInventoryItem(Item newitem)
    {
        InventoryItem olditem = null;

        foreach (var i in items)
        {
            if (i.Item.itemDataSO == newitem.itemDataSO)
            {
                olditem = i;
                return olditem;
            }
        }
        return olditem;
    }

    [ContextMenu("DropBackpack")]
    public void DropBackpack()
    {
        PlayerBackpackDataSO playerBackpackDataSO = PlayerStats.instance.PlayerBackpackData;

        PlayerStats.instance.PlayerBackpackData = null;
        Backpack i = Instantiate(inventoryBackpackPrefab);
        i.transform.position = PlayerStats.instance.DropPoint.position;

        if(GetItems().Count > PlayerStats.instance.GetInventoryCellsCount())
        {
            for(int a = PlayerStats.instance.GetInventoryCellsCount(); a < GetItems().Count; a++)
            {
               items.Remove(DropItem(items[a]));
            }
        }

        currentCapacity = CalculateCurrentCap();
    }

    public virtual InventoryItem DropItem(InventoryItem item)
    {
        Item _item = Instantiate(item.Item.itemDataSO.Prefab).GetComponent<Item>();
        _item.transform.position = PlayerStats.instance.DropPoint.position;
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
            GameObject _tempgameObject = new GameObject();
            _tempgameObject.name = item.ItemId;
            _tempgameObject.transform.SetParent(inventoryStorage);
            Item _newInventoryItem = new Item();

            if (item.equipType == EquipType.Backpack)
            {
                _newInventoryItem = _tempgameObject.AddComponent<Backpack>();
                _newInventoryItem.ItemId = item.ItemId;
                _newInventoryItem.playerBackpackData = item.playerBackpackData;
                _newInventoryItem.ItemRare = item.ItemRare;
                _newInventoryItem.Count = 1;
            }
            
            InventoryItem newInvItem = _tempgameObject.AddComponent<InventoryItem>();
            newInvItem.Set(_newInventoryItem, item.ItemType, 1);

            items.Add(newInvItem);
            Destroy(item.gameObject);
            return _newInventoryItem;
        }
        else
        {
            print("Can't find emty slot in enventory!");
        }

        return item;
    }

    public virtual Item AddItem(Item item, int count)
    {
        float maxCap = PlayerStats.instance.GetInventoryCapacity();
        float currentCap = CalculateCurrentCap();

        if (maxCap < currentCap + item.itemDataSO.Capacity || currentCap >= maxCap)
        {
            print("Can't take, new item is bigger max cap");
            return item;
        }

        Item olditem = FindEqualsItem(item);

        if (olditem && item.equipType == EquipType.none)
        {

            InventoryItem oldInvItem = FindEqualsInventoryItem(item);
            oldInvItem.Add(count);
            oldInvItem.Item.Count = count;
            currentCapacity = CalculateCurrentCap();
            Destroy(item.gameObject);
            return oldInvItem.Item;

        } 
        else if(GetEmptyCellsCount() > 0)
        {
            GameObject _tempgameObject = new GameObject();
            _tempgameObject.name = item.ItemId;
            _tempgameObject.transform.SetParent(inventoryStorage);

            Item _newInventoryItem = _tempgameObject.AddComponent<Item>();
            _newInventoryItem.ItemId = item.ItemId;
            _newInventoryItem.itemDataSO = item.itemDataSO;
            _newInventoryItem.ItemRare = item.ItemRare;
            _newInventoryItem.Count = count;

            InventoryItem newInvItem = _tempgameObject.AddComponent<InventoryItem>();
            newInvItem.Set(_newInventoryItem, item.ItemType, count);

            items.Add(newInvItem);
            currentCapacity = CalculateCurrentCap();
            Destroy(item.gameObject);
            return _newInventoryItem;
        }

        return null;
    } 


    protected override void Start()
    {
        print("Create Player Inventory");
    }
}
