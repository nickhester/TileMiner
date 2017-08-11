using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionDestroy : IAction
{
	private Tile tileToDestroy;

	public ActionDestroy(Tile t)
	{
		tileToDestroy = t;
	}

	public void Execute()
	{
		if (tileToDestroy.GetComponent<TileDirt>() != null)
		{
			EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.PLAYER_COLLECTED_DIRT);
		}
		else if (tileToDestroy.GetComponent<TileStone>() != null)
		{
			EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.PLAYER_COLLECTED_STONE);
		}

		LevelGenerator.Instance.DestroyOneTile(tileToDestroy.GetCoordinate());
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

		retVal = WeightAnalyzer.CanTileBeRemoved(tileToDestroy);
		if (!retVal)
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.STRUCTURE_REQUIRED_FOR_WEIGHT));
			return false;
		}

		return retVal;
	}
}
