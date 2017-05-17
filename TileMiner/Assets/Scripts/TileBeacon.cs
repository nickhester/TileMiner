using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileBeacon : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int energyAdjustmentToBuild;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);
	}

	protected override void PlayerClick()
	{
		Activate();
	}

	public override void Activate()
	{
		// no actions - building completes level
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;
		
		if (_tileGrid.GetDepth(_myCoordinate) < 0)
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, 1, "Not Above Ground."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		return isValid;
	}

	public int GetEnergyAdjustmentToBuild()
	{
		return energyAdjustmentToBuild;
	}

	public override Resource GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return new Resource(energyAdjustmentToBuild, Resource.ResourceType.ENERGY);
	}
}
