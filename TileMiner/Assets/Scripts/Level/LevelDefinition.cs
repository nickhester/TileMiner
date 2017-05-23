﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDefinition
{
	public int? mapHeight = null;
	public int? numSkyTiles = null;

	public List<TileGenerationInfo> tileGenerationInfoList = new List<TileGenerationInfo>();
	
	public LevelDefinition(int mapHeight)
	{
		this.mapHeight = mapHeight;
	}

	public void AddTileGenerationInfo(TileGenerationInfo info)
	{
		tileGenerationInfoList.Add(info);
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

public class TileGenerationInfo
{
	public Tile.TileType tileType;
	public float baseProbability;
	public float increaseProbabilityPerRow;
	public int depthRangeStart;
	public int depthRangeEnd;
	public int guaranteeAtLeast;

	public TileGenerationInfo (Tile.TileType tileType, float baseProbability, float increaseProbabilityPerRow, int depthRangeStart, int depthRangeEnd, int guaranteeAtLeast)
	{
		this.tileType = tileType;
		this.baseProbability = baseProbability;
		this.increaseProbabilityPerRow = increaseProbabilityPerRow;
		this.depthRangeStart = depthRangeStart;
		this.depthRangeEnd = depthRangeEnd;
		this.guaranteeAtLeast = guaranteeAtLeast;
	}
}