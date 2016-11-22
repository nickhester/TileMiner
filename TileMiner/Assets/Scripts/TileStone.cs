﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileStone : Tile
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
					MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.STONE).GetMineralAdjustmentToBuild())));
		namedActionSet.Add(new NamedActionSet("Destroy Stone", actions));
		ProposeActions(namedActionSet);
	}
}
