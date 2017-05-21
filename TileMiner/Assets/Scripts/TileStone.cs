using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileStone : Tile
{
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

		namedActionSet.Add(new NamedActionSet("Destroy Stone", GetDestroyAction()));
		
		ProposeActions(namedActionSet);
	}

	public override Resource GetResourceAdjustmentToDestroy()
	{
		return new Resource(Mathf.Min(mineralAdjustmentToDestroy + tileGrid.GetStoneCollectAdjustmentValue(), 0), Resource.ResourceType.MINERAL);
	}
}
