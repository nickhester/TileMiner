using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TileGridFilter
{
	public static List<Tile> GetAllStructureTiles(TileGrid tileGrid)
	{
		Tile[,] grid = tileGrid.GetRawGrid();
		List<Tile> returnList = new List<Tile>();

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j].isStructure)
				{
					returnList.Add(grid[i, j]);
				}
			}
		}

		return returnList;
	}

	public static int GetNumOfTileType(TileGrid tileGrid, System.Type _type)
	{
		Tile[,] grid = tileGrid.GetRawGrid();
		int returnCount = 0;

		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j].GetType() == _type)
				{
					returnCount++;
				}
			}
		}

		return returnCount;
	}
}
