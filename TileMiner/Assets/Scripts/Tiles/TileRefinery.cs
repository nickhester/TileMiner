using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileRefinery : Tile, IStackableTile
{
	[Header("Type-Specific Properties")]
	[SerializeField] private int mineralEarnPerInterval = 1;
	[SerializeField] private int mineralEarnBaseInterval = 1;
	private float intervalCounter = 0.0f;
	[SerializeField] protected float stackMultiplierInterval;
	private StackMultiplier stackMultiplier;
	[SerializeField] protected float stackMultiplierCost = 2.0f;

	public override void Initialize(TileGrid _tileGrid, Coordinate _coordinate, TileType _type)
	{
		base.Initialize(_tileGrid, _coordinate, _type);

		stackMultiplier = new StackMultiplier(tileGrid, myCoordinate, this.GetType(), 1.0f/mineralEarnBaseInterval, stackMultiplierInterval);
	}

	private void Update()
	{
		if (isStructureActive)
		{
			intervalCounter += Time.deltaTime;
			float stackedAmount = stackMultiplier.GetStackedAmount_float();		// TODO: Great place to optimize here - don't do this every update loop
			if (stackedAmount != 0.0f && intervalCounter > (1.0f / stackedAmount))
			{
				int mineralEarnMultiplier = (City.Instance.IsCityBenefitAvailable(CityBenefits.Benefit.REFINERY_DOUBLE) ? 2 : 1);
				ActionAdjustResources actionAdjustResources = new ActionAdjustResources(new Resource((mineralEarnPerInterval * mineralEarnMultiplier), Resource.ResourceType.MINERAL));
				actionAdjustResources.Execute();
				intervalCounter = 0.0f;
			}
		}
	}

	protected override void PlayerClick()
	{
		Activate();
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();

		namedActionSet.Add(new NamedActionSet("Destroy", GetDestroyAction()));

		ProposeActions(namedActionSet);
	}

	public float MultiplyStackValue(float f)
	{
		return stackMultiplier.MultiplyStackValue(f);
	}

	// called on prefab
	public override bool CheckIfValidToBuild(TileGrid _tileGrid, Coordinate _myCoordinate, ref List<Requirements> _failureReason, ref bool isExcludedFromPlayerSelection)
	{
		bool isValid = true;

		Tile tileBelow = _tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, _myCoordinate);

		if (tileBelow
			&& (tileBelow.GetType() == typeof(TileDirt)
				|| tileBelow.GetType() == typeof(TileStone)
				|| tileBelow.GetType() == typeof(TileRefinery)))
		{
			// valid
		}
		else
		{
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE, (int)Tile.TileType.DIRT, "Not on ground or other Refinery."));
			isExcludedFromPlayerSelection = true;
			isValid = false;
		}

		// check structure height
		bool passesHeightLimitCheck = BuildRequirementsAnalyzer.IsNotPastHeightLimit(_myCoordinate, _tileGrid, Tile.TileType.REFINERY, GetCurrentStackLimit(City.Instance));

		if (!passesHeightLimitCheck)
		{
			isExcludedFromPlayerSelection = true;
			isValid = false;
			_failureReason.Add(new Requirements(Requirements.BuildRequirement.REQUIRES_UNDER_STRUCTURE_HEIGHT_LIMIT, 3));
		}
		return isValid;
	}

	public override List<Resource> GetResourceAdjustmentToBuild(TileGrid _tileGrid, Coordinate _buildTarget)
	{
		return GetResourceAdjustmentToBuild_stacked(_tileGrid, _buildTarget, stackMultiplierCost);
	}
}
