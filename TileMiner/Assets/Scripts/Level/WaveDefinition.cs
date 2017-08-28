using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDefinition
{
	public float Length;
	public List<WaveSet> Actors;
	public Tile.TileType TileType;

	public WaveDefinition(Tile.TileType tileType, float Length, List<WaveSet> Actors)
	{
		this.TileType = tileType;
		this.Length = Length;
		this.Actors = Actors;
	}
}

public class WaveSet
{
	public string actorName;
	public int quantity;

	public WaveSet(string actorName, int quantity)
	{
		this.actorName = actorName;
		this.quantity = quantity;
	}
}