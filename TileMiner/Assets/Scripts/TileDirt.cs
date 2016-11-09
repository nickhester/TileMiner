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

		Destroy(gameObject);
	}

	public override void Activate()
	{
		Destroy(gameObject);	// TEMP
	}
}
