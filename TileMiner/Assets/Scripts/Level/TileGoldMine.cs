using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileGoldMine : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int goldEarnPerInterval = 1;
	[SerializeField] private float goldEarnBaseInterval = 1.0f;
	private float intervalCounter = 0.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);
	}

	private void Update()
	{
		if (isStructureActive)
		{
			intervalCounter += Time.deltaTime;
			if (intervalCounter > goldEarnBaseInterval)
			{
				ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new Resource(goldEarnPerInterval, Resource.ResourceType.GOLD));
				actionAdjustResources.Execute();
				intervalCounter = 0.0f;
			}
		}
	}

	protected override void PlayerClick()
	{
		Activate();
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();

		namedActionSet.Add(new NamedActionSet("Destroy", GetDestroyAction()));

		ProposeActions(namedActionSet);
	}
	
	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetTileType() == TileType.GOLD_VEIN))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on Gold Vein."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}
		return isValid;
	}

	public override void SetTechSettings(TechSettingsDefinition techSettingsDefinition)
	{
		foreach (var property in techSettingsDefinition.propertyLevels)
		{
			switch (property.Key)
			{
				case "goldEarnBaseInterval":
					{
						switch (property.Value)
						{
							case 0:
								goldEarnBaseInterval = 4; break;
							case 1:
								goldEarnBaseInterval = 3; break;
							case 2:
								goldEarnBaseInterval = 2; break;
							default:
								Debug.LogError("SetTechSetting Default Hit"); break;
						}
					}
					break;
				default:
					Debug.LogError("SetTechSetting Default Hit"); break;
			}
		}
	}
}
