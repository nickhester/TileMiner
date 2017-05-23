using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StackMultiplier
{
	TileGrid tileGrid;
	Coordinate myCoordinate;
	float baseValue;
	float stackMultiplier;
	System.Type myType;

	public StackMultiplier(TileGrid _tileGrid, Coordinate _myCoordinate, System.Type _myType, float _baseValue, float _stackMultiplier)
	{
		tileGrid = _tileGrid;
		myCoordinate = _myCoordinate;
		myType = _myType;
		stackMultiplier = _stackMultiplier;
		baseValue = _baseValue;
	}

	public int GetStackedAmount()
	{
		return (int)GetStackedAmount_float();
	}

	public float GetStackedAmount_float()
	{
		float returnValue = 0;

		if (tileGrid.GetTileNeighbor(TileGrid.Direction.DOWN, myCoordinate).GetType() == myType)
		{
			// I'm not the base, so don't add anything
			returnValue = 0;
		}
		else
		{
			// I'm the base of the stack, so call recursively upward
			float untruncatedMultipliedValue = MultiplyStackValue(baseValue);
			returnValue = untruncatedMultipliedValue;
		}
		return returnValue;
	}

	public float MultiplyStackValue(float f)
	{
		float retVal = f;
		var t = tileGrid.GetTileNeighbor(TileGrid.Direction.UP, myCoordinate);
		IStackableTile tileAbove = t as IStackableTile;
		if (tileAbove != null && tileAbove.GetType() == myType)
		{
			retVal = stackMultiplier * tileAbove.MultiplyStackValue(retVal);
		}

		return retVal;
	}
}
