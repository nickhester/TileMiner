using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionTriggerRift : IAction
{
	private Tile tileToReturnTo;

	public ActionTriggerRift(Tile t)
	{
		tileToReturnTo = t;
	}

	public void Execute()
	{
		tileToReturnTo.ReturnResult(true);
	}

	public bool IsActionValid(ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool retVal = true;

		retVal = LevelManager.Instance.CurrentLevelPhase == LevelManager.LevelPhase.MAIN;
		if (!retVal)
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.INCOMPATIBLE_WITH_LEVEL_PHASE));
			return false;
		}

		retVal = WeightAnalyzer.CanTileBeRemoved(tileToReturnTo.GetCoordinate());
		if (!retVal)
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.STRUCTURE_REQUIRED_FOR_WEIGHT));
			return false;
		}

		return retVal;
	}
}
