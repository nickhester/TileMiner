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
		actions.Add(new ActionBuild(GetCoordinate(), TileType.RIFT));

		// replace all tiles in strip group with other resources
		List<Coordinate> stripGroupCoords = LevelGenerator.Instance.GetTilesInGroup(GetCoordinate());
		if (stripGroupCoords != null)
		{
			foreach (var coords in stripGroupCoords)
			{
				// for all except this one, spawn strip reward
				if (coords != GetCoordinate())
				{
					actions.Add(new ActionBuild(coords, TileType.DIRT2, false));
				}
			}
		}

		actions.Add(new ActionAdjustResources(
						new Resource(1, Resource.ResourceType.ENERGY)));
		namedActionSet.Add(new NamedActionSet("Collect Energy Well", actions));
		ProposeActions(namedActionSet);
	}
}
