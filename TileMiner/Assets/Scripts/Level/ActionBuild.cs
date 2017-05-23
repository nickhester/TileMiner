using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionBuild : IAction
{
	private Tile tileToReplace;
	Tile.TileType newType;

	public ActionBuild(Tile _rileToReplace, Tile.TileType _newType)
	{
		tileToReplace = _rileToReplace;
		newType = _newType;
	}

	public void Execute()
	{
		MonoBehaviour.FindObjectOfType<LevelGenerator>().ReplaceOneTile(tileToReplace.GetCoordinate(), newType);
	}

	public bool IsActionValid(ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool result = true;

		Tile tilePrefab = MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(newType);

		// check tile availability
		if (!tilePrefab.IsStructureAvailable)
		{
			isExcludedFromPlayerSelection = true;
			return false;
		}

		// check weight
		int weightValue = tilePrefab.GetWeightSupportValue();
		bool passesWeightCheck = WeightAnalyzer.CanStructureBeAddedHere(tileToReplace, weightValue);
		
		if (PopulationAnalyzer.CanStructureBeAdded(tilePrefab, tileToReplace.GetTileGrid()))
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_ENOUGH_POPULATION));
			result = false;
		}

		// check with tile for validation
		bool passesClassValidation = tilePrefab.CheckIfValidToBuild(tileToReplace.GetTileGrid(), tileToReplace.GetCoordinate(), ref _failureReason, ref isExcludedFromPlayerSelection);

		if (!passesWeightCheck)
		{
			result = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.TILE_BELOW_MUST_SUPPORT_WEIGHT));
		}
		
		return (result && passesClassValidation);
	}
}
