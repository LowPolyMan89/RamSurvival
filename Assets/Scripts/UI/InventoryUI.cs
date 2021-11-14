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
    [SerializeField] private UiDropPanel _uiDropPanel;
    public List<ItemUIElement> itemUIElements = new List<ItemUIElement>();
    public List<InventoryCells> inventoryCells = new List<InventoryCells>();
    public EquipUISlot backpackUIslot;
    [SerializeField] private EquipUI _equipUI;
    public ItemUIElement dragElement;

    public EquipUI EquipUI => _equipUI;

    public UiDropPanel UIDropPanel => _uiDropPanel;

    private void Start()
    {
        EventManager.Instance.OnUpdateUIAction += UpdateUI;
        EventManager.Instance.OnStartDragAction += OnStartDarg;
        EventManager.Instance.OnEndDragAction += OnEndDarg;
    }

    private void OnEnable()
    {
        UpdateUI();
    }

    [ContextMenu("UpdateUI")]
    public void UpdateUI()
    {
        ClearUI();
        inventory = FindObjectOfType<Inventory>();
        CreateInventoryCells();
        CreateInventoryUI(inventory.GetItems(), inventory.GetEqipItems());
        capacityText.text = inventory.CurrentCapacity.ToString("00") + "/" + PlayerStats.Instance.GetInventoryCapacity();
        capacityImage.fillAmount = (inventory.CurrentCapacity + 0.001f) / PlayerStats.Instance.GetInventoryCapacity();
    }
    

    public void CreateInventoryCells()
    {
        for(int i = 0; i < PlayerStats.Instance.GetInventoryCellsCount(); i++)
        {
            inventoryCells[i].gameObject.SetActive(true);
        }
    }

    private ItemUIElement OnStartDarg(ItemUIElement arg)
    {
        return arg;
    }
    
    private ItemUIElement OnEndDarg(ItemUIElement arg)
    {
        UpdateUI();
        return arg;
    }
    
    public void CreateInventoryUI(List<Item> inventoryItems, List<Item> eqippeditems)
    {
        int slotscheck = 0;

        foreach(var i in inventoryItems)
        {
            if (slotscheck < PlayerStats.Instance.GetInventoryCellsCount())
            {
                ItemUIElement newElement = Instantiate(itemUIElementPrefab).GetComponent<ItemUIElement>();
                newElement.transform.SetParent(inventoryCells[slotscheck].transform);
                newElement.transform.localPosition = Vector3.zero;
                
                if(i.ItemType != ItemType.Equip)
                    newElement.Set(i);
                if (i.ItemType == ItemType.Equip)
                    newElement.SetEqip(i);
                
                itemUIElements.Add(newElement);
                slotscheck++;
            }
        }

        foreach (var e in eqippeditems)
        {
            ItemUIElement newElement = Instantiate(itemUIElementPrefab).GetComponent<ItemUIElement>();

            if (e.equipType == EquipType.Backpack)
            {
                newElement.transform.SetParent(backpackUIslot.transform);
                newElement.transform.localPosition = Vector3.zero;
                newElement.SetEqip(e);
                itemUIElements.Add(newElement);
            }

        }
    }
    
    public void StartDrop(ItemUIElement item)
    {
        _uiDropPanel.gameObject.SetActive(true);
        _uiDropPanel.Init(item);
    }
    
    private void ClearUI()
    {
        foreach (var i in itemUIElements)
        {
            Destroy(i.gameObject);
        }
        itemUIElements.Clear();
        foreach (var cell in inventoryCells)
        {
           cell.gameObject.SetActive(false); 
        }
    }

    private void OnDisable()
    {
        ClearUI();
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnUpdateUIAction -= UpdateUI;
        EventManager.Instance.OnStartDragAction -= OnStartDarg;
        EventManager.Instance.OnStartDragAction -= OnEndDarg;
    }


}
