using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionDestroy(this));
		actions.Add(new ActionCollect(new ResourceDirt(), 1));
		namedActionSet.Add(new NamedActionSet("Collect Dirt", actions, true));

		// TODO: remove this, this is just a test
		actions = new List<IAction>();
		actions.Add(new ActionDestroy(this));
		actions.Add(new ActionDestroy(tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate)));
		actions.Add(new ActionCollect(new ResourceDirt(), 5));
		namedActionSet.Add(new NamedActionSet("Do Something Crazy", actions, true));

		// instantiate action option menu
		GameObject menu = Instantiate(FindObjectOfType<Player>().actionOptionMenuPrefab.gameObject) as GameObject;
		menu.GetComponent<ActionOptionMenu>().Initialize(namedActionSet);
	}
}
