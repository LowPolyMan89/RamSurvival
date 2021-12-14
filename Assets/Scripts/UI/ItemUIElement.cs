using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ItemUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemView Item;
    public Image Image;
    public Text CountText;
    [SerializeField] private Transform startRoot;
    public Inventory ItemInventory;
    
    public bool IsEqipped { get; set; }

    public void OnEnable()
    {
        StartCoroutine(CustomUpdate());
    }

    private void OnDisable()
    {
        StopCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        Image.sprite = DatabaseManager.GetItemData(Item.ItemId).Sprite;
        CountText.text = Item.Count.ToString();
        StartCoroutine(CustomUpdate());
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startRoot = transform.parent;
        transform.SetParent(transform.root);
        ItemInventory.RemoveItem(Item);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.screenPosition;
    }


    public void OnEndDrag(PointerEventData eventData)
    {

        foreach (var h in GetEventSystemRaycastResults())
        {
            if (h.gameObject.CompareTag("Cell"))
            {
                InventoryCells cell = h.gameObject.GetComponent<InventoryCells>();
                
                if (!cell.IsChest)
                {
                    if (!Player.Instance.PlayerInventory.CheckToAdd(Item)) continue;
                    Player.Instance.PlayerInventory.AddItem(this.Item.ItemId, this.Item.Count);
                    Destroy(this.gameObject, 0.01f);
                    return;
                }
                else
                {
                    if (!UIController.Instance.UiInventory.chestInventory.CheckToAdd(Item)) continue;
                    UIController.Instance.UiInventory.chestInventory.AddItem(this.Item.ItemId, this.Item.Count);
                    Destroy(this.gameObject, 0.01f);
                    return;
                }
            }
            
  
        }
        
        ItemInventory.AddItem(this.Item.ItemId, this.Item.Count);
        Destroy(this.gameObject, 0.01f);
    }
    
    
    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
