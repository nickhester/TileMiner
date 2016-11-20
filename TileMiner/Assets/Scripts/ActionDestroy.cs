using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			GameObject.FindObjectOfType<EventBroadcast>().TriggerEvent(EventBroadcast.Event.PLAYER_COLLECTED_MINERAL);
		}

		MonoBehaviour.Destroy(tileToDestroy.gameObject);
		MonoBehaviour.FindObjectOfType<LevelGenerator>().CreateOneTile(tileToDestroy.GetCoordinate(), Tile.TileType.EMPTY);
	}

	public bool IsActionValid()
	{
		return WeightAnalyzer.CanTileBeRemoved(tileToDestroy);
	}
}
