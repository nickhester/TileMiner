﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMine : Tile, IEventSubscriber, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int baseMineralEarnPerPlayerAction = 1;
	[SerializeField] protected float stackMultiplierValue = 1.25f;
	private StackMultiplier stackMultiplier;
	[SerializeField] protected float stackMultiplierCost = 1.25f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		EventBroadcast.Instance.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_DIRT, this);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), baseMineralEarnPerPlayerAction, stackMultiplierValue);
	}

	protected override void PlayerClick()
	{
		Activate();
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();

		namedActionSet.Add(new NamedActionSet("Destroy", GetDestroyAction()));

		ProposeActions(namedActionSet);
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.PLAYER_COLLECTED_DIRT)
		{
			if (isStructureActive)
			{
				ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new Resource(stackMultiplier.GetStackedAmount(), Resource.ResourceType.MINERAL));
				actionAdjustResources.Execute();
			}
		}
	}

	public float MultiplyStackValue(float f)
	{
		return stackMultiplier.MultiplyStackValue(f);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection, Player player)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileMine)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on dirt or other Mine."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		// check structure height
		bool passesHeightLimitCheck = BuildRequirementsAnalyzer.IsNotPastHeightLimit(_myCoordinate, _tileGrid, Tile.TileType.MINE, GetCurrentStackLimit(player.GetCity()));

		if (!passesHeightLimitCheck)
		{
			isExcludedFromPlayerSelection = true;
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_UNDER_STRUCTURE_HEIGHT_LIMIT, 3));
		}
		return isValid;
	}

	public override List<Resource> GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetResourceAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}

