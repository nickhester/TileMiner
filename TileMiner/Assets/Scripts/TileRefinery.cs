﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileRefinery : Tile, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int mineralEarnPerInterval = 1;
	[SerializeField] private int mineralEarnBaseInterval = 1;
	private float intervalCounter = 0.0f;
	[SerializeField] protected float stackMultiplierInterval;
	private StackMultiplier stackMultiplier;
	[SerializeField] protected float stackMultiplierCost = 2.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), 1.0f/mineralEarnBaseInterval, stackMultiplierInterval);
	}

	private void Update()
	{
		intervalCounter += Time.deltaTime;
		float stackedAmount = stackMultiplier.GetStackedAmount_float();
		if (stackedAmount != 0.0f && intervalCounter > (1.0f/stackedAmount))
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new Resource(mineralEarnPerInterval, Resource.ResourceType.MINERAL));
			actionAdjustResources.Execute();
			intervalCounter = 0.0f;
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
		namedActionSet.Add(new NamedActionSet("Destroy", actions));

		ProposeActions(namedActionSet);
	}

	public float MultiplyStackValue(float f)
	{
		return stackMultiplier.MultiplyStackValue(f);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileRefinery)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground or other Refinery."));
			isValid = false;
		}

		// check mine range
		bool passesMineProximityCheck = BuildRequirementsAnalyzer.IsWithinRangeOfMine(_myCoordinate, _tileGrid);

		if (!passesMineProximityCheck)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_NEARBY_TILE, (int)Tile.TileType.MINE));
		}

		// check structure height
		bool passesHeightLimitCheck = BuildRequirementsAnalyzer.IsNotPastHeightLimit(_myCoordinate, _tileGrid, Tile.TileType.REFINERY, 3);

		if (!passesHeightLimitCheck)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_UNDER_STRUCTURE_HEIGHT_LIMIT, 3));
		}


		if (PopulationAnalyzer.CanStructureBeAdded(this, _tileGrid))
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_ENOUGH_POPULATION));
			isValid = false;
		}
		return isValid;
	}

	public override Resource GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetMineralAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}
