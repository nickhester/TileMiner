﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionAdjustResources : IAction
{
	Resource resource;
	int amount;
	Inventory inventory;
	bool isAdjustmentPossible;

	public ActionAdjustResources(Resource _resource)
	{
		resource = _resource;

		// check if enough resources
		Player player = MonoBehaviour.FindObjectOfType<Player>();
		inventory = player.GetInventory();
		isAdjustmentPossible = ((inventory.GetResource(typeof(ResourceMineral)) + resource.GetAmount()) >= 0);
	}

	public void Execute()
	{
		inventory.AddResource(resource);
	}

	public bool IsActionValid()
	{
		return isAdjustmentPossible;
	}
}
