using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Entity
{
    [Header("Параметры инвентаря")]
    public bool isPlayerInventory = true;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private List<Item> eqipeditems = new List<Item>();
    [SerializeField] private Transform inventoryStorage;
    [SerializeField] private float currentCapacity;
    [SerializeField] private Backpack inventoryBackpackPrefab;
    public InventoryUI InventoryUI;
 
    [Header("Параметры сундука")]
    [SerializeField] private float capacity;
    [SerializeField] private int slots;
    [SerializeField] private Transform dropPoint;
    
    public float CurrentCapacity => currentCapacity;

    public Transform InventoryStorage => inventoryStorage;

    public virtual List<Item> GetItems()
    {
        return items;
    }
    public virtual List<Item> GetEqipItems()
    {
        return eqipeditems;
    }

    public void EquipItem(Item item)
    {
        if(!isPlayerInventory)
            return;
        
        switch (item.equipType)
        {
            case EquipType.Backpack:
                items.Remove(item);
                eqipeditems.Add(item);
                print("Eqip item " + item.GetName());
                break;
            case EquipType.none:
                break;
        }
        
    }

    public void UpdateCapacity()
    {
        currentCapacity = CalculateCurrentCap();
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
        
        if (isPlayerInventory)
        {
            c = PlayerStats.Instance.GetInventoryCellsCount() - inventoryStorage.childCount;
        }
            
        else
        {
            c = slots - inventoryStorage.childCount;
        }
        
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

    public void DropDragItem()
    {
        ItemUIElement item = PlayerStats.Instance.Inventory.InventoryUI.dragElement;
        Item newobj = Instantiate(item.InventoryItem.Prefab).GetComponent<Item>();
        newobj.transform.position = PlayerStats.Instance.DropPoint.position;
        newobj.Count = item.InventoryItem.Count;
        Destroy(item);

    }
    
    public  Item DropItem(ItemUIElement item)
    {
        if (!item) return null;
        InventoryUI.StartDrop(item);
        return item.InventoryItem;
    }
    
    public  void DropEqipItem(Item item)
    {
        if(!isPlayerInventory)
            return;
        
        item.transform.position = PlayerStats.Instance.DropPoint.position;
        item.transform.SetParent(null);
        if (item.equipType == EquipType.Backpack)
        {
            PlayerStats.Instance.DropBackpack();
        }
        eqipeditems.Remove(item);

    }
    

    public virtual Item AddEqipItem(Item item)
    {
        if (GetEmptyCellsCount() > 0)
        {
            items.Add(item);
            item.Visualize(false);
            item.transform.SetParent(inventoryStorage);
            return item;
        }
        else
        {
            if (PlayerStats.Instance.Inventory.InventoryUI.dragElement)
            {
                DropDragItem();
            }
            print("Can't find emty slot in enventory!");
        }

        return item;
    }
    
    public float GetStorageCapacity()
    {
        float cap = 0;

        cap = capacity;

        return cap;
    }

    public virtual Item AddItem(Item item, int count)
    {
        var maxCap = 0f;
        var currentCap = 0f;
        currentCap = CalculateCurrentCap();
        
        if (isPlayerInventory)
        {
            maxCap = PlayerStats.Instance.GetInventoryCapacity();
        }
        else
        {
            maxCap = GetStorageCapacity();
        }
 

        if (maxCap < currentCap + item.GetStat("Mass") || currentCap == maxCap)
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
            
            if (isPlayerInventory)
            {
                currentCapacity = CalculateCurrentCap();
            }
            else
            {
                currentCapacity = GetStorageCapacity();
            }
            
            
            Destroy(item.gameObject);
            return oldInvItem;

        }

        //если нет, то добавляем новый
        else if (GetEmptyCellsCount() > 0)
        {
            items.Add(item);
            
            if (isPlayerInventory)
            {
                currentCapacity = CalculateCurrentCap();
            }
            else
            {
                currentCapacity = GetStorageCapacity();
            }
            
            item.Visualize(false);
            item.transform.SetParent(inventoryStorage);
            
            return item;
        }

        return null;
    } 


    protected override void Start()
    {
        if(isPlayerInventory)
            print("Create Player Inventory");
    }
}
