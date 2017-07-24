using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class HUD : MonoBehaviour, IEventSubscriber
{
	Player player;
	City city;

	[SerializeField] private Text mineralCount;
	[SerializeField] private Text goldCount;
	[SerializeField] private Text energyCount;
	[SerializeField] private Text alienTechCount;

	[SerializeField] private Button cityUnlockableObjectPrefab;
	[SerializeField] private GameObject cityUnlockableParent;

	void Start()
	{
		player = GameObject.FindObjectOfType<Player>();
		city = player.GetCity();
		GameObject.FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.RESOURCE_VALUES_UPDATED, this);
		GameObject.FindObjectOfType<EventBroadcast>().SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);
		UpdateResourceValues();
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.RESOURCE_VALUES_UPDATED)
		{
			UpdateResourceValues();
		}
		else if (_event == EventBroadcast.Event.PLAYER_ACTION)
		{
			UpdateAvailableCityUnlockables();
		}
	}

	private void UpdateAvailableCityUnlockables()
	{
		// check which ones are available to display the status of
		List<CityBenefits> availableBenefits = city.GetAvailableCityBenefits();

		// destroy all children of parent object
		foreach (var child in cityUnlockableParent.GetComponentsInChildren<Button>())
			Destroy(child.gameObject);

		// create all new children objects for each available benefit
		foreach (var benefit in availableBenefits)
		{
			Button b = Instantiate(cityUnlockableObjectPrefab, cityUnlockableParent.transform);
			Text text = b.GetComponentInChildren<Text>();
			text.text = benefit.description;
		}
	}

	void UpdateResourceValues()
	{
		int numMinerals = player.GetInventory().GetResource(Resource.ResourceType.MINERAL);
		int numGold = player.GetInventory().GetResource(Resource.ResourceType.GOLD);
		int numEnergy = player.GetInventory().GetResource(Resource.ResourceType.ENERGY);
		int numAlienTech = player.GetInventory().GetResource(Resource.ResourceType.ALIEN_TECH);

		mineralCount.text = numMinerals.ToString();
		goldCount.text = numGold.ToString();
		energyCount.text = numEnergy.ToString();
		alienTechCount.text = numAlienTech.ToString();
	}
}
