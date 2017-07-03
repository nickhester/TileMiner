using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileResidence : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] protected float stackMultiplierCost = 2.0f;

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
		
		if (HasClearanceOnSides(_tileGrid, _myCoordinate, new List<Coordinate>() {		// if space around it is empty
			new Coordinate(-1, 0),
			new Coordinate(1, 0),
			new Coordinate(-1, -1),
			new Coordinate(0, -1),
			new Coordinate(1, -1)
		})		
			&& _tileGrid.GetIsGroundType(_myCoordinate + new Coordinate(-1, 1))		// if down and left is ground
			&& _tileGrid.GetIsGroundType(_myCoordinate + new Coordinate(1, 1)))		// if down and right is ground
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_SPACE_AROUND, -1, "Must Have Available Flat Ground On Both Sides"));
			//isExcludedFromPlayerSelection = true;
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

	public override List<Resource> GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetResourceAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}

	bool HasClearanceOnSides(TileGrid _tileGrid, Coordinate _myCoordinate, List<Coordinate> offsets)
	{
		foreach (var offset in offsets)
		{
			Tile t = _tileGrid.GetTileNeighbor(offset, _myCoordinate);

			if (t == null)
				return false;

			if (t.GetTileType() != TileType.EMPTY)
			{
				return false;
			}
		}
		return true;
	}
}
