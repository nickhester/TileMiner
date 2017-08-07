using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PopulationAnalyzer
{
	public static bool CanStructureBeRemoved(Tile t, TileGrid tileGrid)
	{
		if ((GetCurrentPopulationAvailable(tileGrid) - t.GetPopulationAdjustment()) >= 0)
		{
			return true;
		}
		return false;
	}

	public static bool CanStructureBeAdded(Tile t, TileGrid tileGrid)
	{
		if ((GetCurrentPopulationAvailable(tileGrid) + t.GetPopulationAdjustment()) >= 0)
		{
			return true;
		}
		return false;
	}

	public static int GetCurrentPopulationAvailable(TileGrid tileGrid)
	{
		if (tileGrid == null)
			return 0;		// HACK: assume tileGrid hasn't been initialized yet, so there must be no population
		
		List<Tile> structures = TileGridFilter.GetAllStructureTiles(tileGrid);
		int populationAvailable = 0;
		for (int i = 0; i < structures.Count; i++)
		{
			populationAvailable += structures[i].GetPopulationAdjustment();
		}
		return populationAvailable;
	}
}
