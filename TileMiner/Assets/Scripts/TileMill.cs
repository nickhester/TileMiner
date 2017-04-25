﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMill : Tile, IEventSubscriber, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int baseMineralEarnPerPlayerAction = 1;
	[SerializeField] protected float stackMultiplierValue = 1.25f;
	private StackMultiplier stackMultiplier;
	[SerializeField] protected float stackMultiplierCost = 1.25f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_DIRT, this);
		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_STONE, this);

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
		if (_event == EventBroadcast.Event.PLAYER_COLLECTED_DIRT || _event == EventBroadcast.Event.PLAYER_COLLECTED_STONE)
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
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileMill)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on dirt or other Mill."));
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

