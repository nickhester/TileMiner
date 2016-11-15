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
		MonoBehaviour.Destroy(tileToDestroy.gameObject);
		MonoBehaviour.FindObjectOfType<LevelGenerator>().CreateOneTile(tileToDestroy.GetCoordinate(), Tile.TileType.EMPTY);
	}

	public bool IsActionValid()
	{
		return true;
	}
}
