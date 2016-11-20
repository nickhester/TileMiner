using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileRefinery : Tile, IEventSubscriber
{
	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		base.Initialize(_tileGrid, _coordinate);

		eventBroadcast.SubscribeToEvent(EventBroadcast.Event.PLAYER_COLLECTED_MINERAL, this);
	}

	public override void Activate()
	{
		//
	}

	protected override void PlayerClick()
	{
		//
	}

	public void InformOfEvent(EventBroadcast.Event _event)
	{
		if (_event == EventBroadcast.Event.PLAYER_COLLECTED_MINERAL)
		{
			ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new ResourceMineral(1));
			actionAdjustResources.Execute();
		}
	}
}
