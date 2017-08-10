using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class City : MonoBehaviour
{
	[HideInInspector] public bool hasBeenBuilt = false;
	private List<CityBenefits> cityBenefits = new List<CityBenefits>();
	private TileGrid tileGrid;
	private List<Tile> cityTiles;

	private int _currentHealth = 100;
	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}
		private set
		{
			_currentHealth = value;
		}
	}

	private Coordinate baseCoordinate;
	[SerializeField] private GameObject building;
	private List<GameObject> myBuildings = new List<GameObject>();

	// static ref to singleton
	private static City instance;
	public static City Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<City>();
			}
			return instance;
		}
	}

	public void HitCity(int damage)
	{
		if (Instance != null)
			Instance.GetHit(damage);
	}

	void Start()
	{
		// set up all city benefits in order
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_2, 12, "Stack some bldgs 2 high for increased benefits"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.BOMB, 8, "Can build Bombs"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.DRILL_RIG, 16, "Can build Drill Rigs"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.STACK_3, 22, "Stack some bldgs 3 high for increased benefits"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.MINERAL_FARM, 26, "Can build Mineral Farms"));
		cityBenefits.Add(new CityBenefits(CityBenefits.Benefit.REFINERY_DOUBLE, 32, "All Refineries produce 2x resources"));
	}

	void OnDestroy()
	{
		instance = null;
	}

	public void Build(Coordinate buildingBaseCoordinate, List<Tile> tilesReserved)
	{
		hasBeenBuilt = true;

		// reserve all city tiles (3x3 grid up)
		cityTiles = tilesReserved;
		foreach (Tile tile in cityTiles)
		{
			LevelGenerator.Instance.ReplaceOneTile(tile.GetCoordinate(), Tile.TileType.RESIDENCE);
		}

		baseCoordinate = buildingBaseCoordinate;
		BuildBuilding();

		LevelGenerator.Instance.GetComponent<PathManager>().UpdateTilePathSteps();
	}

	void BuildBuilding()
	{
		Vector3 pos = LevelGenerator.Instance.GetWorldSpacePositionFromCoordinate(baseCoordinate) + new Vector2(0.0f, 0.75f);
		GameObject go = Instantiate(building, pos, Quaternion.identity, this.transform) as GameObject;
		go.transform.localScale = new Vector2(5.0f, 20.0f);
		myBuildings.Add(go);
	}

	public Tile GetCityTile()
	{
		if (cityTiles != null && cityTiles.Count > 0)
			return cityTiles[0];
		else
			return null;
	}

	public List<CityBenefits> GetAvailableCityBenefits()
	{
		return cityBenefits.Where(b => IsCityBenefitAvailable(b.benefit)).ToList();
	}

	public bool IsCityBenefitAvailable(CityBenefits.Benefit benefit)
	{
		// first, check if specific tiles are even unlocked yet
		List<Tile.TileType> availableTiles = LevelGenerator.Instance.GetAvailableTileTypes();
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
		int currentPopulation = Mathf.Abs(PopulationAnalyzer.GetCurrentPopulationAvailable(LevelGenerator.Instance.GetTileGrid()));
		CityBenefits result = cityBenefits.Where(b => b.benefit == benefit).Single();
		return currentPopulation >= result.populationRequirement;
	}

	public void GetHit(int damage)
	{
		CurrentHealth--;

		EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.CITY_HIT);

		if (CurrentHealth <= 0)
		{
			LevelManager.Instance.ReportLevelCompleted(false);
		}
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