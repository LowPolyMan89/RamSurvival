using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDropPanel : MonoBehaviour
{
        public ItemView DropItem;
        public ItemUIElement ItemUIElement;
        public Image Image;
        public Button DropButton;
        public Slider DropSlider;
        public Text InventoryCount, ToDropCount;
        public int countToDrop = 1;
        private Inventory dropinvent;
        private void Start()
        {
                DropButton.onClick.AddListener(Drop);
                DropSlider.onValueChanged.AddListener(ValueChangeCheck);
        }


        public void Init(ItemUIElement itemToDrop, Inventory dropinventory)
        {
                ItemUIElement = itemToDrop;
                DropSlider.minValue = 1;
                DropItem = itemToDrop.Item;
                DropSlider.maxValue = DropItem.Count;
                countToDrop = DropItem.Count;
                Image.sprite = DatabaseManager.GetItemData(DropItem.ItemId).Sprite;
                DropSlider.value = countToDrop;
                ToDropCount.text = countToDrop.ToString();
                InventoryCount.text = (DropItem.Count - countToDrop).ToString();
                dropinvent = dropinventory;
                dropinvent.RemoveItem(DropItem.ItemId);
                Destroy(itemToDrop.gameObject);
        }
        
        private void ValueChangeCheck(float arg0)
        {
                countToDrop = (int)arg0;
                InventoryCount.text = (DropItem.Count - countToDrop).ToString();
                ToDropCount.text = countToDrop.ToString();  
        }

        

        public void Drop()
        {
                if (countToDrop < DropItem.Count)
                {
                        

                        ItemDataSO data = DatabaseManager.GetItemData(DropItem.ItemId);
                        Item item = Instantiate(data.Prefab).GetComponent<Item>();
                        item.transform.position = Player.Instance.dropPoint.position;
                        item.Visualize(true);
                        item.Count = countToDrop;
                        DropItem.Count -= countToDrop;
                        dropinvent.AddItem(DropItem.ItemId, DropItem.Count);
                        print($@"Part of Item {DropItem.ItemId} dropped ");
                }
                else
                {
                        ItemDataSO data = DatabaseManager.GetItemData(DropItem.ItemId);
                        Item item = Instantiate(data.Prefab).GetComponent<Item>();
                        item.transform.position = Player.Instance.dropPoint.position;
                        item.Visualize(true);
                        item.Count = countToDrop;
                        print($@"Item {DropItem.ItemId} dropped ");
                        
                        if(dropinvent is Inventory)
                                UIController.Instance.UiInventory.HideButtons();
                }
                gameObject.SetActive(false);
        }
        
 
}
