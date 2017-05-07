using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionAdjustResources : IAction
{
	Resource resource;
	Inventory inventory;
	bool isAdjustmentPossible;

	public ActionAdjustResources(Resource _resource)
	{
		resource = _resource;

		// check if enough resources
		Player player = MonoBehaviour.FindObjectOfType<Player>();
		inventory = player.GetInventory();
		isAdjustmentPossible = ((inventory.GetResource(resource.GetResourceType()) + resource.GetAmount()) >= 0);
	}

	public void Execute()
	{
		inventory.AddResource(resource);
	}

	public bool IsActionValid(ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		if (!isAdjustmentPossible)
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_ENOUGH_RESOURCES));
		}

		return isAdjustmentPossible;
	}

	public int GetResourceAdjustmentAmount()
	{
		return resource.GetAmount();
	}

	public string GetResourceAdjustmentName()
	{
		return resource.GetName();
	}
}
