using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileEnergyWell : Tile
{
	protected override void PlayerClick()
	{
		if (GetIsExposed())
		{
			Activate();
		}
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, TileType.RIFT));
		actions.Add(
			new ActionAdjustResources(
				new Resource(1, Resource.ResourceType.ENERGY)));
		namedActionSet.Add(new NamedActionSet("Collect Energy Well", actions));
		ProposeActions(namedActionSet);
	}
}
