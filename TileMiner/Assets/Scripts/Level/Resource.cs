using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resource
{
	private string name;
	private int amount = 0;
	public enum ResourceType
	{
		MINERAL,
		ENERGY,
		GOLD
	}
	private ResourceType resourceType;

	public Resource(int _amount, ResourceType _resourceType)
	{
		amount = _amount;
		resourceType = _resourceType;
		name = _resourceType.ToString();
	}

	public void Add(int _amountChange)
	{
		if (amount + _amountChange < 0)
			Debug.LogError("Resource less than zero");

		amount += _amountChange;
	}

	public int GetAmount()
	{
		return amount;
	}

	public string GetName()
	{
		return name;
	}

	public ResourceType GetResourceType()
	{
		return resourceType;
	}
}
