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
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionDestroy(this));
		actions.Add(
			new ActionAdjustResources(
				new Resource(GetMineralAdjustmentToDestroy(), Resource.ResourceType.MINERAL)));
		namedActionSet.Add(new NamedActionSet("Destroy Stone", actions));
		ProposeActions(namedActionSet);
	}

	public override int GetMineralAdjustmentToDestroy()
	{
		return mineralAdjustmentToDestroy + tileGrid.GetStoneCollectAdjustmentValue();
	}
}
