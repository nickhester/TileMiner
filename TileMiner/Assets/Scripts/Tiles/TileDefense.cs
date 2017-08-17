using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDefense : Tile
{
	
	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);
	}

	protected override void PlayerClick()
	{
		Activate();
	}
	
	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		
		namedActionSet.Add(new NamedActionSet("Destroy", GetDestroyAction()));

		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileDefense)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		// check structure height
		bool passesHeightLimitCheck = BuildRequirementsAnalyzer.IsNotPastHeightLimit(_myCoordinate, _tileGrid, typeof(TileDefense), GetCurrentStackLimit(City.Instance));

		if (!passesHeightLimitCheck)
		{
			isExcludedFromPlayerSelection = true;
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_UNDER_STRUCTURE_HEIGHT_LIMIT, 3));
		}
		return isValid;
	}
}
