using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
	public bool hasBeenBuilt = false;
	private List<CityBenefits> cityBenefits = new List<CityBenefits>();
	private TileGrid tileGrid;
	
	void Start()
	{
		tileGrid = FindObjectOfType<LevelGenerator>().GetTileGrid();

		// set up all city benefits in order
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_2, 6));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.BOMB, 10));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.DRILL_RIG, 16));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_3, 20));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.MINERAL_FARM, 26));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.REFINERY_DOUBLE, 30));
	}

	public void Build()
	{
		hasBeenBuilt = true;
	}

	public bool IsCityBenefitAvailable(CityBenefits.Benefit benefit)
	{
		int currentPopulation = Mathf.Abs(PopulationAnalyzer.GetCurrentPopulationAvailable(tileGrid));
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

	public CityBenefits(Benefit benefit, int populationRequirement)
	{
		this.benefit = benefit;
		this.populationRequirement = populationRequirement;
	}
}