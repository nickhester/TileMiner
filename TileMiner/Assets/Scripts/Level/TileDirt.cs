using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDirt : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float chanceToDropGold = 0.2f;

	protected override void PlayerClick()
	{
		if (GetIsExposed() && IsIlluminated())
		{
			Activate();
		}
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();

		namedActionSet.Add(new NamedActionSet("Collect Mineral", GetDestroyAction(), true));

		ProposeActions(namedActionSet);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection, Player player)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}
		
		return isValid;
	}
	
	public override List<Resource> GetResourceAdjustmentToDestroy()
	{
		List<Resource> resources = new List<Resource>();
		resources.Add(new Resource(mineralAdjustmentToDestroy, Resource.ResourceType.MINERAL));

		if (UnityEngine.Random.Range(0.0f, 1.0f) < chanceToDropGold)
			resources.Add(new Resource(goldAdjustmentToDestroy, Resource.ResourceType.GOLD));

		resources.Add(new Resource(energyAdjustmentToDestroy, Resource.ResourceType.ENERGY));
		resources.Add(new Resource(alienTechAdjustmentToDestroy, Resource.ResourceType.ALIEN_TECH));
		return resources;
	}
}
