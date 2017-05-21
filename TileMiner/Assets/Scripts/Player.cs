using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Entity
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
		// HACK: Mineral Earn Cheat
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

	public override void ExecuteAction(NamedActionSet _actionSet)
	{
		bool isActionCancel = false;
		base.ExecuteActionBase(_actionSet, ref isActionCancel);
		
		Destroy(currentActionMenu.gameObject);
		if (!isActionCancel)
		{
			eventBroadcast.TriggerEvent(EventBroadcast.Event.PLAYER_ACTION);
		}
	}
}
