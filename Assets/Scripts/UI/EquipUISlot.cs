using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipUISlot : MonoBehaviour
{
    public ItemUIElement ItemUIElement;
    [SerializeField] private EquipType slotEquipType;
    [SerializeField] public Image image;
    public Item Item;

    public EquipType SlotEquipType { get => slotEquipType; }
    
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
        else
        {
            UiInventory.SelectItem(null);
        }
    }

}
