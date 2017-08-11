using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : Entity
{
	private Inventory myInventory;
	public ActionOptionMenu actionOptionMenuPrefab;
	private ActionOptionMenu currentActionMenu;

	// static ref to singleton
	private static Player instance;
	public static Player Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Player>();
			}
			return instance;
		}
	}

	void Start ()
	{
		GetInventory();
	}
	
	void Update ()
	{
		// HACK: Mineral Earn Cheat
		if (Input.GetKeyDown(KeyCode.M))
			GetInventory().AddResource(new Resource(100, Resource.ResourceType.MINERAL));
		if (Input.GetKeyDown(KeyCode.G))
			GetInventory().AddResource(new Resource(10, Resource.ResourceType.GOLD));
		if (Input.GetKeyDown(KeyCode.E))
			GetInventory().AddResource(new Resource(1, Resource.ResourceType.ENERGY));
		if (Input.GetKeyDown(KeyCode.T))
			GetInventory().AddResource(new Resource(1, Resource.ResourceType.ALIEN_TECH));
	}

	void OnDestroy()
	{
		instance = null;
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
			currentActionMenu.Initialize(_actionSets);
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
		
		if (!isActionCancel)
		{
			EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.PLAYER_ACTION);
		}
	}
}
