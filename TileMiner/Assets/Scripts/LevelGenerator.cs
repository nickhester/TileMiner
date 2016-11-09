using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField] private int mapHeight = 20;
	[SerializeField] private int mapWidth = 10;
	[SerializeField] private int numSkyTiles = 2;

	[SerializeField] private float tileSpacing = 1.0f;
	[SerializeField] private List<GameObject> tilePrefabs = new List<GameObject>();
	private TileGrid tileGrid;

	public enum TileType
	{
		EMPTY,
		DIRT
	}

	void Start ()
	{
		tileGrid = new TileGrid(mapWidth, mapHeight);
		CreateTiles();
	}

	void CreateTiles()
	{
		float verticalOffset = ((numSkyTiles) * tileSpacing);			// offset to make ground appear in the middle
		float horizontalOffset = ((mapWidth - 1) * tileSpacing) / 2.0f;	// offset to center left-right

		for (int i = 0; i < mapHeight; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				Tile newTile = CreateOneTile(
					new Vector2((j * tileSpacing) - horizontalOffset, (-i * tileSpacing) + verticalOffset),
					ChooseNextTileType(j, i));
				newTile.transform.SetParent(transform);
				newTile.Initialize(tileGrid, new Coordinate(j, i));
				tileGrid.AddTile(j, i, newTile);
			}
		}
	}

	TileType ChooseNextTileType(int dimX, int dimY)
	{
		if (dimY >= numSkyTiles)
		{
			return TileType.DIRT;
		}
		return TileType.EMPTY;
	}

	Tile CreateOneTile(Vector2 _position, TileType _type)
	{
		GameObject go = Instantiate(tilePrefabs[(int)_type], _position, Quaternion.identity) as GameObject;
		return go.GetComponent<Tile>(); ;
	}

	/*
	public Tile CreateTileDuringGame(TileType _type)
	{
		Tile returnTile = CreateOneTile
	}
	*/
}
