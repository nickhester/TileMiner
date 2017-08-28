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

	public List<Actor> actorsToSpawn;
	List<Actor> sequenceToSpawn = new List<Actor>();

	void Update()
	{
		// spawn rift enemies
		if (isSpawning)
		{
			spawnCounter += Time.deltaTime;
			if (spawnCounter >= spawnInterval && sequenceToSpawn.Count > 0)
			{
				SpawnActor(sequenceToSpawn[0]);
				sequenceToSpawn.RemoveAt(0);

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
			// read from wave definition
			List<Actor> actors = new List<Actor>();
			foreach (var waveSet in waveDefinition.Actors)
			{
				// figure out which prefab goes with which actor specified in the definition
				Actor actorPrefab = null;
				for (int i = 0; i < actorsToSpawn.Count; i++)
				{
					if (String.Compare(waveSet.actorName, actorsToSpawn[i].name, true) == 0)
					{
						actorPrefab = actorsToSpawn[i];
					}
				}
				
				// fill list with the correct quantity of them
				for (int i = 0; i < waveSet.quantity; i++)
				{
					actors.Add(actorPrefab);
				}
			}
			// shuffle list
			while (actors.Count > 0)
			{
				int randomInt = UnityEngine.Random.Range(0, actors.Count);
				sequenceToSpawn.Add(actors[randomInt]);
				actors.RemoveAt(randomInt);
			}
			// set intervals
			lifetime = waveDefinition.Length;
			spawnInterval = lifetime / (float)sequenceToSpawn.Count;

			isSpawning = true;
			LevelGenerator.Instance.GetTileGroup(GetCoordinate()).CurrentState = TileGroup.GroupState.ACTIVE;
		}
	}

	protected void SpawnActor(Actor actor)
	{
		Instantiate(actor, transform.position + (new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f))), Quaternion.identity);
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
