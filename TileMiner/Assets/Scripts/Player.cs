using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour, IEventSubscriber
{
	private Inventory myInventory;
	public ActionOptionMenu actionOptionMenuPrefab;

	void Start ()
	{
		myInventory = new Inventory();
		FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.TILE_COLLECTED_DIRT, this);
	}
	
	void Update ()
	{

	}

	public Inventory GetInventory()
	{
		return myInventory;
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.TILE_COLLECTED_DIRT)
		{
			myInventory.AddResource(new ResourceDirt(1));
		}
	}
}
