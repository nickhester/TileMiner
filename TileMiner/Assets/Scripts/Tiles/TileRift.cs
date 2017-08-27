using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Scripts.Level;

public class TileRift : Tile
{
	bool isSpawning = false;
	float spawnInterval = 0.5f;
	float spawnCounter = 0.0f;
	float lifetime = 20.0f;
	float lifetimeCounter = 0.0f;

	void Update()
	{
		// spawn rift enemies
		if (isSpawning)
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
				CompleteSpawning();
			}
		}
	}
	
	protected override void PlayerClick()
	{
		if (GetIsExposed() && !isSpawning && LevelGenerator.Instance.GetTileGroup(GetCoordinate()).CurrentState == TileGroup.GroupState.READY)
		{
			Activate();
		}
	}

	public override void Activate()
	{
		List<NamedActionSet> namedActionSet = new List<NamedActionSet>();
		List<IAction> actions = new List<IAction>();

		actions = new List<IAction>();
		
		actions.Add(new ActionTriggerRift(this));
		namedActionSet.Add(new NamedActionSet("Break Rift", actions));

		ProposeActions(namedActionSet);
	}

	public override void ReturnResult(bool result)
	{
		if (result)
		{
			isSpawning = true;
			LevelGenerator.Instance.GetTileGroup(GetCoordinate()).CurrentState = TileGroup.GroupState.ACTIVE;
		}
	}

	void CompleteSpawning()
	{
		// replace all tiles in strip group with other resources
		List<Coordinate> stripGroupCoords = LevelGenerator.Instance.GetTileGroup(GetCoordinate()).tileLocations;
		if (stripGroupCoords != null)
		{
			foreach (var coord in stripGroupCoords)
			{
				// for all except this one, spawn strip reward
				if (coord != GetCoordinate())
				{
					LevelGenerator.Instance.ReplaceOneTile(coord, TileType.DIRT2);
				}
			}
		}

		// destroy self
		LevelGenerator.Instance.DestroyOneTile(GetCoordinate());
	}
}
