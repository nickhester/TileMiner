﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.Level;
using System.Linq;

public class LevelGenerator : MonoBehaviour
{
	public int MapHeight = 20;
	public int MapWidth = 10;
	public int NumSkyTiles = 14;

	int numRewardTilesAtBottom = 4;

	[SerializeField] private float tileSpacing = 1.0f;
	[SerializeField] private List<Tile> tilePrefabs = new List<Tile>();
	private TileGrid tileGrid;
	private LevelDefinition levelDefinition;

	private float verticalOffset
	{
		get
		{
			// offset to make ground appear in the middle
			return ((NumSkyTiles) * tileSpacing);
		}
		set { }
	}

	private float horizontalOffset
	{
		get
		{
			// offset to center left-right
			return ((MapWidth - 1) * tileSpacing) / 2.0f;
		}
		set { }
	}

	private List<TileGroup> TileGroups = new List<TileGroup>();

	// static ref to singleton
	private static LevelGenerator instance;
	public static LevelGenerator Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<LevelGenerator>();
			}
			return instance;
		}
	}

	public void Initialize()
	{
		Initialize(null, null);
	}

	public void Initialize(LevelDefinition levelDefinition, List<TechSettingsDefinition> techSettingsDefinitions)
	{
		// create local reference copies of all tile prefabs
		for (int i = 0; i < tilePrefabs.Count; i++)
		{
			GameObject instance = Instantiate(tilePrefabs[i].gameObject) as GameObject;
			Tile instanceTile = instance.GetComponent<Tile>();
			instance.SetActive(false);
			tilePrefabs[i] = instanceTile;
		}

		// check tile types vs prefabs
		if (tilePrefabs.Count != Enum.GetValues(typeof(Tile.TileType)).Length)
		{
			Debug.LogWarning("number of tile types and prefabs are different");
		}

		// implement level definition
		this.levelDefinition = levelDefinition;
		if (levelDefinition != null)
		{
			if (levelDefinition.mapHeight != null) { MapHeight = (int)(levelDefinition.mapHeight); }
			if (levelDefinition.numSkyTiles != null) { NumSkyTiles = (int)(levelDefinition.numSkyTiles); }
		}

		// generate level
		tileGrid = new TileGrid(MapWidth, MapHeight, NumSkyTiles);
		CreateTiles();
		
		GetComponent<LightManager>().Initialize(tileGrid);
		GetComponent<PathManager>().Initialize(tileGrid);

		ImplementTechSettings(techSettingsDefinitions);
	}

	void OnDestroy()
	{
		instance = null;
	}

	void ImplementTechSettings(List<TechSettingsDefinition> techSettingsDefinitions)
	{
		// implement tech settings definition
		if (techSettingsDefinitions != null)
		{
			foreach (var tile in tilePrefabs)
			{
				tile.IsStructureAvailable = true;
				foreach (var setting in techSettingsDefinitions)
				{
					if (TileTypeToEnumTileType(tile.GetType()) == setting.tileType)
					{
						tile.SetTechSettings(setting);
						break;
					}
				}
			}
		}
	}

	/// Level generation logic
	void CreateTiles()
	{
		Dictionary<Tile.TileType, int> tileCount = new Dictionary<Tile.TileType, int>();
		Dictionary<Tile.TileType, int> tileMinimumGuarantees = new Dictionary<Tile.TileType, int>();

		foreach (var tileGenerationInfo in levelDefinition.tileGenerationInfoList)
		{
			// add each minimum guarantee
			tileMinimumGuarantees.Add(tileGenerationInfo.tileType, tileGenerationInfo.guaranteeAtLeast);
			// pre-populate dictionary
			tileCount.Add(tileGenerationInfo.tileType, 0);
		}

		for (int i = 0; i < MapHeight; i++)
		{
			for (int j = 0; j < MapWidth; j++)
			{
				Tile.TileType _type = ChooseNextTileType(j, i);
				Coordinate coordinateToCreate = new Coordinate(j, i);
				CreateOneTile(coordinateToCreate, _type);

				if (_type != Tile.TileType.EMPTY)
					tileCount[_type]++;
			}
		}

		// add tile strips
		int groupNameIndex = 0;
		foreach (var stripInfo in levelDefinition.tileGenerationInfoList.Where(i => i.isStripType == true))
		{
			for (int i = 0; i < stripInfo.numStrips; i++)
			{
				// generate name for tile group
				TileGroup tileGroup = new TileGroup(stripInfo.tileType.ToString() + " " + groupNameIndex);

				// get even distribution position
				int stripDepth = ((MapHeight - NumSkyTiles - numRewardTilesAtBottom)
					/ (int)stripInfo.numStrips * (i + 1));    // i + 1 so that a strip doesn't appear at the very top

				int stripDepthOffset = 0;
				for (int j = 0; j < MapWidth; j++)
				{
					// random offset
					stripDepthOffset += UnityEngine.Random.Range(-1, 2);

					int depthOfStripTile = stripDepth + stripDepthOffset + NumSkyTiles;
					Coordinate stripTileCoordinate = new Coordinate(j, Mathf.Min(depthOfStripTile, MapHeight - numRewardTilesAtBottom - 1));

					// increase tile count for tile added
					tileCount[stripInfo.tileType]++;

					// decrease tile count for tile removed
					tileCount[TileTypeToEnumTileType(tileGrid.GetTileAt(stripTileCoordinate).GetType())]--;

					// replace the tile
					Tile t = ReplaceOneTile(stripTileCoordinate, stripInfo.tileType);
					t.waveDefinition = levelDefinition.waveDefinitionList[groupNameIndex];
					
					tileGroup.tileLocations.Add(stripTileCoordinate);
				}
				TileGroups.Add(tileGroup);

				groupNameIndex++;
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
					TileGenerationInfo tileGenInfo = levelDefinition.GetTileGenerationInfoFromType(minGuarantee.Key);
					if ((tileGenInfo.depthRangeStart + NumSkyTiles) >= MapHeight)
						Debug.LogError("Tile Depth Range Start set to beyond depth of grid");

					int randX = UnityEngine.Random.Range(0, MapWidth);
					int randY = UnityEngine.Random.Range(
						tileGenInfo.depthRangeStart + NumSkyTiles, 
						Mathf.Min(tileGenInfo.depthRangeEnd + NumSkyTiles, MapHeight - 1));

					// verify that you're not replacing over another tile with a minimum guarantee, or a strip
					Tile tileBeingReplaced = tileGrid.GetTileAt(new Coordinate(randX, randY));
					TileGenerationInfo overwriteCandidateInfo = levelDefinition.GetTileGenerationInfoFromType(tileBeingReplaced.GetTileType());
					if (overwriteCandidateInfo.guaranteeAtLeast > 0
						|| overwriteCandidateInfo.isStripType == true)
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

	private Tile.TileType ChooseNextTileType(int dimX, int dimY)
	{
		if (dimY >= NumSkyTiles)
		{
			int depth = dimY - NumSkyTiles;

			List<TileProbability> tileProbabilities = new List<TileProbability>();
			List<TileGenerationInfo> tileGenInfoList = levelDefinition.tileGenerationInfoList;
			for (int i = 0; i < tileGenInfoList.Count; i++)
			{
				TileProbability tp = new TileProbability(
					tileGenInfoList[i].tileType, 
					tileGenInfoList[i].baseProbability, 
					tileGenInfoList[i].increaseProbabilityPerRow, 
					tileGenInfoList[i].depthRangeStart, 
					tileGenInfoList[i].depthRangeEnd);
				tileProbabilities.Add(tp);
			}
			
			ProbabilitySelector probabilitySelector = new ProbabilitySelector(tileProbabilities);
			return probabilitySelector.GetTileType(depth);
		}
		return Tile.TileType.EMPTY;
	}

	private int ChooseTileAtDepth(int depth, float[] probabilitiesBase, float[] probabilitiesIncreaseWithDepth)
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

	private Tile CreateOneTile(Coordinate _coordinate, Tile.TileType _type)
	{
		GameObject go = Instantiate(tilePrefabs[(int)_type].gameObject, GetWorldSpacePositionFromCoordinate(_coordinate), Quaternion.identity) as GameObject;
		go.SetActive(true);		// local reference copy was set as inactive, so it has to be activated
		Tile t = go.GetComponent<Tile>();

		t.transform.SetParent(transform);
		t.Initialize(tileGrid, _coordinate, _type);
		tileGrid.AddTile(_coordinate, t);

		// initialize light source if it has one
		LightSource lightSource = go.GetComponent<LightSource>();
		if (lightSource != null)
			lightSource.Initialize();

		EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.LEVEL_CHANGE);

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
		Tile oldTile = tileGrid.GetTileAt(_coordinate);
		oldTile.OnDestroyTile();

		ReplaceOneTile(_coordinate, Tile.TileType.EMPTY);
	}
	
	public bool MoveTile(Coordinate _coordMoveStart, Coordinate _coordMoveEnd, Tile.TileType _typeToLeaveBehind)
	{
		Tile tileAtEnd = tileGrid.GetTileAt(_coordMoveEnd);
		Tile tileToMove = tileGrid.GetTileAt(_coordMoveStart);

		if (tileAtEnd == null || tileToMove == null)
			return false;

		if (tileAtEnd.GetTileType() != Tile.TileType.EMPTY)
			Debug.LogError("Trying to move tile to non-empty location");

		// replace in tileGrid
		Destroy(tileAtEnd.gameObject);
		tileGrid.AddTile(_coordMoveEnd, tileToMove);

		// update transform of existing tile
		Vector2 newPosition = GetWorldSpacePositionFromCoordinate(_coordMoveEnd);
		tileToMove.transform.position = newPosition;
		tileToMove.UpdateCoordinates(_coordMoveEnd);

		// create new tile to leave behind
		CreateOneTile(_coordMoveStart, _typeToLeaveBehind);
		return true;
	}

	public Tile GetTilePrefab(Tile.TileType _type)
	{
		return tilePrefabs[(int)_type].GetComponent<Tile>();
	}

	public Tile.TileType TileTypeToEnumTileType(Type t)
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

	public Type TileTypeEnumToTileType(Tile.TileType t)
	{
		return tilePrefabs[(int)t].GetType();
	}

	public TileGrid GetTileGrid()
	{
		return tileGrid;
	}

	public List<Tile.TileType> GetAvailableTileTypes()
	{
		List<Tile.TileType> availableTiles = new List<Tile.TileType>();
		foreach (var tilePrefab in tilePrefabs)
		{
			if (tilePrefab.IsStructureAvailable)
			{
				availableTiles.Add(tilePrefab.GetTileType());
			}
		}
		return availableTiles;
	}

	public Coordinate GetClosestTileCoordinateFromWorldSpacePosition(Vector2 v)
	{
		return new Coordinate(Mathf.RoundToInt((v.x + horizontalOffset) / tileSpacing), Mathf.RoundToInt(-(v.y - verticalOffset) / tileSpacing));
	}

	public Vector2 GetWorldSpacePositionFromCoordinate(Coordinate c)
	{
		return new Vector2((c.x * tileSpacing) - horizontalOffset, (-c.y * tileSpacing) + verticalOffset);
	}

	public List<Coordinate> GetTilesInGroup(Coordinate c)
	{
		foreach (var tileGroup in TileGroups)
		{
			foreach (var loc in tileGroup.tileLocations)
			{
				if (c == loc)
				{
					return tileGroup.tileLocations;
				}
			}
		}
		return null;
	}

	public TileGroup GetTileGroup(Coordinate c)
	{
		foreach (var tileGroup in TileGroups)
		{
			foreach (var loc in tileGroup.tileLocations)
			{
				if (c == loc)
				{
					return tileGroup;
				}
			}
		}
		return null;
	}
}
