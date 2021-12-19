using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager Instance = null;
	public event Action OnTimerSecondAction;
	public event Func<Entity, Entity> OnResorceSelectAction;
	public event Func<ItemUIElement, ItemUIElement> OnStartDragAction;
	public event Func<ItemUIElement, ItemUIElement> OnEndDragAction;
	public event Func<bool> OnJumpAction;
	
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
	
	public void OnTimerSecond()
	{
		if (OnTimerSecondAction != null)
		{
			OnTimerSecondAction();
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
	
	public bool OnJump(bool arg)
	{
		if (OnJumpAction != null)
		{
			OnJumpAction();
		}

		return arg;
	}
}
