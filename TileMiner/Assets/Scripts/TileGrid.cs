using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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

	public void AddTile(Coordinate _coordinate, Tile _tile)
	{
		grid[_coordinate.y, _coordinate.x] = _tile;
	}

	public Tile GetTileAt(Coordinate _coordinate)
	{
		if (_coordinate.y < grid.GetLength(0)
			&& _coordinate.y >= 0
			&& _coordinate.x < grid.GetLength(1)
			&& _coordinate.x >= 0)
		{
			return grid[_coordinate.y, _coordinate.x];
		}
		return null;
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

	public Tile GetTileNeighbor(Direction _direction, Coordinate _originCoordinate)
	{
		Tile returnTile = null;
		Coordinate returnCoordinate = new Coordinate(-1, -1);
		switch (_direction)
		{
			case Direction.UP:
				{
					if (_originCoordinate.y >= 0)
					{
						returnCoordinate.x = _originCoordinate.x;
						returnCoordinate.y = _originCoordinate.y - 1;
					}
					break;
				}
			case Direction.RIGHT:
				{
					if (_originCoordinate.x <= (dimX - 1))
					{
						returnCoordinate.x = _originCoordinate.x + 1;
						returnCoordinate.y = _originCoordinate.y;
					}
					break;
				}
			case Direction.DOWN:
				{
					if (_originCoordinate.y <= (dimY - 1))
					{
						returnCoordinate.x = _originCoordinate.x;
						returnCoordinate.y = _originCoordinate.y + 1;
					}
					break;
				}
			case Direction.LEFT:
				{
					if (_originCoordinate.x >= 0)
					{
						returnCoordinate.x = _originCoordinate.x - 1;
						returnCoordinate.y = _originCoordinate.y;
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
