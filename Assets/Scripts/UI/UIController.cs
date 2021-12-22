using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
	public GameObject LoadingPanel;
	public UiInfoPanel UiInfoPanel;
	public StatsUI StatsUI;
	public LogPanel LogPanel;
	public EquipUISlot GetEqipSlot(string itemid)
	{
		ItemDataSO data = DatabaseManager.Instance.GetItemData(itemid);
		
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
		
		var st = Player.Instance.PlayerStats;
		if (st)
		{
			int currentXp = Player.Instance.PlayerStats.CurrentXp;
			var exp = new Vector3();
			exp.x = currentXp;
			var stat = Player.Instance.PlayerStats.PlayerStatsDataSo.expStats;
			if (Player.Instance.PlayerStats.CurrentLvl > 1)
			{
				exp.x -= 
					stat.PlayerLevelStats[Player.Instance.PlayerStats.CurrentLvl - 2].ExpToNextLevel;
			}
			exp.y = stat.PlayerLevelStats[Player.Instance.PlayerStats.CurrentLvl - 1].ExpToNextLevel;
			exp.z = currentXp;
			StatsUI.Update(new Vector2(st.CurrentHitPoint, st.MAXHitPoint), new Vector2(st.CurrentFood,st.MAXFood), new Vector3(st.CurrentEnergy, st.MAXEnergy), exp);
		}
		
		LogPanel.UpdateLogger();
	}

	public void AddLogs(float time, string message, Color color)
	{
		LogPanel.AddLogs(time, message, color);
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
		eventManager.OnAddLogEvent += AddLogs;

	}

	public void OpenInfoPanel(ItemView selectedItemItem)
	{
		UiInfoPanel.gameObject.SetActive(true);
		ItemDataSO itemd = DatabaseManager.Instance.GetItemData(selectedItemItem.ItemId);
		UiInfoPanel.Init(itemd);
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


	public void LoadingComplite()
	{
		LoadingPanel.SetActive(false);
	}

}

[System.Serializable]
public class StatsUI
{
	[SerializeField] private Image HPimage;
	[SerializeField] private Image FoodImage;
	[SerializeField] private Image EnergyImage;
	[SerializeField] private Image ExpImage;
	[SerializeField] private TMP_Text HPtext;
	[SerializeField] private TMP_Text ENERGYtext;
	[SerializeField] private TMP_Text FOODtext;
	[SerializeField] private TMP_Text EXPtext;
	

	public void Update(Vector2 HP, Vector2 FOOD, Vector2 ENERGY, Vector3 EXP)
	{
		HPimage.fillAmount = HP.x / HP.y;
		FoodImage.fillAmount = FOOD.x / FOOD.y;
		EnergyImage.fillAmount = ENERGY.x / ENERGY.y;
		ExpImage.fillAmount = EXP.x / EXP.y;

		HPtext.text = $@"HP: {HP.x.ToString("0")}";
		ENERGYtext.text = $@"Energy: {ENERGY.x.ToString("0")}";
		FOODtext.text =$@"Food: {FOOD.x.ToString("0")}";
		EXPtext.text =
			$@"Level: {Player.Instance.PlayerStats.CurrentLvl}  {EXP.z.ToString("0")} / {EXP.y.ToString("0")}";
	}
	
}

[System.Serializable]
public class LogPanel
{
	[SerializeField] private TMP_Text LogText;
	private Queue<LogElement> LogElements = new Queue<LogElement>();
	private LogElement inProcess;
	
	public void UpdateLogger()
	{
		if (LogElements.Count < 1)
		{
			LogText.text = "";
			return;
		}
		
		inProcess = LogElements.Peek();
		LogText.text = inProcess.Message;
		inProcess.Time -= Time.deltaTime;
		LogText.alpha = inProcess.Time;
		LogText.color = inProcess.Color;
		
		if (inProcess.Time <= 0)
		{
			LogText.text = "";
			LogElements.Dequeue();
			inProcess = null;
		}
	}
	
	public void AddLogs(float time, string message, Color color)
	{
		LogElement el = new LogElement();
		el.Message = message;
		el.Time = time;
		el.Color = color;
		LogElements.Enqueue(el);	
	}
	
	[System.Serializable]
	public class LogElement
	{
		public float Time;
		public string Message;
		public Color Color;
	}
}

