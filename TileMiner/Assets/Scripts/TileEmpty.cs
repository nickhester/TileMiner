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
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.REFINERY).GetMineralAdjustmentToBuild())));
		namedActionSet.Add(new NamedActionSet("Build Refinery", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.MILL));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.MILL).GetMineralAdjustmentToBuild())));
		namedActionSet.Add(new NamedActionSet("Build Mill", actions));

		ProposeActions(namedActionSet);
	}
}
