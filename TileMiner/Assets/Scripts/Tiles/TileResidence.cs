using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileResidence : Tile
{
	protected override void PlayerClick()
	{
		Activate();
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		
		// none; cannot destroy.

		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;
		
		// if there's already a residence built, don't ever show the option again
		if (City.Instance.hasBeenBuilt)
		{
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileResidence)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground or other Residence."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		return isValid;
	}
}
