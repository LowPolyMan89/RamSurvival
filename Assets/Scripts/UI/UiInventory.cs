using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiInventory : MonoBehaviour
{
    public Transform InventoryCellsesParent;
    public List<InventoryCells> InventoryCellses = new List<InventoryCells>();
    [SerializeField] private ItemUIElement _itemUIElement;
    [SerializeField] private Player _player;
    [SerializeField] private Text capacityText;
    [SerializeField] private Image capacityImage;
    [SerializeField] private GameObject infoButton;
    [SerializeField] private GameObject useButton;
    [SerializeField] private GameObject dropButton;
    public UiDropPanel UiDropPanel;
    public ItemUIElement SelectedItem;

    [ContextMenu("CreateCells")]
    public void CreateCells()
    {
        InventoryCellses.Clear();

        for (int i = 0; i < InventoryCellsesParent.childCount; ++i)
        {
            InventoryCellses.Add(InventoryCellsesParent.GetChild(i).GetComponent<InventoryCells>());
        }
    }

    private void Start()
    {
        useButton.GetComponent<Button>().onClick.AddListener(EqipButtonUse);
        infoButton.GetComponent<Button>().onClick.AddListener(InfoButtonUse);
        dropButton.GetComponent<Button>().onClick.AddListener(DropButtonUse);
    }

    private void OnEnable()
    {
        _player = Player.Instance;
        StartCoroutine(CustomUpdate());
        HideButtons();
    }


private void OnDisable()
    {
        StopCoroutine(CustomUpdate());
    }

    private IEnumerator CustomUpdate()
    {
        yield return new WaitForSeconds(0.1f);
        
        capacityText.text = _player.PlayerInventory.GetCurrentInventoryMass().ToString("00") + "/" + _player.PlayerInventory.GetMaxInventoryMass();
        capacityImage.fillAmount = (_player.PlayerInventory.GetCurrentInventoryMass() + 0.001f) / _player.PlayerInventory.GetMaxInventoryMass();
        
        StartCoroutine(CustomUpdate());
    }
    

    public void AddItem(Item item)
    {
        //create item ui
       ItemUIElement itemUIElement = Instantiate(_itemUIElement);
       itemUIElement.transform.SetParent(GetEmptyCell());
       itemUIElement.transform.localPosition = Vector3.zero;
       itemUIElement.Item = item;
       
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
    
    public void DropButtonUse()
    {
        UiDropPanel.gameObject.SetActive(true);
        UiDropPanel.Init(SelectedItem);
    }

    public void InfoButtonUse()
    {
        print(SelectedItem.Item.ItemId);
    }

    public void EqipButtonUse()
    {
        switch (SelectedItem.Item.equipType)
        {
            case EquipType.Backpack:
                if (SelectedItem.IsEqipped)
                {
                    Player.Instance.EqipBackpack(null);
                    Player.Instance.PlayerInventory.AddItem(SelectedItem.Item);
                    Destroy(SelectedItem.gameObject);
                    SelectedItem = null;
                    HideButtons();
                }
                else
                {
                    Player.Instance.EqipBackpack(SelectedItem.Item);
                    SelectedItem.IsEqipped = true;
                    var tr = UIController.Instance.GetEqipSlot(SelectedItem.Item);
                    tr.Item = SelectedItem.Item;
                    SelectedItem.transform.SetParent(tr.transform);
                    SelectedItem.transform.localPosition = Vector3.zero;
                    Player.Instance.PlayerInventory.RemoveItem(SelectedItem.Item);
                    SelectedItem = null;
                    HideButtons();
                }
                break;
            case EquipType.none:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void HideButtons()
    {
        useButton.SetActive(false);
        infoButton.SetActive(false);
        dropButton.SetActive(false);
    }
    
    public void SelectItem(ItemUIElement itemUIElement)
    {
        if (itemUIElement == null)
        {
            HideButtons();
            return;
        }
        SelectedItem = itemUIElement;
        useButton.SetActive(false);

       if (itemUIElement.Item.ItemType == ItemType.Resource)
       {
           infoButton.SetActive(true);
           dropButton.SetActive(true);
       }
       if (itemUIElement.Item.ItemType == ItemType.Equip)
       {
           useButton.SetActive(true);
           infoButton.SetActive(true);
           dropButton.SetActive(true);
       }
    }
}
