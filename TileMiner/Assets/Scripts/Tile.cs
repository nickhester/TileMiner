﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Tile : MonoBehaviour
{
	protected TileGrid tileGrid;
	protected Coordinate myCoordinate;
	protected EventBroadcast eventBroadcast;
	private CameraControl cameraControl;
	protected Player player;
	protected bool hasBeenInitialized = false;

	[SerializeField] protected int weightSupportValue = 0;
	[SerializeField] protected int mineralAdjustmentToBuild = 0;
	[SerializeField] protected int mineralAdjustmentToDestroy = 0;
	public bool isStructure = false;
	protected bool isStructureActive = true;

	[SerializeField] protected int populationAdjustment = 0;

	[Header("Level Generation")]
	public float baseProbability;
	public float increaseProbabilityPerRow;
	public int depthRangeStart = 0;
	public int depthRangeEnd = 999;
	public int guaranteeAtLeast = 0;

	public enum TileType
	{
		EMPTY,
		DIRT,
		DIRT2,
		STONE,
		STONE2,
		BEACON,
		RESIDENCE,
		MINE,
		REFINERY,
		QUARRY,
		STATION,
		ENERGY_WELL
	}
	private TileType myTileType;

	private int brightnessLevel = 0;
	public int maxBrightness = 5;
	private Color originalColor;
	private SpriteRenderer spriteRenderer;

	public virtual void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _tileType)
	{
		tileGrid = _tileGrid;
		myCoordinate = _coordinate;
		eventBroadcast = FindObjectOfType<EventBroadcast>();
		cameraControl = FindObjectOfType<CameraControl>();
		player = FindObjectOfType<Player>();
		myTileType = _tileType;

		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		UpdateBrightness();

		hasBeenInitialized = true;
	}
	
	void OnMouseUpAsButton()
	{
		if (FindObjectOfType<ActionOptionMenu>() == null)
		{
			bool isCameraDragging = false;
			float timeSinceCameraDragEnded = -1.0f;
			cameraControl.GetDraggingStatus(ref isCameraDragging, ref timeSinceCameraDragEnded);
			if (!isCameraDragging || timeSinceCameraDragEnded > 0.001f)
			{
				PlayerClick();
			}
		}
	}

	protected abstract void PlayerClick();

	public abstract void Activate();

	public Coordinate GetCoordinate()
	{
		return myCoordinate;
	}

	public bool GetIsExposed()
	{
		int numDirections = Enum.GetNames(typeof(TileGrid.Direction)).Length;
		for (int i = 0; i < numDirections; i++)
		{
			Tile neighborTile = tileGrid.GetTileNeighbor((TileGrid.Direction)i, GetCoordinate());       // TODO: make "null" neighbors not count toward being exposed
			if (neighborTile && 
				(neighborTile.GetComponent<TileEmpty>() != null
					|| neighborTile.isStructure))
			{
				return true;
			}
		}
		return false;
	}

	protected void ProposeActions(List<NamedActionSet> _actions)
	{
		player.ProposeActions(_actions);
	}

	public TileGrid GetTileGrid()
	{
		return tileGrid;
	}

	public int GetWeightSupportValue()
	{
		return weightSupportValue;
	}
	
	public virtual Resource GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return new Resource(mineralAdjustmentToBuild, Resource.ResourceType.MINERAL);
	}
	protected Resource GetMineralAdjustmentToBuild_stacked(TileGrid _tileGrid, Coordinate buildTarget, float multiplier)
	{
		Tile targetTile = _tileGrid.GetTileAt(buildTarget);
		Tile currentTileDown = targetTile;
		int numLikeTilesBelow = 0;
		while (true)
		{
			Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, currentTileDown.GetCoordinate());
			if (tileBelow.GetType() == this.GetType())
			{
				currentTileDown = tileBelow;
				numLikeTilesBelow++;
			}
			else
			{
				break;
			}
		}
		
		return new Resource((int)(mineralAdjustmentToBuild * Mathf.Pow(multiplier, numLikeTilesBelow)), Resource.ResourceType.MINERAL);
	}

	public virtual int GetMineralAdjustmentToDestroy()
	{
		return mineralAdjustmentToDestroy;
	}
	
	public virtual bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		return true;
	}

	public int GetPopulationAdjustment()
	{
		return populationAdjustment;
	}

	public static string GetTileNameByEnum(TileType type)
	{
		switch (type)
		{
			case TileType.EMPTY:
				return "Empty";
			case TileType.DIRT:
				return "Dirt";
			case TileType.DIRT2:
				return "Dirt 2";
			case TileType.STONE:
				return "Stone";
			case TileType.STONE2:
				return "Stone 2";
			case TileType.BEACON:
				return "Beacon";
			case TileType.RESIDENCE:
				return "Residence";
			case TileType.MINE:
				return "Mine";
			case TileType.REFINERY:
				return "Refinery";
			case TileType.QUARRY:
				return "Quarry";
			case TileType.STATION:
				return "Mine";
			case TileType.ENERGY_WELL:
				return "Energy Well";
			default:
				return "ERROR OUT OF RANGE";
		}
	}
	
	public TileType GetTileType()
	{
		if (!hasBeenInitialized)
		{
			Debug.LogError("Cannot get myTileType on prefab - not instantiated yet.");
		}
		return myTileType;
	}

	public void Brighten()
	{
		Brighten(1);
	}

	public void Brighten(int _amount)
	{
		SetBrightnessLevel(_amount + GetBrightnessLevel());
	}

	private void UpdateBrightness()
	{
		if (myTileType != TileType.EMPTY)
		{
			Color currentColor = Color.Lerp(Color.black, originalColor, (brightnessLevel / (float)maxBrightness));

			spriteRenderer.color = currentColor;
		}
	}

	public int GetBrightnessLevel()
	{
		return brightnessLevel;
	}

	public void SetBrightnessLevel(int _level)
	{
		brightnessLevel = _level;
		brightnessLevel = Mathf.Min(brightnessLevel, maxBrightness);
		UpdateBrightness();

		isStructureActive = (brightnessLevel > 0 ? true : false);
	}

	public bool IsIlluminated()
	{
		return (GetBrightnessLevel() > 0);
	}
}
