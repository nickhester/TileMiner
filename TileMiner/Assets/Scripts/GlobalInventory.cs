using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalInventory
{
	public int NumTechPieces = 0;

	public Dictionary<Tile.TileType, int> TechStatus = new Dictionary<Tile.TileType, int>();
	private Dictionary<Tile.TileType, int> techRequirements = new Dictionary<Tile.TileType, int>();
	private List<Tile.TileType> unlockableTech = new List<Tile.TileType>();

	public GlobalInventory()
	{
		unlockableTech.Add(Tile.TileType.BOMB);
		unlockableTech.Add(Tile.TileType.DRILL_RIG);
		unlockableTech.Add(Tile.TileType.MINERAL_FARM);

		// initialize all tech
		TechStatus.Add(Tile.TileType.BOMB, 0);
		TechStatus.Add(Tile.TileType.DRILL_RIG, 0);
		TechStatus.Add(Tile.TileType.MINERAL_FARM, 0);
		
		techRequirements.Add(Tile.TileType.BOMB, 5);
		techRequirements.Add(Tile.TileType.DRILL_RIG, 10);
		techRequirements.Add(Tile.TileType.MINERAL_FARM, 15);
	}

	public KeyValuePair<Tile.TileType, int> NumTechRequiredToUnlockNextTech()
	{
		int numTechs = unlockableTech.Count;
		for (int i = 0; i < numTechs; i++)
		{
			if (NumTechPieces < techRequirements[unlockableTech[i]])
			{
				if (i < numTechs)
					return new KeyValuePair<Tile.TileType, int>((unlockableTech[i]), (techRequirements[unlockableTech[i]] - NumTechPieces));
			}
		}
		return new KeyValuePair<Tile.TileType, int>(0, -1);
	}

	public void AddTechPieces(int n)
	{
		NumTechPieces += n;

		if (NumTechPieces >= techRequirements[Tile.TileType.BOMB])
		{
			TechStatus[Tile.TileType.BOMB] = 1;
		}

		if (NumTechPieces >= techRequirements[Tile.TileType.DRILL_RIG])
		{
			TechStatus[Tile.TileType.DRILL_RIG] = 1;
		}

		if (NumTechPieces >= techRequirements[Tile.TileType.MINERAL_FARM])
		{
			TechStatus[Tile.TileType.MINERAL_FARM] = 1;
		}
	}
}