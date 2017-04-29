using UnityEngine;
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

	[SerializeField] protected int weightSupportValue = 0;
	[SerializeField] protected int mineralAdjustmentToBuild = 0;
	[SerializeField] protected int mineralAdjustmentToDestroy = 0;
	public bool isStructure = false;

	[SerializeField] protected int populationAdjustment = 0;

	[Header("Level Generation")]
	public float baseProbability;
	public float increaseProbabilityPerRow;
	public int depthRangeStart = 0;
	public int depthRangeEnd = 999;
	public int guaranteeOneOnRow = -1;
	public int guaranteeColumn = -1;

	public enum TileType
	{
		EMPTY,
		DIRT,
		DIRT2,
		STONE,
		STONE2,
		DIAMOND,
		RESIDENCE,
		MILL,
		REFINERY,
		QUARRY,
		MINE,
		ENERGY_WELL,
		ENERGY_RELAY
	}

	public virtual void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		tileGrid = _tileGrid;
		myCoordinate = _coordinate;
		eventBroadcast = FindObjectOfType<EventBroadcast>();
		cameraControl = FindObjectOfType<CameraControl>();
		player = FindObjectOfType<Player>();
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
			if (neighborTile && neighborTile.GetComponent<TileEmpty>() != null)
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

	public virtual int GetMineralAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return mineralAdjustmentToBuild;
	}
	protected int GetMineralAdjustmentToBuild_stacked(TileGrid _tileGrid, Coordinate buildTarget, float multiplier)
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
		
		return (int)(mineralAdjustmentToBuild * Mathf.Pow(multiplier, numLikeTilesBelow));
	}

	public virtual int GetMineralAdjustmentToDestroy()
	{
		return mineralAdjustmentToDestroy;
	}


	public virtual bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
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
			case TileType.DIAMOND:
				return "Diamond";
			case TileType.RESIDENCE:
				return "Residence";
			case TileType.MILL:
				return "Mill";
			case TileType.REFINERY:
				return "Refinery";
			case TileType.QUARRY:
				return "Quarry";
			case TileType.MINE:
				return "Mine";
			case TileType.ENERGY_WELL:
				return "Energy Well";
			case TileType.ENERGY_RELAY:
				return "Energy Relay";
			default:
				return "ERROR OUT OF RANGE";
		}
	}
}
