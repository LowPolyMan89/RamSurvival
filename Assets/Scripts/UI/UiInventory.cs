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
        useButton.SetActive(false);
        infoButton.SetActive(false);
        dropButton.SetActive(false);
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
       itemUIElement.transform.SetParent(InventoryCellses[0].transform);
       itemUIElement.transform.localPosition = Vector3.zero;
       itemUIElement.Item = item;
       
    }

    public void DropButtonUse()
    {
        
    }

    public void InfoButtonUse()
    {
        print(SelectedItem.Item.ItemId);
    }

    public void EqipButtonUse()
    {
        
    }
    
    public void SelectItem(ItemUIElement itemUIElement)
    {
        SelectedItem = itemUIElement;
       useButton.SetActive(false);

       if (itemUIElement.Item.ItemType == ItemType.Resource)
       {
           infoButton.SetActive(true);
           dropButton.SetActive(true);
       }
    }
}
