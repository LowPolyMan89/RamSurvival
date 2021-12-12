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
                Image.sprite = DatabaseManager.GetItemData(MoveItem.ItemId).Sprite;
                MoveSlider.value = countToMove;
                ToDropCount.text = countToMove.ToString();
                InventoryCount.text = (MoveItem.Count - countToMove).ToString();
                _frominventory = frominventory;
                _toinventory = toinventory;
        }
        
        private void ValueChangeCheck(float arg0)
        {
                countToMove = (int)arg0;
                InventoryCount.text = (MoveItem.Count - countToMove).ToString();
                ToDropCount.text = countToMove.ToString();  
        }

        

        public void Move()
        {
                if (countToMove < MoveItem.Count)
                {
                        _toinventory.AddItem(MoveItem.ItemId, countToMove);
                        MoveItem.Count -= countToMove;
                        print($@"Part of Item {MoveItem.ItemId} moved ");
                }
                else
                {
                        Destroy(ItemUIElement);
                        _toinventory.AddItem(MoveItem.ItemId, countToMove);
                        MoveItem.Count = countToMove;
                        _frominventory.RemoveItem(MoveItem.ItemId);
                        print($@"Item {MoveItem.ItemId} moved ");
                        if(_frominventory is Inventory)
                                UIController.Instance.UiInventory.HideButtons();
                        if(_frominventory is Chest)
                                UIController.Instance.ChestInventoryUI.HideButtons();
                        Destroy(ItemUIElement.gameObject);
                }
                UIController.Instance.UiInventory.HideButtons();
                UIController.Instance.ChestInventoryUI.HideButtons();
                gameObject.SetActive(false);
        }
        
}
