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

		if (isIlluminateOnStart)
			IlluminateRadial(radius);
	}
	
	void IlluminateRadial(int _radius)
	{
		for (int i = -_radius; i < _radius; i++)
		{
			Coordinate coordinate;
			for (int j = -_radius; j < _radius; j++)
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

	// this is for skylight
	public static int IlluminateDownward(int _maxDistance, int _currentDepth)
	{
		// truncate and negate
		return -(_currentDepth - _maxDistance);
	}
}
