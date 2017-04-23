using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileGrid
{
	private int dimX;
	private int dimY;
	private Tile[,] grid;
	private int numRowsSky;

	public enum Direction
	{
		UP,
		RIGHT,
		DOWN,
		LEFT
	}

	public TileGrid(int _dimX, int _dimY, int _numRowsSky)
	{
		dimX = _dimX;
		dimY = _dimY;
		grid = new Tile[dimY, dimX];
		numRowsSky = _numRowsSky;
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

	public Tile[,] GetRawGrid()
	{
		return grid;
	}

	public Tile FindNearestTileOfType(Coordinate _coordinate, Type _tileType)
	{
		float dummyValue = -1.0f;
		return FindNearestTileOfType(_coordinate, _tileType, ref dummyValue);
	}

	public Tile FindNearestTileOfType(Coordinate _coordinate, Type _tileType, ref float distance)
	{
		// find all tiles of that type
		List<Tile> matchingTiles = new List<Tile>();
		for (int i = 0; i < grid.GetLength(0); i++)
		{
			for (int j = 0; j < grid.GetLength(1); j++)
			{
				Tile t = grid[i, j].GetComponent<Tile>();

				if (t && (t.GetType() == _tileType))
				{
					matchingTiles.Add(t);
				}
			}
		}

		// find closest one
		float shortestDistance = -1.0f;
		Tile returnTile = null;
		for (int i = 0; i < matchingTiles.Count; i++)
		{
			float _distance = Vector2.Distance(_coordinate.ToVector2(), matchingTiles[i].GetCoordinate().ToVector2());
			if (returnTile == null || _distance < shortestDistance)
			{
				shortestDistance = _distance;
				returnTile = matchingTiles[i];
			}
		}

		distance = shortestDistance;
		return returnTile;
	}

	public int GetDepth(Coordinate c)
	{
		return c.y - numRowsSky;
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

	public Vector2 ToVector2 ()
	{
		return new Vector2(x, y);
	}
}
