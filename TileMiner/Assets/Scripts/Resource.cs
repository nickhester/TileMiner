using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Resource
{
	private string name;
	private int amount = 0;

	public void Add(int _amountChange)
	{
		amount += _amountChange;
	}

	public int GetAmount()
	{
		return amount;
	}

	public abstract string GetName();
}
