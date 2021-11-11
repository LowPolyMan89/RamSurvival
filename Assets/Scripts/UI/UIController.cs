using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public static UIController instance = null;
	[SerializeField] private SelectedPanel selectedPanel;
	[SerializeField] private EventManager eventManager;
	[SerializeField] private InventoryUI inventoryUI;
	[SerializeField] private EquipUI equipUI;

	void Start()
	{

		if (instance == null)
		{
			instance = this;
		}
		else if (instance == this)
		{
			Destroy(gameObject);
		}

		selectedPanel.gameObject.SetActive(false);
		inventoryUI.gameObject.SetActive(false);
		eventManager = EventManager.instance;
		eventManager.OnResorceSelectAction += ShowSelectedPanel;

		inventoryUI = FindObjectOfType<InventoryUI>();
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
}
