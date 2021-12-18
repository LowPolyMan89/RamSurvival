using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController Instance = null;
	public GrabbetItemsHUD grabbetItemsHUD;
	[SerializeField] private SelectedPanel selectedPanel;
	[SerializeField] private EventManager eventManager;
	[SerializeField] private EquipUI equipUI;
	[SerializeField] private Button grabButton;
	[SerializeField] private Button useButton;
	[SerializeField] private Transform chestUi;
	public List<Item> nearestItems = new List<Item>();
	public UiInventory UiInventory;
	public CrafterUi CrafterUi;
	public Transform craftPanel;
	public ChestInventoryUI ChestInventoryUI;
	public UiDropPanel UiDropPanel;
	public UiMovePanel UiMovePanel;
	public Transform MainInventory;
	public EquipUISlot GetEqipSlot(string itemid)
	{
		ItemDataSO data = DatabaseManager.GetItemData(itemid);
		
		EquipType type = data.equipType;
		
		foreach (var VARIABLE in equipUI.EquipUISlots)
		{
			if (VARIABLE.SlotEquipType == type)
			{
				return VARIABLE;
			}
		}

		return null;
	}
	
	public Button GrabButton => grabButton;
	public Button UseButton => useButton;

	public bool isUseObjectFind = false;

	private void Update()
	{
		useButton.gameObject.SetActive(isUseObjectFind);
		grabButton.gameObject.SetActive(nearestItems.Count > 0);
	}

	void Start()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance == this)
		{
			Destroy(gameObject);
		}
		
		eventManager = EventManager.Instance;
		eventManager.OnResorceSelectAction += ShowSelectedPanel;
		
	}

	private Entity ShowSelectedPanel(Entity entity)
	{
		if(entity != null)
		{
			selectedPanel.gameObject.SetActive(true);

			if(entity is Resource)
			{
				selectedPanel.Name.text = (string)entity.GetName() + " T" + entity.GetRare();
				selectedPanel.Count.text = entity.GetCount().ToString();
				selectedPanel.Icon.sprite = entity.GetSprite();
				selectedPanel.entity = entity;
				selectedPanel.Init();
			}
			if (entity is Item)
			{
				selectedPanel.Name.text = (string)entity.GetName() + " T" + entity.GetRare();
				selectedPanel.Count.text = entity.GetCount().ToString();
				selectedPanel.Icon.sprite = entity.GetSprite();
				selectedPanel.entity = entity;
				selectedPanel.Init();
			}
			
		}
		else
		{
			selectedPanel.gameObject.SetActive(false);
		}

		return entity;
	}


	public void OpenCraftPlayer()
	{
		OpenCraftPanel(Player.Instance.playerCrafter.Sheme, Player.Instance.PlayerInventory, Player.Instance.playerCrafter);
	}
	
	public void OpenCraftPanel(CraftSheme sheme, Inventory inventoryToGetItems, Crafter currentcrafter)
	{
		craftPanel.gameObject.SetActive(true);
		CrafterUi.Open(sheme, inventoryToGetItems, currentcrafter);
	}

	public void OpenChestUi(Chest value)
	{
		MainInventory.gameObject.SetActive(true);
		chestUi.gameObject.SetActive(true);
		ChestInventoryUI.Open(value);
		UiInventory.chestInventory = value;
	}
	
	public void AddNearestItem(Item item)
	{
		nearestItems.Add(item);
		grabbetItemsHUD.AddElement(item);
	}
	public void RemoveNearestItem(Item item)
	{
		nearestItems.Remove(item);
		grabbetItemsHUD.RemoveElement(item);
	}
	
	[System.Serializable]
	public class GrabbetItemsHUD
	{
		public Transform panel;
		public GameObject Ptrefab;
		public List<GrabbetItemElement> elements = new List<GrabbetItemElement>();

		public void AddElement(Item item)
		{
			GrabbetItemElement el = Instantiate(Ptrefab, panel).GetComponent<GrabbetItemElement>();
			el.image.sprite = item.Sprite;
			el.text.text = item.Count.ToString();
			el.grabbetItem = item;
			elements.Add(el);
		}

		public void RemoveElement(Item item)
		{
			GrabbetItemElement toremove = null;
			foreach (var v in elements)
			{
				if (v.grabbetItem == item)
				{
					toremove = v;
					break;
				}
			}
			if (toremove)
			{
				elements.Remove(toremove);
				Destroy(toremove.gameObject);
			}
		}
	}


	
}