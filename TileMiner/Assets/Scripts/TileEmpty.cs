using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileEmpty : Tile
{
	protected override void PlayerClick()
	{
		var tileBelow = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate);

		// if above something that is not empty
		if (tileBelow
			&& tileBelow.GetType() != typeof(TileEmpty))
		{
			Activate();
		}
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.REFINERY));
		actions.Add(new ActionAdjustResources(new ResourceMineral(-8)));
		namedActionSet.Add(new NamedActionSet("Build Refinery", actions));
		
		ProposeActions(namedActionSet);
	}
}
