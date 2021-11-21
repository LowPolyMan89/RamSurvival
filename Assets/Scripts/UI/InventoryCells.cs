using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCells : MonoBehaviour
{
    public ItemUIElement ItemUIElement;
    public Button Button;
    public UiInventory UiInventory;
    private void Start()
    {
        Button = gameObject.GetComponent<Button>();
        Button.onClick.AddListener(Click);
        UiInventory = Player.Instance.UiInventory;
    }

    public void Click()
    {
        if (transform.childCount > 0)
        {
            ItemUIElement = transform.GetChild(0).GetComponent<ItemUIElement>();
            UiInventory.SelectItem(ItemUIElement);
        }
    }
}
