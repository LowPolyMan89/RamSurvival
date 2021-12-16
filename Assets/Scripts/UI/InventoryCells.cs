using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCells : MonoBehaviour
{
    public ItemUIElement ItemUIElement;
    public Button Button;
    public UiInventory UiInventory = null;
    public ChestInventoryUI chestUI = null;
    public bool IsChest = false;
    
    private void Start()
    {
        Button = gameObject.GetComponent<Button>();
        Button.onClick.AddListener(Click);
        
        if (!IsChest)
        {
            UiInventory = Player.Instance.UiInventory;
        }
        else
        {
            chestUI = UIController.Instance.ChestInventoryUI;
        }
        
    }

    public void Click()
    {
        if (transform.childCount > 0)
        {
            ItemUIElement = transform.GetChild(0).GetComponent<ItemUIElement>();
            if (!IsChest)
            {
                UiInventory.SelectItem(ItemUIElement);
            }
            else
            {
                chestUI.SelectItem(ItemUIElement);
            }
        }
        else
        {
            if (!IsChest)
            {
                UiInventory.SelectItem(null);
            }
            else
            {
                chestUI.SelectItem(null);
            }
        }
    }
}
