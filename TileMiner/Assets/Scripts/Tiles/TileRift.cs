using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class TileRift : Tile
{
	float spawnInterval = 0.5f;
	float spawnCounter = 0.0f;
	float lifetime = 20.0f;
	float lifetimeCounter = 0.0f;

	void Update()
	{
		spawnCounter += Time.deltaTime;
		if (spawnCounter >= spawnInterval)
		{
			SpawnEnemy();

			spawnCounter = 0.0f;
		}
		lifetimeCounter += Time.deltaTime;
		if (lifetimeCounter > lifetime)
		{
			LevelGenerator.Instance.DestroyOneTile(GetCoordinate());
		}
	}
	
	protected override void PlayerClick()
	{
		// no player action
	}

	public override void Activate()
	{
		// no player action
	}
}
