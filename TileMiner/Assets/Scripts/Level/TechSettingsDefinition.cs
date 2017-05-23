using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSettingsDefinition
{
	public Tile.TileType tileType;
	public Dictionary<string, int> propertyLevels = new Dictionary<string, int>();

	public TechSettingsDefinition(Tile.TileType tileType)
	{
		this.tileType = tileType;
	}
}
