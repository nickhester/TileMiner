﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileDrillRig : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float intervalToDrillTile = 20.0f;
	private float counterToDrillTile = 0.0f;
	[SerializeField] private int numTilesLifetime = 5;
	
	private void Update()
	{
		if (isStructureActive)
		{
			counterToDrillTile += Time.deltaTime;
			if (counterToDrillTile > intervalToDrillTile)
			{
				// destroy tile below
				Tile tileBelow = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, GetCoordinate());
				if (tileBelow != null)
					tileBelow.DestroyImmediate(false);

				// move drill down one
				LevelGenerator.Instance.MoveTile(GetCoordinate(), tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, GetCoordinate()).GetCoordinate(), TileType.EMPTY);

				// check tile below that
				Tile nextTileDown = tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, GetCoordinate());
				// remove self if no ground, or if lifetime is up
				numTilesLifetime--;
				if (numTilesLifetime <= 0
					|| nextTileDown.GetTileType() == TileType.EMPTY
					|| nextTileDown.GetTileType() == TileType.ENERGY_WELL)
				{
					LevelGenerator.Instance.DestroyOneTile(GetCoordinate());
				}

				counterToDrillTile = 0.0f;
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

		bool isCityDevelopedForThis = City.Instance.IsCityBenefitAvailable(CityBenefits.Benefit.DRILL_RIG);
		if (isCityDevelopedForThis)
		{
			// valid
		}
		else
		{
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
				case "isAvailable":
					{
						IsStructureAvailable = (property.Value == 1 ? true : false);
					} break;
				case "numTilesLifetime":
					{
						switch (property.Value)
						{
							case 0:
								numTilesLifetime = 3; break;
							case 1:
								numTilesLifetime = 4; break;
							case 2:
								numTilesLifetime = 5; break;
							default:
								Debug.LogError("SetTechSetting Default Hit"); break;
						}
					} break;
				default:
					Debug.LogError("SetTechSetting Default Hit"); break;
			}
		}
	}
}
