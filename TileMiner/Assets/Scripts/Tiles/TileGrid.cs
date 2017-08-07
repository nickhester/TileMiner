using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileGrid
{
	public int dimX { get; private set; }
	public int dimY { get; private set; }
	private Tile[,] grid;
	private int numRowsSky;

	private int currentStoneCollectAdjustmentValue = 0;
	private EventBroadcast eventBroadcast;

	private List<Tile.TileType> groundTypes;

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

		groundTypes = new List<Tile.TileType>()
		{
			Tile.TileType.DIRT,
			Tile.TileType.DIRT2,
			Tile.TileType.STONE,
			Tile.TileType.STONE2
		};

		eventBroadcast = MonoBehaviour.FindObjectOfType<EventBroadcast>();
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
	
	public Tile GetTileNeighbor(Coordinate _offset, Coordinate _originCoordinate)
	{
		Coordinate coord = _originCoordinate;
		coord.x += _offset.x;
		coord.y += _offset.y;
		return GetTileAt(coord);
	}

	public List<Tile> GetTileNeighbors(Coordinate _originCoordinate)
	{
		List<Tile> returnList = new List<Tile>();

		for (int i = 0; i < Enum.GetNames(typeof(Direction)).Length; i++)
		{
			Tile t = GetTileNeighbor((Direction)i, _originCoordinate);
			if (t != null)
				returnList.Add(t);
		}
		return returnList;
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

	public void ReportStoneCollectAdjustmentValue(int v)
	{
		currentStoneCollectAdjustmentValue += v;
	}

	public int GetStoneCollectAdjustmentValue()
	{
		currentStoneCollectAdjustmentValue = 0;
		eventBroadcast.TriggerEvent(EventBroadcast.Event.PLAYER_SELECTED_STONE);
		// subscribers update currentStoneRebateValue at this point
		return currentStoneCollectAdjustmentValue;
	}

	public bool GetIsGroundType(Coordinate _coordinate)
	{
		Tile t = GetTileAt(_coordinate);
		return GetIsGroundType(t.GetTileType());
	}

	public bool GetIsGroundType(Tile.TileType _type)
	{
		if (groundTypes.Contains(_type))
		{
			return true;
		}
		return false;
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

	public static Coordinate operator + (Coordinate c1, Coordinate c2)
	{
		return new Coordinate(c1.x + c2.x, c1.y + c2.y);
	}

	public static Coordinate operator -(Coordinate c1, Coordinate c2)
	{
		return new Coordinate(c1.x - c2.x, c1.y - c2.y);
	}

	public override string ToString()
	{
		return "(" + x + "," + y + ")";
	}
}
