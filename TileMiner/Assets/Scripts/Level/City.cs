using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
	public bool hasBeenBuilt = false;
	private List<CityBenefits> cityBenefits = new List<CityBenefits>();
	private TileGrid tileGrid;
	private LevelGenerator levelGenerator;
	private List<Tile> cityTiles;
	
	void Start()
	{
		levelGenerator = FindObjectOfType<LevelGenerator>();

		// set up all city benefits in order
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_2, 12, "Stack some bldgs 2 high for increased benefits"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.BOMB, 8, "Can build Bombs"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.DRILL_RIG, 16, "Can build Drill Rigs"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_3, 22, "Stack some bldgs 3 high for increased benefits"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.MINERAL_FARM, 26, "Can build Mineral Farms"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.REFINERY_DOUBLE, 32, "All Refineries produce 2x resources"));
	}

	public void Build(List<Tile> tilesReserved)
	{
		// this has to be first (to avoid infinite loop creating other city tiles below)
		hasBeenBuilt = true;

		// reserve all city tiles (3x3 grid up)
		cityTiles = tilesReserved;
		foreach (Tile tile in cityTiles)
		{
			levelGenerator.ReplaceOneTile(tile.GetCoordinate(), Tile.TileType.RESIDENCE);
		}
	}

	public List<CityBenefits> GetAvailableCityBenefits()
	{
		return cityBenefits.Where(b => IsCityBenefitAvailable(b.benefit)).ToList();
	}

	public bool IsCityBenefitAvailable(CityBenefits.Benefit benefit)
	{
		// first, check if specific tiles are even unlocked yet
		List<Tile.TileType> availableTiles = levelGenerator.GetAvailableTileTypes();
		if (benefit == CityBenefits.Benefit.BOMB)
		{
			if (!availableTiles.Contains(Tile.TileType.BOMB))
				return false;
		}
		else if (benefit == CityBenefits.Benefit.DRILL_RIG)
		{
			if (!availableTiles.Contains(Tile.TileType.DRILL_RIG))
				return false;
		}
		else if (benefit == CityBenefits.Benefit.MINERAL_FARM)
		{
			if (!availableTiles.Contains(Tile.TileType.MINERAL_FARM))
				return false;
		}

		// then, check if population requirement has been met
		int currentPopulation = Mathf.Abs(PopulationAnalyzer.GetCurrentPopulationAvailable(levelGenerator.GetTileGrid()));
		CityBenefits result = cityBenefits.Where(b => b.benefit == benefit).Single();
		return currentPopulation >= result.populationRequirement;
	}
}

public class CityBenefits
{
	public enum Benefit
	{
		STACK_2,
		STACK_3,
		BOMB,
		DRILL_RIG,
		MINERAL_FARM,
		REFINERY_DOUBLE
	}

	public Benefit benefit;
	public int populationRequirement;
	public string description;

	public CityBenefits(Benefit benefit, int populationRequirement, string description)
	{
		this.benefit = benefit;
		this.populationRequirement = populationRequirement;
		this.description = description;
	}
}