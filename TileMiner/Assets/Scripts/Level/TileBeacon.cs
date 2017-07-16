using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileBeacon : Tile
{

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		LevelManager levelManager = GameObject.FindObjectOfType<LevelManager>();
		levelManager.ReportLevelCompleted();
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
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection, Player player)
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
}
