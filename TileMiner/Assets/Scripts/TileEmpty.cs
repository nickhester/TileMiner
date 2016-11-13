using UnityEngine;
using System.Collections;
using System;

public class TileEmpty : Tile
{
	public override void Activate()
	{
		// do nothing
	}

	protected override void PlayerClick()
	{
		Tile tileBelow = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate);
		TileDirt tileBelow_dirt = tileBelow.GetComponent<TileDirt>();
		if (tileBelow && tileBelow_dirt != null)
		{
			print("ready to build");

		}
		
	}
}
