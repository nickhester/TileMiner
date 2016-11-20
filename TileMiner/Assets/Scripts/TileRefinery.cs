using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileRefinery : Tile, IEventSubscriber
{
	[SerializeField] private int baseMineralEarnPerPlayerCollect = 1;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_MINERAL, this);
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
		if (_event == EventBroadcast.Event.PLAYER_COLLECTED_MINERAL)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(GetMineralAmountToAdd()));
			actionAdjustResources.Execute();
		}
	}

	int GetMineralAmountToAdd()
	{
		return baseMineralEarnPerPlayerCollect;
	}
}
