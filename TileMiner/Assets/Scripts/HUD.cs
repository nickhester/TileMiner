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
		GameObject.FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);
		UpdateResourceValues();
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.PLAYER_ACTION)
		{
			UpdateResourceValues();
		}
	}

	void UpdateResourceValues()
	{
		int numMinerals = player.GetInventory().GetResource(typeof(ResourceMineral));
		mineralCount.text = numMinerals.ToString();
	}
}
