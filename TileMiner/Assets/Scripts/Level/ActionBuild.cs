using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionBuild : IAction
{
	private Tile tileToReplace;
	Tile.TileType newType;
	bool ShouldValidateAction = true;

	public ActionBuild(Tile _tileToReplace, Tile.TileType _newType)
	{
		tileToReplace = _tileToReplace;
		newType = _newType;
	}

	public ActionBuild(Tile _tileToReplace, Tile.TileType _newType, bool _validateAction)
	{
		ShouldValidateAction = _validateAction;
		tileToReplace = _tileToReplace;
		newType = _newType;
	}

	public void Execute()
	{
		MonoBehaviour.FindObjectOfType<LevelGenerator>().ReplaceOneTile(tileToReplace.GetCoordinate(), newType);
	}

	public bool IsActionValid(ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool result = true;

		if (tileToReplace == null)
		{
			return false;
		}

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
		
		/*
		// check population
		if (PopulationAnalyzer.CanStructureBeAdded(tilePrefab, tileToReplace.GetTileGrid()))
		{
			//
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_ENOUGH_POPULATION));
			result = false;
		}
		*/

		bool passesClassValidation = true;
		if (ShouldValidateAction)
		{
			// check with tile for validation
			passesClassValidation = tilePrefab.CheckIfValidToBuild(tileToReplace.GetTileGrid(), tileToReplace.GetCoordinate(), ref _failureReason, ref isExcludedFromPlayerSelection);
		}

		if (!passesWeightCheck)
		{
			result = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.TILE_BELOW_MUST_SUPPORT_WEIGHT));
		}
		
		return (result && passesClassValidation);
	}
}
