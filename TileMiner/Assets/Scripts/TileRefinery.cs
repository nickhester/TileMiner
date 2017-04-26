using UnityEngine;
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

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), 1.0f/mineralEarnBaseInterval, stackMultiplierInterval);
	}

	private void Update()
	{
		intervalCounter += Time.deltaTime;
		float stackedAmount = stackMultiplier.GetStackedAmount_float();
		if (stackedAmount != 0.0f && intervalCounter > (1.0f/stackedAmount))
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(mineralEarnPerInterval));
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

	public override int GetMineralAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetMineralAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}
