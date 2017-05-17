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
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.RESIDENCE).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Residence", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.STATION));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.STATION).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Station", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.REFINERY));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.REFINERY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Refinery", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.MINE));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.MINE).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Mine", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.QUARRY));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.QUARRY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Quarry", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.BEACON));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.BEACON).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Build Beacon", actions));

		actions = new List<IAction>();
		actions.Add(new ActionBuild(this, Tile.TileType.DIRT));
		actions.Add(
			new ActionAdjustResources(
					levelGenerator.GetTilePrefab(TileType.DIRT).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate())));
		namedActionSets.Add(new NamedActionSet("Fill With Dirt", actions));

		ProposeActions(namedActionSets);
	}
}
