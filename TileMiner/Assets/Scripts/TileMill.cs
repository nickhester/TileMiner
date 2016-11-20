using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMill : Tile, IEventSubscriber
{
	[SerializeField] private int baseMineralEarnPerPlayerAction = 1;
	[SerializeField] private float stackMultiplier = 1.25f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_ACTION, this);
	}

	protected override void PlayerClick()
	{
		var tileBelow = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate);

		// if above dirt or another mill
		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileMill)))
		{
			Activate();
		}
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
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(GetMineralAmountToAdd()));
			actionAdjustResources.Execute();
		}
	}

	int GetMineralAmountToAdd()
	{
		int returnValue = 0;

		if (tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate).GetType() == this.GetType())
		{
			// I'm not the base, so don't add anything
			returnValue = 0;
		}
		else
		{
			// I'm the base Mill, so call recursively upward
			float untruncatedMultipliedValue = MultiplyValue((float)baseMineralEarnPerPlayerAction);
			returnValue = (int)untruncatedMultipliedValue;
		}
		return returnValue;
	}

	public float MultiplyValue(float f)
	{
		float retVal = f;
		var tileAbove = tileGrid.GetTileNeighbor(TileGrid.Direction.UP, myCoordinate);
		if (tileAbove.GetType() == this.GetType())
		{
			retVal *= stackMultiplier * ((TileMill)tileAbove).MultiplyValue(retVal);
		}

		return retVal;
	}
}

