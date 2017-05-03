using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDirt : Tile
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
		namedActionSet.Add(new NamedActionSet("Collect Mineral", actions, true));
		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground."));
			isValid = false;
		}
		
		return isValid;
	}
}
