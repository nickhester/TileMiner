using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	private TileGrid tileGrid;

	public void Initialize(TileGrid _tileGrid)
	{
		tileGrid = _tileGrid;
	}
}
