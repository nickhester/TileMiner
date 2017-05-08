using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelGenerator : MonoBehaviour
{
	[SerializeField] private int mapHeight = 20;
	[SerializeField] private int mapWidth = 10;
	[SerializeField] private int numSkyTiles = 14;

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
		GetComponent<LightManager>().Initialize(tileGrid);
	}

	void CreateTiles()
	{
		Dictionary<Tile.TileType, int> tileCount = new Dictionary<Tile.TileType, int>();
		Dictionary<Tile.TileType, int> tileMinimumGuarantees = new Dictionary<Tile.TileType, int>();
		for (int i = 0; i < tilePrefabs.Count; i++)
		{
			tileMinimumGuarantees.Add((Tile.TileType)i, tilePrefabs[i].guaranteeAtLeast);
			tileCount.Add((Tile.TileType)i, 0);
		}

		for (int i = 0; i < mapHeight; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				Tile.TileType _type = ChooseNextTileType(j, i);
				Coordinate coordinateToCreate = new Coordinate(j, i);
				Tile t = CreateOneTile(coordinateToCreate, _type);

				tileCount[_type]++;
			}
		}

		// check if guaranteed tile quantities were met, and add more if not
		foreach (var minGuarantee in tileMinimumGuarantees)
		{
			if (minGuarantee.Value > 0)
			{
				while (minGuarantee.Value > tileCount[minGuarantee.Key])
				{
					// choose a place in the grid to add one
					if ((GetTilePrefab(minGuarantee.Key).depthRangeStart + numSkyTiles) >= mapHeight)
						Debug.LogError("Tile Depth Range Start set to beyond depth of grid");

					int randX = UnityEngine.Random.Range(0, mapWidth);
					int randY = UnityEngine.Random.Range(
						GetTilePrefab(minGuarantee.Key).depthRangeStart + numSkyTiles, 
						Mathf.Min(GetTilePrefab(minGuarantee.Key).depthRangeEnd + numSkyTiles, mapHeight - 1));

					// verify that you're not replacing another tile with a minimum guarantee
					Tile tileBeingReplaced = tileGrid.GetTileAt(new Coordinate(randX, randY));
					if (tileBeingReplaced.guaranteeAtLeast > 0)
						continue;

					// increase tile count for tile added
					tileCount[minGuarantee.Key]++;

					// decrease tile count for tile removed
					tileCount[TileTypeToEnumTileType(tileGrid.GetTileAt(new Coordinate(randX, randY)).GetType())]--;

					// replace the tile
					ReplaceOneTile(new Coordinate(randX, randY), minGuarantee.Key);
				}
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
		t.Initialize(tileGrid, _coordinate, _type);
		tileGrid.AddTile(_coordinate, t);
		
		return t;
	}

	public Tile ReplaceOneTile(Coordinate _coordinate, Tile.TileType _newType)
	{
		Tile oldTile = tileGrid.GetTileAt(_coordinate);
		
		// get some attributes to pass from old tile to new tile
		int illuminationLevel = oldTile.GetBrightnessLevel();
		
		Destroy(oldTile.gameObject);
		Tile newTile = CreateOneTile(_coordinate, _newType);

		// set passed attributes
		newTile.SetBrightnessLevel(illuminationLevel);

		return newTile;
	}

	public void DestroyOneTile(Coordinate _coordinate)
	{
		ReplaceOneTile(_coordinate, Tile.TileType.EMPTY);
	}

	public Tile GetTilePrefab(Tile.TileType _type)
	{
		return tilePrefabs[(int)_type].GetComponent<Tile>();
	}

	public Tile.TileType TileTypeToEnumTileType(System.Type t)
	{
		for (int i = 0; i < tilePrefabs.Count; i++)
		{
			System.Type t2 = tilePrefabs[i].GetType();
			if (t2 == t)
			{
				return (Tile.TileType)i;
			}
		}
		Debug.LogError("TileToEnumTileType didn't find the tile type");
		return Tile.TileType.EMPTY;
	}

	public System.Type TileTypeEnumToTileType(Tile.TileType t)
	{
		return tilePrefabs[(int)t].GetType();
	}

	public int GetNumSkyTiles()
	{
		return numSkyTiles;
	}

	public int GetMapWidth()
	{
		return mapWidth;
	}
}
