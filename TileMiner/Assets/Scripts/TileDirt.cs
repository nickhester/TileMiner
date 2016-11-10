using UnityEngine;
using System.Collections;
using System;

public class TileDirt : Tile
{
	protected override void PlayerClick()
	{
		Tile neighbor = tileGrid.GetTileNeighbor(TileGrid.Direction.LEFT, myCoordinate);
		if (neighbor)
		{
			neighbor.Activate();
		}

		Activate();
	}

	public override void Activate()
	{
		eventBroadcast.TriggerEvent(EventBroadcast.Event.TILE_COLLECTED_DIRT);
		Destroy(gameObject);	// TEMP
	}
}
