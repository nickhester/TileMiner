using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMill : Tile, IEventSubscriber, IStackableTile
{
	[SerializeField] private int baseMineralEarnPerPlayerAction = 1;
	[SerializeField] protected float stackMultiplierValue = 1.25f;
	private StackMultiplier stackMultiplier;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), baseMineralEarnPerPlayerAction, stackMultiplierValue);
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
		if (_event == EventBroadcast.Event.PLAYER_ACTION)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(stackMultiplier.GetMineralAmountToAdd()));
			actionAdjustResources.Execute();
		}
	}

	public float MultiplyStackValue(float f)
	{
		return stackMultiplier.MultiplyStackValue(f);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref string _failureReason)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& tileBelow.GetType() == typeof(TileDirt))
		{
			// valid
		}
		else
		{
			_failureReason += "Not on dirt. ";
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
}

