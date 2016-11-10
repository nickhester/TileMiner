using UnityEngine;
using System.Collections;

public abstract class Tile : MonoBehaviour
{
	protected TileGrid tileGrid;
	protected Coordinate myCoordinate;
	protected EventBroadcast eventBroadcast;

	public void Initialize(TileGrid _tileGrid, Coordinate _coordinate)
	{
		tileGrid = _tileGrid;
		myCoordinate = _coordinate;
		eventBroadcast = FindObjectOfType<EventBroadcast>();
	}

	void OnMouseDown()
	{
		print("you clicked me, alright!");
		PlayerClick();
	}

	protected abstract void PlayerClick();

	public abstract void Activate();
}
