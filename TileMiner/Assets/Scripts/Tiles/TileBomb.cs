using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileBomb : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float intervalToExplode = 20.0f;
	private float counterToExplode = 0.0f;
	[SerializeField] private int numTilesRadiusExplosion = 1;
	
	private void Update()
	{
		if (isStructureActive)
		{
			counterToExplode += Time.deltaTime;
			if (counterToExplode > intervalToExplode)
			{
				// explode - remove all tiles around self, except self
				for (int i = -numTilesRadiusExplosion; i <= numTilesRadiusExplosion; i++)
				{
					for (int j = -numTilesRadiusExplosion; j <= numTilesRadiusExplosion; j++)
					{
						if (!(i == 0 && j == 0))	// don't destroy self yet
						{
							Tile tileToDestroy = tileGrid.GetTileAt(GetCoordinate() + new Coordinate(i, j));
							if (tileToDestroy != null)
								tileToDestroy.DestroyImmediate(false);
						}
					}
				}
				// destroy self
				LevelGenerator.Instance.DestroyOneTile(GetCoordinate());
			}
		}
	}

	protected override void PlayerClick()
	{
		Activate();
	}
	
	public override void Activate()
	{
		// can't be destroyed
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

		bool isCityDevelopedForThis = City.Instance.IsCityBenefitAvailable(CityBenefits.Benefit.BOMB);
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
				case "numTilesRadiusExplosion":
					{
						switch (property.Value)
						{
							case 0:
								numTilesRadiusExplosion = 1; break;
							case 1:
								numTilesRadiusExplosion = 2; break;
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
