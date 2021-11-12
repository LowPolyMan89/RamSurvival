using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager Instance = null;
	public event Func<Entity, Entity> OnResorceSelectAction;
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
		print("OnUpdateUIAction");
		OnUpdateUIAction();
	}
}
