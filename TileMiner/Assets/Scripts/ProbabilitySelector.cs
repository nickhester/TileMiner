using System;
using System.Collections;
using System.Collections.Generic;

public class ProbabilitySelector {

	private List<TileProbability> probabilities;

	public ProbabilitySelector(List<TileProbability> probabilities)
	{
		this.probabilities = probabilities;
	}

	public Tile.TileType GetTileType(int depth)
	{
		float totalProbability = 0.0f;
		List<float> currentProbability = new List<float>();

		// first add up all probabilities to get range
		foreach (TileProbability tile in probabilities)
		{
			
			if (depth < tile.rangeStart || depth > tile.rangeEnd)
			{
				// not within range, probability should be zero
				currentProbability.Add(0.0f);
				continue;
			}
			float prob = TileProbabilityAtDepth(tile, depth);
			currentProbability.Add(prob);
			totalProbability += prob;
		}

		// then choose one
		float randomSelection = UnityEngine.Random.Range(0.0f, totalProbability);
		int selectionIndex = GetIndexFromProbability(randomSelection, currentProbability);
		return probabilities[selectionIndex].tileType;
	}

	int GetIndexFromProbability(float randomSelection, List<float> currentProbability)
	{
		float currentSum = 0.0f;
		for (int i = 0; i < probabilities.Count; i++)
		{
			currentSum += currentProbability[i];
			if (currentSum - randomSelection > 0.0f)
			{
				return i;
			}
		}
		UnityEngine.Debug.LogError("Couldn't determine tile from probability");
		return -1;
	}

	float TileProbabilityAtDepth(TileProbability tp, int depth)
	{
		int linesFromStart = depth - tp.rangeStart;
		return tp.baseProbability + (tp.increasePerLine * linesFromStart);
	}
}

public struct TileProbability
{
	public Tile.TileType tileType;
	public float baseProbability;
	public float increasePerLine;
	public int rangeStart;
	public int rangeEnd;

	public TileProbability(Tile.TileType tileType, float baseProbability, float increasePerLine, int rangeStart, int rangeEnd)
	{
		this.tileType = tileType;
		this.baseProbability = baseProbability;
		this.increasePerLine = increasePerLine;
		this.rangeStart = rangeStart;
		this.rangeEnd = rangeEnd;
	}
}