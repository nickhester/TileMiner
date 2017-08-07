using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour, IEventSubscriber
{
	private TileGrid tileGrid;
	private LevelGenerator levelGenerator;
	private City city;

	public void Initialize(TileGrid _tileGrid)
	{
		tileGrid = _tileGrid;
		city = FindObjectOfType<Player>().GetCity();
		levelGenerator = GetComponent<LevelGenerator>();

		EventBroadcast.Instance.SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);
	}

	public void UpdateTilePathSteps()
	{
		// todo: make method a coroutine, or run on another thread

		// 2 queues: ones I'm checking now, and ones to check in the next phase
		Queue<Coordinate> tilesToCheck = new Queue<Coordinate>();
		Queue<Coordinate> tilesAdjacent = new Queue<Coordinate>();
		int currentStepValue = 0;

		// set all empty tiles to -1
		for (int x = 0; x < tileGrid.dimX; x++)
		{
			for (int y = 0; y < tileGrid.dimY; y++)
			{
				Coordinate c = new Coordinate(x, y);
				if (tileGrid.GetTileAt(c).GetTileType() == Tile.TileType.EMPTY
					|| tileGrid.GetTileAt(c).GetTileType() == Tile.TileType.RESIDENCE)
				{
					tileGrid.GetTileAt(c).numStepsFromCity = -1;
				}
			}
		}

		// add city tile(s) to queue 1
		tilesToCheck.Enqueue(city.GetCityTile().GetCoordinate());

		while (tilesToCheck.Count > 0)
		{
			// run through queue 1, setting all tiles in it to an incremental number, starting at 0
			while (tilesToCheck.Count > 0)
			{
				Coordinate c = tilesToCheck.Dequeue();

				// do another check now to see if it's been set since it was initially checked
				if (tileGrid.GetTileAt(c).numStepsFromCity == -1)
				{
					tileGrid.GetTileAt(c).numStepsFromCity = currentStepValue;
				}
				else
				{
					continue;
				}

				// also find all adjacent tiles, and add them to queue 2 
				List<Tile> neighbors = tileGrid.GetTileNeighbors(c);
				foreach (var n in neighbors)
				{
					// if the tiles are -1. if the tiles are empty.
					if ((n.GetTileType() == Tile.TileType.EMPTY || n.GetTileType() == Tile.TileType.RESIDENCE)	// TODO: The residence part will probably change once Cities work better
							&& tileGrid.GetTileAt(n.GetCoordinate()).numStepsFromCity == -1)
					{
						tilesAdjacent.Enqueue(n.GetCoordinate());
					}
				}
			}
			
			// then, increment the number, and move queue 2 into queue 1, and clear queue 2
			currentStepValue++;
			while (tilesAdjacent.Count > 0)
			{
				tilesToCheck.Enqueue(tilesAdjacent.Dequeue());
			}
			// loop
		}
	}
	
	public bool GetPathTargetPosition(Vector2 currentPos, ref Vector2 returnTargetPosition)
	{
		Coordinate currentCoord = levelGenerator.GetClosestTileCoordinateFromWorldSpacePosition(currentPos);
		Tile currentClosestTile = tileGrid.GetTileAt(currentCoord);
		if (currentClosestTile == null)
		{
			return false;
		}
		int currentStepNumber = currentClosestTile.numStepsFromCity;

		if (currentStepNumber < 0)
		{
			return false;
		}

		Coordinate targetCoord = new Coordinate(0, 0);
		List<TileGrid.Direction> directions = new List<TileGrid.Direction>();
		directions.Add(TileGrid.Direction.UP);
		directions.Add(TileGrid.Direction.LEFT);
		directions.Add(TileGrid.Direction.RIGHT);
		directions.Add(TileGrid.Direction.DOWN);

		// find lower step number
		bool returningValidValue = false;
		foreach (var dir in directions)
		{
			Tile t = tileGrid.GetTileNeighbor(dir, currentCoord);
			if (t != null && t.numStepsFromCity > 0 && t.numStepsFromCity < currentStepNumber)
			{
				targetCoord = t.GetCoordinate();
				returningValidValue = true;
				break;
			}
		}

		if (returningValidValue)
			returnTargetPosition = levelGenerator.GetWorldSpacePositionFromCoordinate(targetCoord);
		return returningValidValue;
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.PLAYER_ACTION)
		{
			UpdateTilePathSteps();
		}
	}
}
