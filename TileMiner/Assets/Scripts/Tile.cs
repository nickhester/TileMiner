using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class Tile : MonoBehaviour
{
	protected TileGrid tileGrid;
	protected Coordinate myCoordinate;
	protected EventBroadcast eventBroadcast;

	[SerializeField] protected int weightSupportValue = 0;
	[SerializeField] protected int mineralAdjustmentToBuild = 0;
	public bool isStructure = false;

	[SerializeField] protected int populationAdjustment = 0;

	public enum TileType
	{
		EMPTY,
		DIRT,
		STONE,
		DIAMOND,
		RESIDENCE,
		MILL,
		REFINERY,
		QUARRY
	}

	public virtual void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		tileGrid = _tileGrid;
		myCoordinate = _coordinate;
		eventBroadcast = FindObjectOfType<EventBroadcast>();
	}
	
	void OnMouseDown()
	{
		if (FindObjectOfType<ActionOptionMenu>() == null)
		{
			PlayerClick();
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
		FindObjectOfType<Player>().ProposeActions(_actions);
	}

	public TileGrid GetTileGrid()
	{
		return tileGrid;
	}

	public int GetWeightSupportValue()
	{
		return weightSupportValue;
	}

	public int GetMineralAdjustmentToBuild()
	{
		return mineralAdjustmentToBuild;
	}

	public virtual bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref string _failureReason)
	{
		return true;
	}

	public int GetPopulationAdjustment()
	{
		return populationAdjustment;
	}
}
