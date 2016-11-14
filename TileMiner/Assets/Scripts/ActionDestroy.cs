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
		MonoBehaviour.FindObjectOfType<LevelGenerator>().CreateOneTile(tileToDestroy.GetCoordinate(), LevelGenerator.TileType.EMPTY);
		MonoBehaviour.Destroy(tileToDestroy.gameObject);
	}
}
