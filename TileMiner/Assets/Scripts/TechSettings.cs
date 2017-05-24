using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechSettings : MonoBehaviour
{
	public Tile.TileType tileType;
	public List<string> propertyNames = new List<string>();
	//public List<int> currentLevel = new List<int>();
	public List<int> maxLevel = new List<int>();

	/*
	public int GetCurrentLevel(string propertyName)
	{
		for (int i = 0; i < propertyNames.Count; i++)
		{
			if (propertyNames[i] == propertyName)
			{
				return currentLevel[i];
			}
		}
		return -1;
	}
	*/

	public int GetMaxLevel(string propertyName)
	{
		for (int i = 0; i < propertyNames.Count; i++)
		{
			if (propertyNames[i] == propertyName)
			{
				return maxLevel[i];
			}
		}
		return -1;
	}
}
