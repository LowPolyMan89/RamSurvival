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
	public event Func<int, int> OnPlayerGetXPAction;
	public event Func<float, float> OnPlayerGetHPAction;
	public event Func<float, float> OnPlayerGetFoodAction;
	public event Func<float, float> OnPlayerGetEnergyAction;
	public event Action<float, string, Color> OnAddLogEvent;
	
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
	
	public int OnPlayerGetXP(int arg)
	{
		if (OnPlayerGetXPAction != null)
		{
			OnPlayerGetXPAction(arg);
		}
		
		return arg;
	}
	
	public float OnPlayerGetHP(float arg)
	{
		if (OnPlayerGetHPAction != null)
		{
			OnPlayerGetHPAction(arg);
		}
		
		return arg;
	}
	
	public float OnPlayerGetFood(float arg)
	{
		if (OnPlayerGetFoodAction != null)
		{
			OnPlayerGetFoodAction(arg);
		}
		
		return arg;
	}
	
	public float OnPlayerGetEnergy(float arg)
	{
		if (OnPlayerGetEnergyAction != null)
		{
			OnPlayerGetEnergyAction(arg);
		}
		
		return arg;
	}
	
	public string AddLog(float time, string message, Color color)
	{
		if (OnAddLogEvent != null)
		{
			OnAddLogEvent(time, message, color);
		}
		
		return message;
	}

	
}
