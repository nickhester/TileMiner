using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
	private TileGrid tileGrid;
	private City city;

	public void Initialize(TileGrid _tileGrid)
	{
		tileGrid = _tileGrid;
		city = FindObjectOfType<Player>().GetCity();
	}

	public void UpdateTilePathSteps()
	{
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
				if (tileGrid.GetTileAt(c).GetTileType() == Tile.TileType.EMPTY)
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
}
