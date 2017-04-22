﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMine : Tile
{
	public float radiusToSupport;

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
		// TODO: destroying mines will require checking that nothing in its range relies on its existence
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref string _failureReason)
	{
		bool isValid = true;
		
		if (PopulationAnalyzer.CanStructureBeAdded(this, _tileGrid))
		{
			//
		}
		else
		{
			_failureReason += "Population can't sustain this. ";
			isValid = false;
		}
		return isValid;
	}
}