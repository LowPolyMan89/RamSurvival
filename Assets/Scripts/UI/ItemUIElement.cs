using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Item inventoryItem;
    [SerializeField] private Image image;
    [SerializeField] private Text text;
    [SerializeField] private Transform startRoot;
    public bool isEqiped = false;
    public Item InventoryItem { get => inventoryItem; }

    private void Start()
    {
        startRoot = transform.parent;
        

    }

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
            inventoryItem = _inventoryItem;
            text.text = _inventoryItem.Count.ToString();
            image.sprite = _inventoryItem.Sprite;
        }

        return this;
    }



    public void OnDrag(PointerEventData eventData)
    {
#if UNITY_EDITOR
        transform.position = Input.mousePosition;
#endif

#if !UNITY_EDITOR
        transform.position = Input.touches[0].position;
#endif
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        EventManager.Instance.OnOnStartDrag(this);
        startRoot = transform.parent;
        PlayerStats.Instance.Inventory.InventoryUI.dragElement = this;
        
        if (PlayerStats.Instance.Inventory.GetItems().Contains(inventoryItem))
        {
            //PlayerStats.Instance.Inventory.DropItem(this);
            PlayerStats.Instance.Inventory.GetItems().Remove(inventoryItem);
        }
        
        if (PlayerStats.Instance.Inventory.GetEqipItems().Contains(inventoryItem))
        {
            PlayerStats.Instance.Inventory.GetEqipItems().Remove(inventoryItem);
            PlayerStats.Instance.Inventory.DropEqipItem(inventoryItem);
        }
        transform.SetParent(transform.root);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var uiEventSystem = EventSystem.current;
        
        
        if (uiEventSystem != null)
        {
            var uiPointerEventData = new PointerEventData(uiEventSystem);
            uiPointerEventData.position = Input.mousePosition;

            List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();

            uiEventSystem.RaycastAll(uiPointerEventData, uiRaycastResultCache);
            
            if (uiRaycastResultCache.Count > 0)
            {
                foreach (var v in uiRaycastResultCache)
                {
                    if(v.gameObject.CompareTag($"Cell"))
                    {
                        NewPosition(v.gameObject.transform);
                        PlayerStats.Instance.Inventory.AddItem(inventoryItem, inventoryItem.Count);
                        isEqiped = false;
                        EventManager.Instance.OnOnEndDrag(this);
                        return;
                    }
                    else if(v.gameObject.CompareTag($"ItemSlot"))
                    {
                        var slot = v.gameObject.GetComponent<EquipUISlot>();
                        
                        if (slot.SlotEquipType == EquipType.Backpack)
                        {
                            isEqiped = true;
                            inventoryItem.transform.SetParent(PlayerStats.Instance.Inventory.InventoryStorage);
                            PlayerStats.Instance.EqipBacpack(inventoryItem);
                        }
                        
                        NewPosition(v.gameObject.transform);
                        EventManager.Instance.OnOnEndDrag(this);
                        return;
                    }
                    else if(v.gameObject.CompareTag($"DropZone"))
                    {
                        PlayerStats.Instance.Inventory.DropItem(this);
                        EventManager.Instance.OnOnEndDrag(this);
                        return;
                    }
                    else
                    {
                        isEqiped = false;
                       ResetPosition();
                    }
                }
            }
        }
        
        

    }

    public void NewPosition(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
    }
    private void ResetPosition()
    {
        transform.SetParent(startRoot);
        transform.localPosition = Vector3.zero;
    }

    private void OnDestroy()
    {
        
    }
}
