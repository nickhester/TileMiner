using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField] private int mapHeight = 20;
	[SerializeField] private int mapWidth = 10;
	[SerializeField] private int numSkyTiles = 2;

	[SerializeField] private float tileSpacing = 1.0f;
	[SerializeField] private List<Tile> tilePrefabs = new List<Tile>();
	private TileGrid tileGrid;

	void Start ()
	{
		// check tile types vs prefabs
		if (tilePrefabs.Count != Enum.GetValues(typeof(Tile.TileType)).Length)
		{
			Debug.LogWarning("number of tile types and prefabs are different");
		}

		tileGrid = new TileGrid(mapWidth, mapHeight, numSkyTiles);
		CreateTiles();

		GetComponent<LevelManager>().Initialize(tileGrid);
	}

	void CreateTiles()
	{
		for (int i = 0; i < mapHeight; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				CreateOneTile(
					new Coordinate(j, i),
					ChooseNextTileType(j, i));
			}
		}
	}

	Tile.TileType ChooseNextTileType(int dimX, int dimY)
	{
		if (dimY >= numSkyTiles)
		{
			int depth = dimY - numSkyTiles;

			List<TileProbability> tileProbabilities = new List<TileProbability>();
			for (int i = 0; i < tilePrefabs.Count; i++)
			{
				TileProbability tp = new TileProbability((Tile.TileType)i, tilePrefabs[i].baseProbability, tilePrefabs[i].increaseProbabilityPerRow, tilePrefabs[i].depthRangeStart, tilePrefabs[i].depthRangeEnd);
				tileProbabilities.Add(tp);
			}

			ProbabilitySelector probabilitySelector = new ProbabilitySelector(tileProbabilities);
			return probabilitySelector.GetTileType(depth);
		}
		return Tile.TileType.EMPTY;
	}

	int ChooseTileAtDepth(int depth, float[] probabilitiesBase, float[] probabilitiesIncreaseWithDepth)
	{
		float rand = UnityEngine.Random.Range(0.0f, 1.0f);

		float aggregateValue = 0.0f;
		for (int i = 0; i < probabilitiesBase.Length; i++)
		{
			aggregateValue = probabilitiesBase[i] - (probabilitiesIncreaseWithDepth[i] * depth);
			if (rand - aggregateValue < 0.0f)
			{
				return i - 1;
			}
		}
		return probabilitiesBase.Length - 1;
	}

	public Tile CreateOneTile(Coordinate _coordinate, Tile.TileType _type)
	{
		float verticalOffset = ((numSkyTiles) * tileSpacing);				// offset to make ground appear in the middle
		float horizontalOffset = ((mapWidth - 1) * tileSpacing) / 2.0f;		// offset to center left-right

		GameObject go = Instantiate(tilePrefabs[(int)_type].gameObject, new Vector2((_coordinate.x * tileSpacing) - horizontalOffset, (-_coordinate.y * tileSpacing) + verticalOffset), Quaternion.identity) as GameObject;
		Tile t = go.GetComponent<Tile>();

		t.transform.SetParent(transform);
		t.Initialize(tileGrid, _coordinate);
		tileGrid.AddTile(_coordinate, t);
		
		return t;
	}

	public Tile GetTilePrefab(Tile.TileType _type)
	{
		return tilePrefabs[(int)_type].GetComponent<Tile>();
	}
}
