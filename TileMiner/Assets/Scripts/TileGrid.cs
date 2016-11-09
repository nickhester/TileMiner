using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGrid
{
	private int dimX;
	private int dimY;
	private Tile[,] grid;

	public enum Direction
	{
		UP,
		RIGHT,
		DOWN,
		LEFT
	}

	public TileGrid(int _dimX, int _dimY)
	{
		dimX = _dimX;
		dimY = _dimY;
		grid = new Tile[dimY, dimX];
	}

	public void AddTile(int _x, int _y, Tile _tile)
	{
		grid[_y, _x] = _tile;
	}

	public Tile GetTileAt(Coordinate _coordinate)
	{
		return grid[_coordinate.y, _coordinate.x];
	}

	public Coordinate GetCoordinate(Tile t)
	{
		Coordinate c = new Coordinate(-1, -1);
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				if (grid[i, j] == t)
				{
					c.x = j;
					c.y = i;
				}
			}
		}
		return c;
	}

	public Tile GetTileNeighbor(Direction _direction, Coordinate _coordinate)
	{
		Tile returnTile = null;
		Coordinate returnCoordinate = new Coordinate(-1, -1);
		switch (_direction)
		{
			case Direction.UP:
				{
					if (_coordinate.y >= 0)
					{
						returnCoordinate.x = _coordinate.x;
						returnCoordinate.y = _coordinate.y - 1;
					}
					break;
				}
			case Direction.RIGHT:
				{
					if (_coordinate.x <= (dimX - 1))
					{
						returnCoordinate.x = _coordinate.x + 1;
						returnCoordinate.y = _coordinate.y;
					}
					break;
				}
			case Direction.DOWN:
				{
					if (_coordinate.y <= (dimY - 1))
					{
						returnCoordinate.x = _coordinate.x;
						returnCoordinate.y = _coordinate.y + 1;
					}
					break;
				}
			case Direction.LEFT:
				{
					if (_coordinate.x >= 0)
					{
						returnCoordinate.x = _coordinate.x - 1;
						returnCoordinate.y = _coordinate.y;
					}
					break;
				}
			default:
				{
					Debug.LogError("Case not handled");
					break;
				}
		}

		if (returnCoordinate.x != -1)
		{
			returnTile = GetTileAt(returnCoordinate);
		}
		return returnTile;
	}
}

public struct Coordinate
{
	public int x, y;

	public Coordinate (int _x, int _y)
	{
		x = _x;
		y = _y;
	}
}
