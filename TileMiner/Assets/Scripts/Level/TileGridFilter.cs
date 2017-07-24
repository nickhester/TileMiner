using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGridFilter
{
	TileGrid tileGrid;

	public TileGridFilter(TileGrid _tileGrid)
	{
		if (_tileGrid == null)
			Debug.LogError("TileGridFilter ctor received null TileGrid");

		tileGrid = _tileGrid;
	}

	public List<Tile> GetAllStructureTiles()
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

	public int GetNumOfTileType(System.Type _type)
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
