using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour, IEventSubscriber
{
	Player player;

	[SerializeField] Text mineralCount;

	void Start()
	{
		player = GameObject.FindObjectOfType<Player>();
		GameObject.FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.RESOURCE_VALUES_UPDATED, this);
		UpdateResourceValues();
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.RESOURCE_VALUES_UPDATED)
		{
			UpdateResourceValues();
		}
	}

	void UpdateResourceValues()
	{
		int numMinerals = player.GetInventory().GetResource(Resource.ResourceType.MINERAL);
		mineralCount.text = numMinerals.ToString();
	}
}
