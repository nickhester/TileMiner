using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDiamond : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float distanceToEnergySource;
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

		// check if is near energy well, or another energy relay
		bool passesProximityCheck = (BuildRequirementsAnalyzer.IsWithinRangeOfTile(_myCoordinate, _tileGrid, typeof(TileEnergyWell), distanceToEnergySource)
									|| BuildRequirementsAnalyzer.IsWithinRangeOfTile(_myCoordinate, _tileGrid, typeof(TileEnergyRelay), distanceToEnergySource));

		if (!passesProximityCheck)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_NEARBY_TILE, (int)Tile.TileType.ENERGY_RELAY, "Not close enough to energy source."));
		}

		if (_tileGrid.GetDepth(_myCoordinate) > -heightRequiredToBuild)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_CERTAIN_HEIGHT, heightRequiredToBuild));
		}

		return isValid;
	}
}
