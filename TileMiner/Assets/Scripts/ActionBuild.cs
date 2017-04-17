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
		MonoBehaviour.Destroy(tileToReplace.gameObject);
		MonoBehaviour.FindObjectOfType<LevelGenerator>().CreateOneTile(tileToReplace.GetCoordinate(), newType);
	}

	public bool IsActionValid(ref string _failureReason)
	{
		bool result = true;

		Tile tilePrefab = MonoBehaviour.FindObjectOfType<LevelGenerator>().GetTilePrefab(newType);

		// check weight
		int weightValue = tilePrefab.GetWeightSupportValue();
		bool passesWeightCheck = WeightAnalyzer.CanStructureBeAddedHere(tileToReplace, weightValue);

		// check with tile for validation
		bool passesClassValidation = tilePrefab.CheckIfValidToBuild(tileToReplace.GetTileGrid(), tileToReplace.GetCoordinate(), ref _failureReason);

		if (!passesWeightCheck)
		{
			result = false;
			_failureReason += "Can't support weight. ";
		}

		return (result && passesClassValidation);
	}
}
