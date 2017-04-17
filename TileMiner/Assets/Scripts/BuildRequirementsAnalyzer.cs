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
}
