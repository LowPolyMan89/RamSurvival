using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiDropPanel : MonoBehaviour
{
        public Item DropItem;
        public ItemUIElement ItemUIElement;
        public Image Image;
        public Button DropButton;
        public Slider DropSlider;
        public Text InventoryCount, ToDropCount;
        public int countToDrop = 1;
        private void Start()
        {
                DropButton.onClick.AddListener(Drop);
                DropSlider.onValueChanged.AddListener(ValueChangeCheck);
        }


        public void Init(ItemUIElement itemToDrop)
        {
                ItemUIElement = itemToDrop;
                DropSlider.minValue = 1;
                DropItem = itemToDrop.Item;
                DropSlider.maxValue = DropItem.Count;
                countToDrop = DropItem.Count;
                Image.sprite = DropItem.Sprite;
                DropSlider.value = countToDrop;
                ToDropCount.text = countToDrop.ToString();
                InventoryCount.text = (DropItem.Count - countToDrop).ToString();  
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
                        Item item = Instantiate(DropItem.Prefab).GetComponent<Item>();
                        item.transform.position = Player.Instance.dropPoint.position;
                        item.Visualize(true);
                        item.Count = countToDrop;
                        DropItem.Count -= countToDrop;
                        print($@"Part of Item {DropItem.GetName()} dropped ");
                }
                else
                {
                        Destroy(ItemUIElement);
                        DropItem.transform.position = Player.Instance.dropPoint.position;
                        DropItem.transform.SetParent(null);
                        DropItem.Visualize(true);
                        Player.Instance.PlayerInventory.RemoveItem(DropItem);
                        print($@"Item {DropItem.GetName()} dropped ");
                        Destroy(ItemUIElement.gameObject);
                        
                }
              
 
                gameObject.SetActive(false);
        }
        
 
}
