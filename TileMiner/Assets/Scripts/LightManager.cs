﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
	List<LightSource> allLightSources = new List<LightSource>();
	private TileGrid tileGrid;
	[SerializeField] private int depthOfSkylight = 12;
	LevelGenerator levelGenerator;

	public void Initialize(TileGrid _tileGrid)
	{
		tileGrid = _tileGrid;
		levelGenerator = GetComponent<LevelGenerator>();

		ApplyMinimumSkylight();
	}

	void ApplyMinimumSkylight()
	{
		int numSkyTiles = levelGenerator.GetNumSkyTiles();
		int mapWidth = levelGenerator.GetMapWidth();
		int totalDepthToIlluminate = numSkyTiles + depthOfSkylight;
		Coordinate coordinate;
		
		for (int i = 0; i < totalDepthToIlluminate; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				coordinate.x = j;
				coordinate.y = i;

				int lightValue = 0;
				Tile t = tileGrid.GetTileAt(coordinate);
				if (t != null)
				{
					if (coordinate.y <= numSkyTiles)
					{
						lightValue = 5;
					}
					else
					{
						// fade to black at end
						lightValue = -(coordinate.y - totalDepthToIlluminate);
					}
					t.Brighten(lightValue);
				}
			}
		}
	}

	void TurnOffSkylight()
	{
		int numSkyTiles = levelGenerator.GetNumSkyTiles();
		int mapWidth = levelGenerator.GetMapWidth();
		int totalDepthToIlluminate = numSkyTiles + depthOfSkylight;
		Coordinate coordinate;

		for (int i = 0; i < totalDepthToIlluminate; i++)
		{
			for (int j = 0; j < mapWidth; j++)
			{
				coordinate.x = j;
				coordinate.y = i;
				
				Tile t = tileGrid.GetTileAt(coordinate);
				if (t != null)
				{
					t.SetBrightnessLevel(0);
				}
			}
		}
	}
	
	public void RegisterLightSource(LightSource lightSource)
	{
		allLightSources.Add(lightSource);

		RecalculateIllumination();
	}

	public void UnregisterLightSource(LightSource lightSource)
	{
		allLightSources.Remove(lightSource);

		RecalculateIllumination();
	}

	void RecalculateIllumination()
	{
		TurnOffSkylight();

		foreach (var light in allLightSources)
		{
			light.TurnOffLightInMyRange();
		}

		ApplyMinimumSkylight();

		foreach (var light in allLightSources)
		{
			light.IlluminateRadial();
		}
	}
}
