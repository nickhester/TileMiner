using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour, IEventSubscriber
{
	private Inventory myInventory;

	void Start ()
	{
		myInventory = new Inventory();
		FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.TILE_COLLECTED_DIRT, this);
	}
	
	void Update ()
	{

	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.TILE_COLLECTED_DIRT)
		{
			myInventory.AddDirt(1);
		}
	}

}
