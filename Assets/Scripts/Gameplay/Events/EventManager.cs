using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager Instance = null;
	public event Func<Entity, Entity> OnResorceSelectAction;
	public event Func<ItemUIElement, ItemUIElement> OnStartDragAction;
	public event Func<ItemUIElement, ItemUIElement> OnEndDragAction;
	public Action OnUpdateUIAction;

	private void Start()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance == this)
		{
			Destroy(gameObject);
		}
	}

	public void OnResorceSelect(Entity entity)
	{
		if (OnResorceSelectAction != null)
		{
			OnResorceSelectAction(entity);
		}
	}

	public void OnUpdateUI()
	{
		if (OnUpdateUIAction == null) return;
		OnUpdateUIAction();
	}

	public ItemUIElement OnOnStartDrag(ItemUIElement arg)
	{
		OnStartDragAction?.Invoke(arg);
		return arg;
	}
	
	public ItemUIElement OnOnEndDrag(ItemUIElement arg)
	{
		OnEndDragAction?.Invoke(arg);
		return arg;
	}
}
