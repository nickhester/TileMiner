using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionBuildCity : IAction
{
	private Tile tileToReplace;
	List<Coordinate> coordinatesToReserve;

	public ActionBuildCity(Tile _tileToReplace)
	{
		tileToReplace = _tileToReplace;

		coordinatesToReserve = new List<Coordinate>();
		coordinatesToReserve.Add(new Coordinate(0, 0));
		coordinatesToReserve.Add(new Coordinate(-1, 0));
		coordinatesToReserve.Add(new Coordinate(1, 0));
		coordinatesToReserve.Add(new Coordinate(1, -1));
		coordinatesToReserve.Add(new Coordinate(0, -1));
		coordinatesToReserve.Add(new Coordinate(-1, -1));
		coordinatesToReserve.Add(new Coordinate(1, -2));
		coordinatesToReserve.Add(new Coordinate(0, -2));
		coordinatesToReserve.Add(new Coordinate(-1, -2));
	}

	public void Execute()
	{
		// city build logic
		Coordinate baseCoord = tileToReplace.GetCoordinate();
		List<Tile> tilesReservedForCity = new List<Tile>();
		foreach (var coord in coordinatesToReserve)
		{
			tilesReservedForCity.Add(LevelGenerator.Instance.ReplaceOneTile(baseCoord + coord, Tile.TileType.RESIDENCE));
		}
		City.Instance.Build(tileToReplace.GetCoordinate(), tilesReservedForCity);
	}

	public bool IsActionValid(ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool result = true;

		if (tileToReplace == null)
		{
			return false;
		}

		// check tiles around
		if (LevelGenerator.Instance.GetTileGrid().GetDepth(tileToReplace.GetCoordinate()) < 0)
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, 1, "Not Above Ground."));
			isExcludedFromPlayerSelection = true;
			result = false;
		}

		// make sure there's space for the layout
		if (HasClearanceOnSides(LevelGenerator.Instance.GetTileGrid(), tileToReplace.GetCoordinate(), coordinatesToReserve)
			&& LevelGenerator.Instance.GetTileGrid().GetIsGroundType(tileToReplace.GetCoordinate() + new Coordinate(-1, 1))     // if down and left is ground
			&& LevelGenerator.Instance.GetTileGrid().GetIsGroundType(tileToReplace.GetCoordinate() + new Coordinate(1, 1)))     // if down and right is ground
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_SPACE_AROUND, -1, "Must Have Available Flat Ground On Both Sides"));
			//isExcludedFromPlayerSelection = true;
			result = false;
		}

		return (result);
	}

	bool HasClearanceOnSides(TileGrid _tileGrid, Coordinate _myCoordinate, List<Coordinate> offsets)
	{
		foreach (var offset in offsets)
		{
			Tile t = _tileGrid.GetTileNeighbor(offset, _myCoordinate);

			if (t == null)
				return false;

			if (t.GetTileType() != Tile.TileType.EMPTY)
			{
				return false;
			}
		}
		return true;
	}
}
