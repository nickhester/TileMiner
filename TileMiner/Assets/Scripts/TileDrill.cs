using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDrill : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float intervalToDrillTile = 20.0f;
	private float counterToDrillDrill = 0.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);
	}

	private void Update()
	{
		if (isStructureActive)
		{
			counterToDrillDrill += Time.deltaTime;
			if (counterToDrillDrill > intervalToDrillTile)
			{
				// drill
				// destroy tile below
				// move drill down one
				// check tile below that
				// remove self if no ground

				counterToDrillDrill = 0.0f;
			}
		}
	}

	protected override void PlayerClick()
	{
		Activate();
	}
	
	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();
		
		actions = new List<IAction>();
		actions.Add(new ActionDestroy(this));
		actions.Add(
			new ActionAdjustResources(
				new Resource(GetMineralAdjustmentToDestroy(), Resource.ResourceType.MINERAL)));
		namedActionSet.Add(new NamedActionSet("Destroy", actions));

		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;
		
		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}
		return isValid;
	}
}
