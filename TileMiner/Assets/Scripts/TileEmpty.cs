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
		actions.Add(new ActionBuild(this, Tile.TileType.MILL));
		actions.Add(new ActionAdjustResources(new ResourceDirt(-2)));
		namedActionSet.Add(new NamedActionSet("Build Mill", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.RESIDENCE));
		actions.Add(new ActionAdjustResources(new ResourceDirt(-4)));
		namedActionSet.Add(new NamedActionSet("Build Residence", actions));

		// instantiate action option menu
		CreateActionOptionMenu(namedActionSet);
	}
}
