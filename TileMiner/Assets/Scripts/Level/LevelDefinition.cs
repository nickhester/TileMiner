using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDefinition
{
	public int? mapHeight = null;
	public int? numSkyTiles = null;

	public List<TileGenerationInfo> tileGenerationInfoList = new List<TileGenerationInfo>();
	public List<WaveDefinition> waveDefinitionList = new List<WaveDefinition>();

	public LevelDefinition(int mapHeight)
	{
		this.mapHeight = mapHeight;
	}

	public void AddTileGenerationInfo(TileGenerationInfo info)
	{
		tileGenerationInfoList.Add(info);
	}

	public void AddWaveDefinitions(List<WaveDefinition> info)
	{
		waveDefinitionList.AddRange(info);
	}

	public TileGenerationInfo GetTileGenerationInfoFromType(Tile.TileType type)
	{
		foreach (var info in tileGenerationInfoList)
		{
			if (info.tileType == type)
				return info;
		}
		return null;
	}
}

public static class LevelDefinitionParser
{
	public static TileGenerationInfo GenerateInfoForOneType(Tile.TileType tileType, ref Dictionary<string, Dictionary<string, string>> levelInfoDict)
	{
		string tileString = Tile.GetTileNameByEnum(tileType).ToLower();

		// check if strip
		string numStrips = "numStrips".ToLower();
		if (levelInfoDict[tileString].ContainsKey(numStrips))
		{
			return new TileGenerationInfo(tileType,
				int.Parse(levelInfoDict[tileString][numStrips]));
		}

		string baseProbability = "baseProbability".ToLower();
		string increaseProbabilityPerRow = "increaseProbabilityPerRow".ToLower();
		string depthRangeStart = "depthRangeStart".ToLower();
		string depthRangeEnd = "depthRangeEnd".ToLower();
		string guaranteeAtLeast = "guaranteeAtLeast".ToLower();

		// define each tile type's level generation settings
		return new TileGenerationInfo(tileType,
										float.Parse(levelInfoDict[tileString][baseProbability]),
										float.Parse(levelInfoDict[tileString][increaseProbabilityPerRow]),
										int.Parse(levelInfoDict[tileString][depthRangeStart]),
										int.Parse(levelInfoDict[tileString][depthRangeEnd]),
										int.Parse(levelInfoDict[tileString][guaranteeAtLeast]));
	}

	public static List<WaveDefinition> GenerateWaveInfoForType(Tile.TileType tileType, ref Dictionary<string, Dictionary<string, string>> levelInfoDict)
	{
		List<WaveDefinition> returnList = new List<WaveDefinition>();

		string tileString = Tile.GetTileNameByEnum(tileType).ToLower();

		string lengthOfWave = "lengthOfWave".ToLower();
		string actor = "actor".ToLower();
		string quantity = "quantity".ToLower();
		string numStrips = "numStrips".ToLower();

		int numberOfStrips = int.Parse(levelInfoDict[tileString][numStrips]);
		for (int i = 0; i < numberOfStrips; i++)
		{
			float lengthOfWaveFloat = float.Parse(levelInfoDict[tileString][i + "-" + lengthOfWave]);
			int numActors = 0;
			while (levelInfoDict[tileString].ContainsKey(i + "-" + actor + "-" + numActors))
				numActors++;

			List<WaveSet> waveSets = new List<WaveSet>();
			for (int j = 0; j < numActors; j++)
			{
				string actorName = levelInfoDict[tileString][i + "-" + actor + "-" + j];
				string quantityFromDef = levelInfoDict[tileString][i + "-" + quantity + "-" + j];
				waveSets.Add(new WaveSet(actorName, int.Parse(quantityFromDef)));
			}

			returnList.Add(new WaveDefinition(tileType, lengthOfWaveFloat, waveSets));
		}
		return returnList;
	}
}

public class TileGenerationInfo
{
	public Tile.TileType tileType;
	public float baseProbability = 0f;
	public float increaseProbabilityPerRow = 0f;
	public int depthRangeStart;
	public int depthRangeEnd;
	public int guaranteeAtLeast = 0;

	public bool isStripType = false;
	public int numStrips = 0;

	public TileGenerationInfo (Tile.TileType tileType, float baseProbability, float increaseProbabilityPerRow, int depthRangeStart, int depthRangeEnd, int guaranteeAtLeast)
	{
		this.tileType = tileType;
		this.baseProbability = baseProbability;
		this.increaseProbabilityPerRow = increaseProbabilityPerRow;
		this.depthRangeStart = depthRangeStart;
		this.depthRangeEnd = depthRangeEnd;
		this.guaranteeAtLeast = guaranteeAtLeast;
	}

	public TileGenerationInfo(Tile.TileType tileType, int numStrips)
	{
		this.tileType = tileType;
		isStripType = true;
		this.numStrips = numStrips;
	}
}