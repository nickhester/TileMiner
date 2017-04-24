using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileQuarry : Tile, IEventSubscriber, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int baseMineralEarnPerPlayerAction = 1;
	[SerializeField] protected float stackMultiplierValue = 1.25f;
	private StackMultiplier stackMultiplierPrimary;
	private StackMultiplier stackMultiplierSecondary;
	[SerializeField] private int mineralEarnPerStoneCollection = 5;
	[SerializeField] protected float stackMultiplierCost = 2.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);
		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_STONE, this);

		stackMultiplierPrimary = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), baseMineralEarnPerPlayerAction, stackMultiplierValue);
		stackMultiplierSecondary = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), mineralEarnPerStoneCollection, stackMultiplierValue);
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
		if (_event == EventBroadcast.Event.PLAYER_COLLECTED_DIRT)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(stackMultiplierPrimary.GetMineralAmountToAdd()));
			actionAdjustResources.Execute();
		}
		else if (_event == EventBroadcast.Event.PLAYER_COLLECTED_STONE)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(stackMultiplierSecondary.GetMineralAmountToAdd()));
			actionAdjustResources.Execute();
		}
	}

	public float MultiplyStackValue(float f)
	{
		return stackMultiplierPrimary.MultiplyStackValue(f);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref string _failureReason)
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
			_failureReason += "Not on stone or other Quarry. ";
			isValid = false;
		}

		// check mine range
		bool passesMineProximityCheck = BuildRequirementsAnalyzer.IsWithinRangeOfMine(_myCoordinate, _tileGrid);

		if (!passesMineProximityCheck)
		{
			isValid = false;
			_failureReason += "Not close enough to mine. ";
		}

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

	public override int GetMineralAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetMineralAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}

