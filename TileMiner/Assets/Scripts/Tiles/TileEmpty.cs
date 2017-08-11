using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileEmpty : Tile
{
	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);
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

		if (LevelManager.Instance.CurrentLevelPhase == LevelManager.LevelPhase.BUILD_CITY)
		{
			// build city is a special snowflake
			actions = new List<IAction>();
			actions.Add(new ActionBuildCity(this));
			namedActionSets.Add(new NamedActionSet("Build City", actions));
		}
		else
		{
			// these all follow the same format
			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.STATION));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.STATION).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Station", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.REFINERY));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.REFINERY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Refinery", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.MINE));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.MINE).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Mine", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.QUARRY));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.QUARRY).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Quarry", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.BEACON));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.BEACON).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Beacon", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.DIRT));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.DIRT).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Fill With Dirt", actions));

			// specialized build types

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.BOMB));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.BOMB).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Place Bomb", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.DRILL_RIG));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.DRILL_RIG).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Drill Rig", actions));

			actions = new List<IAction>();
			actions.Add(new ActionBuild(this, Tile.TileType.MINERAL_FARM));
			foreach (Resource res in LevelGenerator.Instance.GetTilePrefab(TileType.MINERAL_FARM).GetResourceAdjustmentToBuild(tileGrid, GetCoordinate()))
			{
				actions.Add(new ActionAdjustResources(res));
			}
			namedActionSets.Add(new NamedActionSet("Build Mineral Farm", actions));
		}

		ProposeActions(namedActionSets);
	}
}
