using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestInventoryUI : MonoBehaviour
{
    
    public Chest currentInventoryChest;
    public Transform chestInventoryPanel;
    public GameObject inventoryCellPrefab;
    [SerializeField] private ItemUIElement _itemUIElement;
    public Inventory frominventory;
    public Inventory toinventory;
    public List<InventoryCells> InventoryCellses = new List<InventoryCells>();
    
    public ItemUIElement SelectedItem { get; set; }
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(CustomUpdate());
        Player.Instance.UiInventory.OpenChestInventory(currentInventoryChest, this);
    }
    private void OnDisable()
    {
        Player.Instance.UiInventory.OpenChestInventory(null, null);
        
        foreach (var cell in InventoryCellses)
        {
            if (cell.transform.childCount > 0)
            {
                Destroy(cell.transform.GetChild(0).gameObject, 0.01f);
            }
        }
        
        StopCoroutine(CustomUpdate());
    }

    public void CreateCells()
    {
        InventoryCellses.Clear();

        for (int i = 0; i < currentInventoryChest.Sloots; ++i)
        {
            InventoryCells cell = Instantiate(inventoryCellPrefab, chestInventoryPanel).GetComponent<InventoryCells>();
            cell.transform.localPosition = Vector3.zero;
            cell.chestUI = this;
            InventoryCellses.Add(chestInventoryPanel.GetChild(i).GetComponent<InventoryCells>());
        }
    }

    public void SelectItem(ItemUIElement itemUIElement, Inventory from, Inventory to, bool isHide = false)
    {
        UIController.Instance.UiInventory.HideButtons();
        if (itemUIElement == null)
        {
            return;
        }
        frominventory = from;
        toinventory = to;
        SelectedItem = itemUIElement;
        if (to == currentInventoryChest)
        {
            bool test = !(currentInventoryChest.Items.Count >= currentInventoryChest.Sloots);

            if (currentInventoryChest.FindContainsItem(itemUIElement.Item.ItemId) != null)
            {
                test = true;
            }
            
          //  moveButton.gameObject.SetActive(test);
        }
    }
    
    public void SelectItem(ItemUIElement itemUIElement)
    {
        if (itemUIElement == null)
        {
            return;
        }

        frominventory = currentInventoryChest;
        toinventory = Player.Instance.PlayerInventory;
        SelectedItem = itemUIElement;

    }

    public void DropButtonUse()
    {
        UIController.Instance.UiDropPanel.gameObject.SetActive(true);
        UIController.Instance.UiDropPanel.Init(SelectedItem, currentInventoryChest);
    }
    
    
    public void InfoButtonUse()
    {
        print(SelectedItem.Item.ItemId);
    }
    
    
    private IEnumerator CustomUpdate()
    {
        yield return new WaitForSeconds(0.1f);

       // moveButton.gameObject.SetActive(currentInventoryChest.Items.Count < currentInventoryChest.Sloots);

        foreach (var cell in InventoryCellses)
        {
            if (cell.transform.childCount > 0)
            {
                if (cell.transform.GetChild(0).GetComponent<ItemUIElement>().Item.Count < 1)
                {
                    Destroy(cell.transform.GetChild(0).gameObject, 0.01f);
                }
            }
        }
        
        StartCoroutine(CustomUpdate());
    }
    
    public void AddItem(ItemView item)
    {
        //create item ui
        ItemUIElement itemUIElement = Instantiate(_itemUIElement);
        itemUIElement.transform.SetParent(GetEmptyCell());
        itemUIElement.transform.localPosition = Vector3.zero;
        itemUIElement.Item = item;
    }
    
    public void AddItem(ItemView item, Inventory iteminventory)
    {
        //create item ui
        ItemUIElement itemUIElement = Instantiate(_itemUIElement);
        itemUIElement.transform.SetParent(GetEmptyCell());
        itemUIElement.transform.localPosition = Vector3.zero;
        itemUIElement.transform.localScale = Vector3.one;
        itemUIElement.Item = item;
        itemUIElement.ItemInventory = iteminventory;

    }
    
    public Transform GetEmptyCell()
    {
        foreach (var v in InventoryCellses)
        {
            if (v.transform.childCount == 0)
            {
                return v.transform;
            }
        }
        return null;
    }
    

    public void Open(Chest chest)
    {
        currentInventoryChest = chest;
        
        if (InventoryCellses.Count < 1)
        {
            CreateCells();
        }

        int curritemstypecount = 0;
        foreach (var v in InventoryCellses)
        {
            if (v.transform.childCount > 0)
            {
                curritemstypecount++;
            }
        }

        if (chest.Items.Count != curritemstypecount)
        {
            foreach (var item in chest.Items)
            {
                AddItem(item, chest);
            }
        }

    }
}
