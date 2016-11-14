using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionCollect : IAction
{
	Resource resource;
	int amount;

	public ActionCollect(Resource _resource, int _amount)
	{
		resource = _resource;
		amount = _amount;
	}

	public void Execute()
	{
		Player player = MonoBehaviour.FindObjectOfType<Player>();
		Inventory inventory = player.GetInventory();
		inventory.AddResource(resource, amount);
	}
}
