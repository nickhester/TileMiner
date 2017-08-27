using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class WeightAnalyzer
{
	public static bool CanTileBeRemoved(Coordinate t)
	{
		Tile upperNeighbor = LevelGenerator.Instance.GetTileGrid().GetTileNeighbor(TileGrid.Direction.UP, t);
		if (upperNeighbor.isStructure)
		{
			return false;
		}

		List<Tile> remainingColumn = GetContiguousTileColumn(t, TileGrid.Direction.UP);
		int weightTotal = 0;
		for (int i = 0; i < remainingColumn.Count; i++)
		{
			weightTotal += remainingColumn[i].GetWeightSupportValue();
		}
		return (weightTotal >= 0);
	}

	public static bool CanStructureBeAddedHere(Coordinate t, int _addedWeight)
	{
		List<Tile> columnBelow = GetContiguousTileColumn(t, TileGrid.Direction.DOWN);
		int weightTotal = 0;
		for (int i = 0; i < columnBelow.Count; i++)
		{
			weightTotal += columnBelow[i].GetWeightSupportValue();
		}
		return (weightTotal + _addedWeight >= 0);
	}

	static List<Tile> GetContiguousTileColumn(Coordinate t, TileGrid.Direction _direction)
	{
		// the resulting list 1st element will be the tile nearest the starting tile, then outward. It will not include the starting tile.
		Tile tileToCheck = LevelGenerator.Instance.GetTileGrid().GetTileNeighbor(_direction, t);

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

			tileToCheck = LevelGenerator.Instance.GetTileGrid().GetTileNeighbor(_direction, tileToCheck.GetCoordinate());
		}
		return contiguousTilesInDirection;
	}
}
