using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileEnergyRelay : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float distanceToEnergySource;

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
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref string _failureReason)
	{
		bool isValid = true;

		// check if is near energy well, or another energy relay
		bool passesMineProximityCheck = (BuildRequirementsAnalyzer.IsWithinRangeOfTile(_myCoordinate, _tileGrid, typeof(TileEnergyWell), distanceToEnergySource)
									|| BuildRequirementsAnalyzer.IsWithinRangeOfTile(_myCoordinate, _tileGrid, typeof(TileEnergyRelay), distanceToEnergySource));

		if (!passesMineProximityCheck)
		{
			isValid = false;
			_failureReason += "Not close enough to mine. ";
		}

		return isValid;
	}
}
