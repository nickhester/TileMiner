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

	TileType ChooseNextTileType(int dimX, int dimY)
	{
		if (dimY >= numSkyTiles)
		{
			return TileType.DIRT;
		}
		return TileType.EMPTY;
	}

	public Tile CreateOneTile(Coordinate _coordinate, TileType _type)
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
}
