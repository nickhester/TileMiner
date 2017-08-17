using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BuildRequirementsAnalyzer
{

	public static bool IsWithinRangeOfTile(Coordinate c, TileGrid tileGrid, System.Type type, float distance)
	{
		float distanceToNearest = -1.0f;
		Tile t = tileGrid.FindNearestTileOfType(c, type, ref distanceToNearest);
		if (t)
		{
			if (distanceToNearest < distance)
			{
				return true;
			}
		}

		return false;
	}

	public static bool IsNotPastHeightLimit(Coordinate c, TileGrid tileGrid, System.Type classType, int limit)
	{
		return IsNotPastHeightLimit(c, tileGrid, Tile.TileType.EMPTY, classType, limit, true);
	}

	public static bool IsNotPastHeightLimit(Coordinate c, TileGrid tileGrid, Tile.TileType tileType, int limit)
	{
		return IsNotPastHeightLimit(c, tileGrid, tileType, null, limit, false);
	}

	private static bool IsNotPastHeightLimit(Coordinate c, TileGrid tileGrid, Tile.TileType tileType, System.Type classType, int limit, bool usingSystemType)
	{
		Tile currentTile = tileGrid.GetTileAt(c);
		int foundOfType = 0;
		while (true)
		{
			currentTile = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, currentTile.GetCoordinate());
			if (usingSystemType)
			{
				if (currentTile.GetType() == classType)
					foundOfType++;
				else
					break;
			}
			else
			{
				if (currentTile.GetTileType() == tileType)
					foundOfType++;
				else break;
			}
		}

		if (foundOfType < limit)
		{
			return true;
		}
		return false;
	}
}
