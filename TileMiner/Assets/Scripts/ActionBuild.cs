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

	public bool IsActionValid()
	{
		return true;
	}
}
