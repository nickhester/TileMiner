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
			GameObject.FindObjectOfType<EventBroadcast>().TriggerEvent(EventBroadcast.Event.PLAYER_COLLECTED_DIRT);
		}
		else if (tileToDestroy.GetComponent<TileStone>() != null)
		{
			GameObject.FindObjectOfType<EventBroadcast>().TriggerEvent(EventBroadcast.Event.PLAYER_COLLECTED_STONE);
		}

		MonoBehaviour.Destroy(tileToDestroy.gameObject);
		MonoBehaviour.FindObjectOfType<LevelGenerator>().CreateOneTile(tileToDestroy.GetCoordinate(), Tile.TileType.EMPTY);
	}

	public bool IsActionValid(ref string _failureReason)
	{
		bool retVal = WeightAnalyzer.CanTileBeRemoved(tileToDestroy);

		if (!retVal)
		{
			_failureReason += "Structures Rely on this. ";
		}

		return retVal;
	}
}
