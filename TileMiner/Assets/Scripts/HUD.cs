using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour, IEventSubscriber
{
	Player player;

	[SerializeField] private Text mineralCount;
	[SerializeField] private Text energyCount;

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
		int numEnergy = player.GetInventory().GetResource(Resource.ResourceType.ENERGY);
		mineralCount.text = numMinerals.ToString();
		energyCount.text = numEnergy.ToString();
	}
}
