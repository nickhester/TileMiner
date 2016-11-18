using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class WeightAnalyzer
{
	public static bool CanTileBeRemoved(Tile t)
	{
		List<Tile> remainingColumn = GetContiguousTileColumn(t, TileGrid.Direction.UP);
		int weightTotal = 0;
		for (int i = 0; i < remainingColumn.Count; i++)
		{
			weightTotal += remainingColumn[i].GetWeightSupportValue();
		}
		return (weightTotal >= 0);
	}

	public static bool CanStructureBeAddedHere(Tile t, int _addedWeight)
	{
		List<Tile> columnBelow = GetContiguousTileColumn(t, TileGrid.Direction.DOWN);
		int weightTotal = 0;
		for (int i = 0; i < columnBelow.Count; i++)
		{
			weightTotal += columnBelow[i].GetWeightSupportValue();
		}
		return (weightTotal + _addedWeight >= 0);
	}

	static List<Tile> GetContiguousTileColumn(Tile t, TileGrid.Direction _direction)
	{
		// the resulting list 1st element will be the tile nearest the starting tile, then outward. It will not include the starting tile.
		TileGrid tileGrid = t.GetTileGrid();
		Tile tileToCheck = tileGrid.GetTileNeighbor(_direction, t.GetCoordinate());

		List<Tile> contiguousTilesInDirection = new List<Tile>();
		while(true)
		{
			if (tileToCheck != null && tileToCheck.GetWeightSupportValue() != 0)
			{
				contiguousTilesInDirection.Add(tileToCheck);
			}
			else
			{
				break;
			}

			tileToCheck = tileGrid.GetTileNeighbor(_direction, tileToCheck.GetCoordinate());
		}
		return contiguousTilesInDirection;
	}
}
