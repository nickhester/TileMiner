using UnityEngine;
using System.Collections;
using System;

public class TileDirt : Tile
{
	protected override void PlayerClick()
	{
		if (GetIsExposed())
		{
			Activate();
		}
	}

	public override void Activate()
	{
		eventBroadcast.TriggerEvent(EventBroadcast.Event.TILE_COLLECTED_DIRT);
		RemoveSelf();
	}
}
