using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	private int numDirt = 0;
	private int numStone = 0;

	public void AddDirt(int value)
	{
		numDirt += value;

		MonoBehaviour.print("dirt: " + numDirt);
	}

	public void AddStone(int value)
	{
		numStone += value;

		MonoBehaviour.print("stone: " + numStone);
	}
}
