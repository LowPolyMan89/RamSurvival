using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour
{
    [SerializeField] private InventoryItem inventoryItem;
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    public InventoryItem InventoryItem { get => inventoryItem; }

    public ItemUIElement Set(InventoryItem _inventoryItem)
    {
        inventoryItem = _inventoryItem;
        image.sprite = _inventoryItem.Item.itemDataSO.ItemSprite;
        text.text = _inventoryItem.Count.ToString();
        return this;
    }

    public ItemUIElement SetEqip(InventoryItem _inventoryItem)
    {
        if(_inventoryItem.Item.equipType == EquipType.Backpack)
        {
            image.sprite = _inventoryItem.Item.playerBackpackData.ItemSprite;
        }
        inventoryItem = _inventoryItem;
        text.text = _inventoryItem.Count.ToString();
        return this;
    }

    [ContextMenu("Eqip")]
    public void EqipItem()
    {
        PlayerStats.instance.Inventory.EquipItem(inventoryItem, null);
        inventoryItem = null;
        image.sprite = null;
        text.text = "";
        Destroy(this.gameObject);
    }
}
