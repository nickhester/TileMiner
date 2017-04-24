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
		actions.Add(new ActionBuild(this, Tile.TileType.RESIDENCE));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.RESIDENCE).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Residence", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.MINE));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.MINE).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Mine", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.REFINERY));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.REFINERY).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Refinery", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.MILL));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.MILL).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Mill", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.QUARRY));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.QUARRY).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Quarry", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.ENERGY_RELAY));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.ENERGY_RELAY).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Energy Relay", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.DIAMOND));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.DIAMOND).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Build Diamond Monument", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.DIRT));
		actions.Add(
			new ActionAdjustResources(
				new ResourceMineral(
					FindObjectOfType<LevelGenerator>().GetTilePrefab(TileType.DIRT).GetMineralAdjustmentToBuild(tileGrid, GetCoordinate()))));
		namedActionSet.Add(new NamedActionSet("Fill With Dirt", actions));

		ProposeActions(namedActionSet);
	}
}
