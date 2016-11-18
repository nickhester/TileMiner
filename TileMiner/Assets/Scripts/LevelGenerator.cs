﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField] private int mapHeight = 20;
	[SerializeField] private int mapWidth = 10;
	[SerializeField] private int numSkyTiles = 2;

	[SerializeField] private float tileSpacing = 1.0f;
	[SerializeField] private List<GameObject> tilePrefabs = new List<GameObject>();
	private TileGrid tileGrid;

	// level generation
	[SerializeField] private float baseStoneChance = 0.1f;
	[SerializeField] private float rowIncreaseStoneChance = 0.05f;

	void Start ()
	{
		// check tile types vs prefabs
		if (tilePrefabs.Count != Enum.GetValues(typeof(Tile.TileType)).Length)
		{
			Debug.LogWarning("number of tile types and prefabs are different");
		}

		tileGrid = new TileGrid(mapWidth, mapHeight);
		CreateTiles();
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
			if (UnityEngine.Random.Range(0.0f, 1.0f) < (baseStoneChance + (rowIncreaseStoneChance * dimY)))
			{
				return Tile.TileType.STONE;
			}
			else
			{
				return Tile.TileType.DIRT;
			}
		}
		return Tile.TileType.EMPTY;
	}

	public Tile CreateOneTile(Coordinate _coordinate, Tile.TileType _type)
	{
		float verticalOffset = ((numSkyTiles) * tileSpacing);				// offset to make ground appear in the middle
		float horizontalOffset = ((mapWidth - 1) * tileSpacing) / 2.0f;		// offset to center left-right

		GameObject go = Instantiate(tilePrefabs[(int)_type], new Vector2((_coordinate.x * tileSpacing) - horizontalOffset, (-_coordinate.y * tileSpacing) + verticalOffset), Quaternion.identity) as GameObject;
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
