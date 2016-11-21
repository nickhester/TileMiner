using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileResidence : Tile
{
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
		if (_myCoordinate.y < _tileGrid.GetNumRowsSky())
		{
			return true;
		}
		_failureReason += "Not Above Ground. ";
		return false;
	}
}
