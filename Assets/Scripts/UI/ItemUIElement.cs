using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour
{
    [SerializeField] private Item inventoryItem;
    [SerializeField] private Image image;
    [SerializeField] private Text text;

    public Item InventoryItem { get => inventoryItem; }

    public ItemUIElement Set(Item _inventoryItem)
    {
        inventoryItem = _inventoryItem;
        image.sprite = _inventoryItem.Sprite;
        text.text = _inventoryItem.Count.ToString();
        return this;
    }

    public ItemUIElement SetEqip(Item _inventoryItem)
    {
        if(_inventoryItem.equipType == EquipType.Backpack)
        {
           
        }
        inventoryItem = _inventoryItem;
        text.text = _inventoryItem.Count.ToString();
        return this;
    }

    [ContextMenu("Eqip")]
    public void EqipItem()
    {
        PlayerStats.Instance.Inventory.EquipItem(inventoryItem, null);
        inventoryItem = null;
        image.sprite = null;
        text.text = "";
        Destroy(this.gameObject);
    }
}
