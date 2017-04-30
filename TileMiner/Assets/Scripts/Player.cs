using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	private Inventory myInventory;
	public ActionOptionMenu actionOptionMenuPrefab;
	private ActionOptionMenu currentActionMenu;
	private EventBroadcast eventBroadcast;

	void Start ()
	{
		GetInventory();
		eventBroadcast = GameObject.FindObjectOfType<EventBroadcast>();
	}
	
	void Update ()
	{
		// HACK
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GetInventory().AddResource(new Resource(100, Resource.ResourceType.MINERAL));
		}
	}

	public Inventory GetInventory()
	{
		if (myInventory == null)
		{
			myInventory = new Inventory();
		}
		return myInventory;
	}

	public void ProposeActions(List<NamedActionSet> _actionSets)
	{
		// create menu
		if (currentActionMenu == null)
		{
			GameObject menu = Instantiate(actionOptionMenuPrefab.gameObject) as GameObject;
			currentActionMenu = menu.GetComponent<ActionOptionMenu>();
			currentActionMenu.Initialize(_actionSets, this);
		}
		else
		{
			Debug.LogError("Old action menu not null, trying to create new one");
		}
	}

	public void ExecuteAction(int _index, List<NamedActionSet> _actionSets)
	{
		bool actionIsCancel = false;
		for (int j = 0; j < _actionSets[_index].actions.Count; j++)
		{
			IAction a = _actionSets[_index].actions[j];
			
			if (a.GetType() == typeof(ActionCancel))
			{
				actionIsCancel = true;
			}

			_actionSets[_index].actions[j].Execute();
		}

		Destroy(currentActionMenu.gameObject);
		if (!actionIsCancel)
		{
			eventBroadcast.TriggerEvent(EventBroadcast.Event.PLAYER_ACTION);
		}
	}
}
