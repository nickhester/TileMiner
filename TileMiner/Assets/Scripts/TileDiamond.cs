using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDiamond : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int energyAdjustmentToBuild;
	[SerializeField] private int heightRequiredToBuild;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);
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
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
	{
		bool isValid = true;
		
		if (_tileGrid.GetDepth(_myCoordinate) > -heightRequiredToBuild)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, heightRequiredToBuild));
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
