using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMineralFarm : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float intervalToFarm = 20.0f;
	private float counterToFarm = 0.0f;
	[SerializeField] private int numTileSpawnMax = 3;

	private LevelGenerator levelGenerator;
	private LightManager lightManager;

	// this is the pattern that tile spaces will choose to fill
	private int[] patternX = { 0, -1, 1, -1, 1 };
	private int[] patternY = { -1, 0, 0, -1, -1 };

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		levelGenerator = FindObjectOfType<LevelGenerator>();
		lightManager = FindObjectOfType<LightManager>();
	}

	private void Update()
	{
		if (isStructureActive)
		{
			counterToFarm += Time.deltaTime;
			if (counterToFarm > intervalToFarm)
			{
				for (int i = 0; (i < patternX.Length && i < numTileSpawnMax); i++)
				{
					Coordinate target = GetCoordinate() + new Coordinate(patternX[i], patternY[i]);
					Tile tileAtTarget = tileGrid.GetTileAt(target);
					if (tileAtTarget != null && tileAtTarget.GetTileType() == TileType.EMPTY)
					{
						// spawn dirt
						levelGenerator.ReplaceOneTile(target, TileType.DIRT);
						break;
					}
				}

				lightManager.RecalculateIllumination();
				counterToFarm = 0.0f;
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
			&& (tileBelow.GetType() == typeof(TileDirt)))
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
				case "intervalToFarm":
					{
						switch (property.Value)
						{
							case 0:
								intervalToFarm = 20; break;
							case 1:
								intervalToFarm = 17; break;
							case 2:
								intervalToFarm = 14; break;
							default:
								Debug.LogError("SetTechSetting Default Hit"); break;
						}
					} break;
				case "numTileSpawnMax":
					{
						switch (property.Value)
						{
							case 0:
								numTileSpawnMax = 3; break;
							case 1:
								numTileSpawnMax = 4; break;
							case 2:
								numTileSpawnMax = 5; break;
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
