using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	public static UIController Instance = null;
	[SerializeField] private SelectedPanel selectedPanel;
	[SerializeField] private EventManager eventManager;
	[SerializeField] private EquipUI equipUI;
	[SerializeField] private Button grabButton;

	public UiInventory UiInventory;

	public EquipUISlot GetEqipSlot(Item item)
	{
		EquipType type = item.equipType;
		
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

	public bool isGrabObjectFind = false;

	private void Update()
	{
		grabButton.gameObject.SetActive(isGrabObjectFind);
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


}
