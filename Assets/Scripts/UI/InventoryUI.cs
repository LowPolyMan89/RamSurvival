using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject itemUIElementPrefab;
    [SerializeField] private GameObject inventoryCellsPrefab;
    [SerializeField] private Transform itemUIElementParent;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Text capacityText;
    [SerializeField] private Image capacityImage;
    public List<ItemUIElement> itemUIElements = new List<ItemUIElement>();
    public List<InventoryCells> inventoryCells = new List<InventoryCells>();

    private void Start()
    {
        EventManager.instance.OnUpdateUIAction += UpdateUI;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        inventory = FindObjectOfType<Inventory>();
        CreateInventoryCells();
        CreateInventoryUI(inventory.GetItems());
        capacityText.text = inventory.CurrentCapacity.ToString("00") + "/" + PlayerStats.instance.GetInventoryCapacity();
        capacityImage.fillAmount = (inventory.CurrentCapacity + 0.001f) / PlayerStats.instance.GetInventoryCapacity();
    }

    public void CreateInventoryCells()
    {
        for(int i = 0; i < PlayerStats.instance.GetInventoryCellsCount(); i++)
        {
            InventoryCells newElement = Instantiate(inventoryCellsPrefab).GetComponent<InventoryCells>();
            newElement.transform.SetParent(itemUIElementParent);
            inventoryCells.Add(newElement);
        }
    }

    public void CreateInventoryUI(List<InventoryItem> inventoryItems)
    {
        int slotscheck = 0;

        foreach(var i in inventoryItems)
        {
            if (slotscheck < PlayerStats.instance.GetInventoryCellsCount())
            {
                ItemUIElement newElement = Instantiate(itemUIElementPrefab).GetComponent<ItemUIElement>();
                newElement.transform.SetParent(inventoryCells[slotscheck].transform);
                if(i.Item.ItemType != ItemType.Equip)
                    newElement.Set(i);
                if (i.Item.ItemType == ItemType.Equip)
                    newElement.SetEqip(i);
                itemUIElements.Add(newElement);
                slotscheck++;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var i in inventoryCells)
        {
            Destroy(i.gameObject);
        }

        inventoryCells.Clear();

        foreach (var i in itemUIElements)
        {
            Destroy(i.gameObject);
        }

        itemUIElements.Clear();
    }

    private void OnDestroy()
    {
        EventManager.instance.OnUpdateUIAction -= UpdateUI;
    }
}
