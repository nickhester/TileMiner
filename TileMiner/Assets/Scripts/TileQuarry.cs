using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileQuarry : Tile, IEventSubscriber, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] protected float stackMultiplierValue = 1.25f;
	private StackMultiplier stackMultiplier;
	[SerializeField] private int mineralEarnPerStoneCollection = 5;
	[SerializeField] protected float stackMultiplierCost = 2.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);
		
		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_STONE, this);
		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_SELECTED_STONE, this);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), mineralEarnPerStoneCollection, stackMultiplierValue);
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

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.PLAYER_SELECTED_STONE)
		{
			ResourceMineral resourceMineral = new ResourceMineral(stackMultiplier.GetStackedAmount());
			tileGrid.ReportStoneCollectAdjustmentValue(resourceMineral.GetAmount());
		}
		else if (_event == EventBroadcast.Event.PLAYER_COLLECTED_STONE)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(stackMultiplier.GetStackedAmount()));
			actionAdjustResources.Execute();
		}
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
			&& (tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileQuarry)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.STONE, "Not on stone or other Quarry."));
			isValid = false;
		}

		// check mine range
		bool passesMineProximityCheck = BuildRequirementsAnalyzer.IsWithinRangeOfMine(_myCoordinate, _tileGrid);

		if (!passesMineProximityCheck)
		{
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_NEARBY_TILE, (int)Tile.TileType.MINE));
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

