using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDirt : Tile
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
		actions.Add(new ActionDestroy(this));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.DIRT).GetMineralAdjustmentToBuild())));
		namedActionSet.Add(new NamedActionSet("Collect Mineral", actions, true));
		ProposeActions(namedActionSet);
	}
}
