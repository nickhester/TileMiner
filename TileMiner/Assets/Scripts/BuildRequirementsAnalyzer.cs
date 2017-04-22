using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class BuildRequirementsAnalyzer
{

	public static bool IsWithinRangeOfMine(Coordinate c, TileGrid tileGrid)
	{
		float distanceToNearestMine = -1.0f;
		Tile t = tileGrid.FindNearestTileOfType(c, typeof(TileMine), ref distanceToNearestMine);
		if (t)
		{
			TileMine mine = t.GetComponent<TileMine>();
			if (distanceToNearestMine < mine.radiusToSupport)
			{
				return true;
			}
		}

		return false;
	}

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
}
