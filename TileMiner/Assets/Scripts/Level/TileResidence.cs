using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileResidence : Tile
{
	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		if (!player.GetCity().hasBeenBuilt)
			player.ReportCityBuilt();
	}

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
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection, Player player)
	{
		bool isValid = true;

		if (_tileGrid.GetDepth(_myCoordinate) < 0)
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, 1, "Not Above Ground."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		// make sure there's space for the layout
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

		// if there's already a residence built, don't ever show the option again
		// TODO: is this what I want?
		if (player.GetCity().hasBeenBuilt)
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
