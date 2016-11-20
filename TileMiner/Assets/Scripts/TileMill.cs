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
		//var tileBelow = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate);
		//
		//// if above dirt or another mill
		//if (tileBelow
		//	&& (tileBelow.GetType() == typeof(TileDirt)
		//		|| tileBelow.GetType() == typeof(TileMill)))
		//{
		//	Activate();
		//}

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
}

