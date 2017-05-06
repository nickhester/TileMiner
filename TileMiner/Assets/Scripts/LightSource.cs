using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LightSource : MonoBehaviour
{
	public int radius = 8;
	private TileGrid tileGrid;
	private Tile myTile;
	public bool isIlluminateOnStart = true;
	
	void Start ()
	{
		myTile = GetComponent<Tile>();
		myTile.Brighten();
		tileGrid = myTile.GetTileGrid();

		FindObjectOfType<LightManager>().RegisterLightSource(this);

		/*
		if (isIlluminateOnStart)
			IlluminateRadial();
			*/
	}

	void OnDestroy()
	{
		TurnOffLightInMyRange();

		LightManager lm = FindObjectOfType<LightManager>();
		if (lm != null)
			lm.UnregisterLightSource(this);
	}
	
	public void IlluminateRadial()
	{
		for (int i = -radius; i < radius; i++)
		{
			Coordinate coordinate;
			for (int j = -radius; j < radius; j++)
			{
				coordinate.x = i + myTile.GetCoordinate().x;
				coordinate.y = j + myTile.GetCoordinate().y;
				float distanceTo = Vector2.Distance(new Vector2(myTile.GetCoordinate().x, myTile.GetCoordinate().y), new Vector2(coordinate.x, coordinate.y));
				// truncate and negate
				int lightValue = -((int)(distanceTo - radius));
				Tile targetTile = tileGrid.GetTileAt(coordinate);
				if (targetTile != null && lightValue > 0)
				{
					targetTile.Brighten(lightValue);
				}
			}
		}
	}

	public void TurnOffLightInMyRange()
	{
		for (int i = -radius; i < radius; i++)
		{
			Coordinate coordinate;
			for (int j = -radius; j < radius; j++)
			{
				coordinate.x = i + myTile.GetCoordinate().x;
				coordinate.y = j + myTile.GetCoordinate().y;
				
				Tile targetTile = tileGrid.GetTileAt(coordinate);
				if (targetTile != null && targetTile.GetBrightnessLevel() > 0)
				{
					targetTile.SetBrightnessLevel(0);
				}
			}
		}
	}
}
