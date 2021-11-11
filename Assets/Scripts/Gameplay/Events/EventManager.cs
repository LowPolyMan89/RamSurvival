using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
	public static EventManager instance = null;
	public event Func<Entity, Entity> OnResorceSelectAction;
	public Action OnUpdateUIAction;

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
		if (OnUpdateUIAction != null)
		{
			print("OnUpdateUIAction");
			OnUpdateUIAction();
		}
	}
}
