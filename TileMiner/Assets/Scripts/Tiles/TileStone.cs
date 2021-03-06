﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileStone : Tile
{
	[SerializeField] protected int minAdjustmentValue;	// even after the quarry has been applied, still has a minimum cost

	protected override void PlayerClick()
	{
		if (GetIsExposed() && IsIlluminated())
		{
			Activate();
		}
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();

		namedActionSet.Add(new NamedActionSet("Mine Stone", GetDestroyAction()));
		
		ProposeActions(namedActionSet);
	}

	public override List<Resource> GetResourceAdjustmentToDestroy()
	{
		List<Resource> resources = new List<Resource>();
		resources.Add(new Resource(Mathf.Min(mineralAdjustmentToDestroy + tileGrid.GetStoneCollectAdjustmentValue(), -minAdjustmentValue), Resource.ResourceType.MINERAL));
		resources.Add(new Resource(goldAdjustmentToDestroy, Resource.ResourceType.GOLD));
		resources.Add(new Resource(energyAdjustmentToDestroy, Resource.ResourceType.ENERGY));
		resources.Add(new Resource(alienTechAdjustmentToDestroy, Resource.ResourceType.ALIEN_TECH));
		return resources;
	}
}
