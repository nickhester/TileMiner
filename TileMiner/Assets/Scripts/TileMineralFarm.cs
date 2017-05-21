using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileMineralFarm : Tile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private float intervalToFarm = 20.0f;
	private float counterToFarm = 0.0f;

	private LevelGenerator levelGenerator;
	private LightManager lightManager;

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
				// this is the pattern that tile spaces will choose to fill
				int[] patternX = { 0, -1, 1, -1, 1 };
				int[] patternY = { -1, 0, 0, -1, -1 };

				for (int i = 0; i < patternX.Length; i++)
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
}
