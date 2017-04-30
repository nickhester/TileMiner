using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileResidence : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] protected float stackMultiplierCost = 2.0f;

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
		namedActionSet.Add(new NamedActionSet("Destroy", actions));

		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
	{
		bool isValid = true;
		if (_tileGrid.GetDepth(_myCoordinate) < 0)
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, 1, "Not Above Ground."));
			isValid = false;
		}

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileResidence)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground or other Residence."));
			isValid = false;
		}

		return isValid;
	}

	public override Resource GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetMineralAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}
