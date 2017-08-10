using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public abstract class Tile : MonoBehaviour
{
	protected TileGrid tileGrid;
	protected Coordinate myCoordinate;
	private CameraControl cameraControl;
	protected bool hasBeenInitialized = false;

	[SerializeField] protected int weightSupportValue = 0;

	[SerializeField] protected int mineralAdjustmentToBuild = 0;
	[SerializeField] protected int mineralAdjustmentToDestroy = 0;
	[SerializeField] protected int goldAdjustmentToBuild = 0;
	[SerializeField] protected int goldAdjustmentToDestroy = 0;
	[SerializeField] protected int energyAdjustmentToBuild = 0;
	[SerializeField] protected int energyAdjustmentToDestroy = 0;
	[SerializeField] protected int alienTechAdjustmentToDestroy = 0;

	public bool isStructure = false;
	protected bool isStructureActive = true;
	public bool IsStructureAvailable { get; set; }

	[SerializeField] protected int populationAdjustment = 0;
	[SerializeField] protected GameObject entityToSpawn;
	
	public enum TileType
	{
		EMPTY,
		DIRT,
		DIRT2,
		STONE,
		STONE2,
		BEACON,
		RESIDENCE,
		MINE,
		REFINERY,
		QUARRY,
		STATION,
		ENERGY_WELL,
		DRILL_RIG,
		BOMB,
		MINERAL_FARM,
		GOLD_VEIN,
		ALIEN_TECH,
		RIFT
	}
	[SerializeField] private TileType myTileType;

	private int brightnessLevel = 0;
	public int maxBrightness = 5;
	private Color originalColor;
	private SpriteRenderer spriteRenderer;

	private int _numStepsFromCity = -1;
	public int numStepsFromCity
	{
		get
		{
			return _numStepsFromCity;
		}
		set
		{
			_numStepsFromCity = value;
		}
	}

	public virtual void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _tileType)
	{
		tileGrid = _tileGrid;
		myCoordinate = _coordinate;
		cameraControl = FindObjectOfType<CameraControl>();
		myTileType = _tileType;

		spriteRenderer = GetComponent<SpriteRenderer>();
		originalColor = spriteRenderer.color;
		UpdateBrightness();

		hasBeenInitialized = true;
	}
	
	void OnMouseUpAsButton()
	{
		if (FindObjectOfType<ActionOptionMenu>() == null)
		{
			bool isCameraDragging = false;
			float timeSinceCameraDragEnded = -1.0f;
			cameraControl.GetDraggingStatus(ref isCameraDragging, ref timeSinceCameraDragEnded);
			if (!isCameraDragging || timeSinceCameraDragEnded > 0.001f)
			{
				PlayerClick();
			}
		}
	}

	protected abstract void PlayerClick();

	public abstract void Activate();
	
	protected virtual List<IAction> GetDestroyAction()
	{
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		actions.Add(new ActionDestroy(this));
		foreach (Resource res in GetResourceAdjustmentToDestroy())
		{
			actions.Add(new ActionAdjustResources(res));
		}
		return actions;
	}

	virtual public void OnDestroyTile() { }

	public bool DestroyImmediate(bool isCollectingResources)
	{
		List<IAction> actions = GetDestroyAction();
		foreach (var action in actions)
		{
			// if "isCollectingResources" is false, then only execute the destroy action
			if (isCollectingResources || (action as ActionDestroy) != null)
			{
				action.Execute();
			}
		}
		return true;
	}
	
	public Coordinate GetCoordinate()
	{
		return myCoordinate;
	}

	public void UpdateCoordinates(Coordinate c)
	{
		myCoordinate = c;
	}

	public bool GetIsExposed()
	{
		int numDirections = Enum.GetNames(typeof(TileGrid.Direction)).Length;
		for (int i = 0; i < numDirections; i++)
		{
			Tile neighborTile = tileGrid.GetTileNeighbor((TileGrid.Direction)i, GetCoordinate());       // TODO: make "null" neighbors not count toward being exposed
			if (neighborTile && 
				(neighborTile.GetComponent<TileEmpty>() != null
					|| neighborTile.isStructure))
			{
				return true;
			}
		}
		return false;
	}

	protected void ProposeActions(List<NamedActionSet> _actions)
	{
		// only allow residence build action if player can't destroy tiles yet
		if (!City.Instance.hasBeenBuilt)
		{
			var buildResidenceAction = from action in _actions
									   where action.name == "Build City"
									   select action;
			List<NamedActionSet> buildResidenceActionFinal = buildResidenceAction.ToList();
			if (buildResidenceActionFinal != null)
				_actions = buildResidenceActionFinal;
			else
				return;
		}


		Player.Instance.ProposeActions(_actions);
	}

	public TileGrid GetTileGrid()
	{
		return tileGrid;
	}

	public int GetWeightSupportValue()
	{
		return weightSupportValue;
	}
	
	public virtual List<Resource> GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		List<Resource> resources = new List<Resource>();

		if (mineralAdjustmentToBuild != 0)
			resources.Add(new Resource(mineralAdjustmentToBuild, Resource.ResourceType.MINERAL));
		if (goldAdjustmentToBuild != 0)
			resources.Add(new Resource(goldAdjustmentToBuild, Resource.ResourceType.GOLD));
		if (energyAdjustmentToBuild != 0)
			resources.Add(new Resource(energyAdjustmentToBuild, Resource.ResourceType.ENERGY));

		return resources;
	}

	// stack cost increases only apply to mineral adjustments right now
	protected List<Resource> GetResourceAdjustmentToBuild_stacked(TileGrid _tileGrid, Coordinate buildTarget, float multiplier)
	{
		List<Resource> resources = new List<Resource>();

		if (goldAdjustmentToBuild != 0)
			resources.Add(new Resource(goldAdjustmentToBuild, Resource.ResourceType.GOLD));
		if (energyAdjustmentToBuild != 0)
			resources.Add(new Resource(energyAdjustmentToBuild, Resource.ResourceType.ENERGY));
		
		Tile targetTile = _tileGrid.GetTileAt(buildTarget);
		Tile currentTileDown = targetTile;
		int numLikeTilesBelow = 0;
		while (true)
		{
			Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, currentTileDown.GetCoordinate());
			if (tileBelow.GetType() == this.GetType())
			{
				currentTileDown = tileBelow;
				numLikeTilesBelow++;
			}
			else
			{
				break;
			}
		}

		resources.Add(new Resource((int)(mineralAdjustmentToBuild * Mathf.Pow(multiplier, numLikeTilesBelow)), Resource.ResourceType.MINERAL));

		return resources;
	}

	public virtual List<Resource> GetResourceAdjustmentToDestroy()
	{
		List<Resource> resources = new List<Resource>();
		resources.Add(new Resource(mineralAdjustmentToDestroy, Resource.ResourceType.MINERAL));
		resources.Add(new Resource(goldAdjustmentToDestroy, Resource.ResourceType.GOLD));
		resources.Add(new Resource(energyAdjustmentToDestroy, Resource.ResourceType.ENERGY));
		resources.Add(new Resource(alienTechAdjustmentToDestroy, Resource.ResourceType.ALIEN_TECH));
		return resources;
	}
	
	public virtual bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		return true;
	}

	public int GetPopulationAdjustment()
	{
		return populationAdjustment;
	}

	public static string GetTileNameByEnum(TileType type)
	{
		switch (type)
		{
			case TileType.EMPTY:
				return "Empty";
			case TileType.DIRT:
				return "Dirt";
			case TileType.DIRT2:
				return "Rich Dirt";
			case TileType.STONE:
				return "Stone";
			case TileType.STONE2:
				return "Dense Stone";
			case TileType.BEACON:
				return "Beacon";
			case TileType.RESIDENCE:
				return "Residence";
			case TileType.MINE:
				return "Mine";
			case TileType.REFINERY:
				return "Refinery";
			case TileType.QUARRY:
				return "Quarry";
			case TileType.STATION:
				return "Mine";
			case TileType.ENERGY_WELL:
				return "Energy Well";
			case TileType.DRILL_RIG:
				return "Drill Rig";
			case TileType.BOMB:
				return "Bomb";
			case TileType.MINERAL_FARM:
				return "Mineral Farm";
			case TileType.GOLD_VEIN:
				return "Gold Vein";
			case TileType.ALIEN_TECH:
				return "Alien Tech";
			default:
				return "ERROR: TILE CASE MISSING DESCRIPTION";
		}
	}
	
	public TileType GetTileType()
	{
		/*
		if (!hasBeenInitialized)
		{
			Debug.LogError("Cannot get myTileType on prefab - not instantiated yet.");
		}
		*/
		return myTileType;
	}

	public void Brighten()
	{
		Brighten(1);
	}

	public void Brighten(int _amount)
	{
		SetBrightnessLevel(_amount + GetBrightnessLevel());
	}

	private void UpdateBrightness()
	{
		if (myTileType != TileType.EMPTY)
		{
			Color currentColor = Color.Lerp(Color.black, originalColor, (brightnessLevel / (float)maxBrightness));

			spriteRenderer.color = currentColor;
		}
	}

	public int GetBrightnessLevel()
	{
		return brightnessLevel;
	}

	public void SetBrightnessLevel(int _level)
	{
		brightnessLevel = _level;
		brightnessLevel = Mathf.Min(brightnessLevel, maxBrightness);
		UpdateBrightness();

		isStructureActive = (brightnessLevel > 0 ? true : false);
	}

	public bool IsIlluminated()
	{
		return (GetBrightnessLevel() > 0);
	}

	public virtual void SetTechSettings(TechSettingsDefinition techSettingsDefinition) { }

	protected int GetCurrentStackLimit(City city)
	{
		if (city.IsCityBenefitAvailable(CityBenefits.Benefit.STACK_3))
			return 3;
		else if (city.IsCityBenefitAvailable(CityBenefits.Benefit.STACK_2))
			return 2;
		else
			return 1;
	}

	protected void SpawnEnemy()
	{
		Instantiate(entityToSpawn, transform.position + (new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f))), Quaternion.identity);
	}
}
