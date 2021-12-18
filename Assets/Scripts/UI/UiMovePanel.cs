using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiMovePanel : MonoBehaviour
{
        public ItemView MoveItem;
        public ItemUIElement ItemUIElement;
        public Image Image;
        public Button MoveButton;
        public Slider MoveSlider;
        public Text InventoryCount, ToDropCount;
        public int countToMove = 1;
        private Inventory _frominventory;
        private Inventory _toinventory;
        private void Start()
        {
                MoveButton.onClick.AddListener(Move);
                MoveSlider.onValueChanged.AddListener(ValueChangeCheck);
        }


        public void Init(ItemUIElement itemToMove, Inventory frominventory, Inventory toinventory)
        {
                ItemUIElement = itemToMove;
                MoveSlider.minValue = 1;
                MoveItem = itemToMove.Item;
                MoveSlider.maxValue = MoveItem.Count;
                countToMove = MoveItem.Count;
                Image.sprite = DatabaseManager.Instance.GetItemData(MoveItem.ItemId).Sprite;
                MoveSlider.value = countToMove;
                ToDropCount.text = countToMove.ToString();
                InventoryCount.text = (MoveItem.Count - countToMove).ToString();
                _frominventory = frominventory;
                _toinventory = toinventory;
                Destroy(ItemUIElement.gameObject);
        }
        
        private void ValueChangeCheck(float arg0)
        {
                countToMove = (int)arg0;
                InventoryCount.text = (MoveItem.Count - countToMove).ToString();
                ToDropCount.text = countToMove.ToString();  
        }

        public void Close()
        { 
                _frominventory.AddItem(MoveItem.ItemId, MoveItem.Count);
                gameObject.SetActive(false);
        }

        public void Move()
        {
                if (countToMove < MoveItem.Count)
                {
                        _toinventory.AddItem(MoveItem.ItemId, countToMove);
                        MoveItem.Count -= countToMove;
                        _frominventory.AddItem(MoveItem.ItemId, MoveItem.Count);
                        print($@"Part of Item {MoveItem.ItemId} moved ");
                        
                }
                else
                {
                        Destroy(ItemUIElement);
                        _toinventory.AddItem(MoveItem.ItemId, countToMove);
                        MoveItem.Count = countToMove;
                        print($@"Item {MoveItem.ItemId} moved ");
                        if(_frominventory is Inventory)
                                UIController.Instance.UiInventory.HideButtons();

                }
                UIController.Instance.UiInventory.HideButtons();
                gameObject.SetActive(false);
        }
        
}
