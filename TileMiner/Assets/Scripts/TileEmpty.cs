using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileEmpty : Tile
{
	private LevelGenerator levelGenerator;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		levelGenerator = FindObjectOfType<LevelGenerator>();
	}

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
		List<NamedActionSet> namedActionSets = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.RESIDENCE));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.RESIDENCE).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Residence", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.STATION));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.STATION).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Station", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.REFINERY));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.REFINERY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Refinery", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.MINE));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.MINE).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Mine", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.QUARRY));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.QUARRY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Quarry", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.BEACON));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.BEACON).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Beacon", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.DIRT));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.DIRT).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Fill With Dirt", actions));

		// specialized build types

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.DRILL_RIG));
		foreach (Resource res in levelGenerator.GetTilePrefab(TileType.DRILL_RIG).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
		{
			actions.Add(new ActionAdjustResources(res));
		}
		namedActionSets.Add(new NamedActionSet("Build Drill Rig", actions));

		ProposeActions(namedActionSets);
	}
}
