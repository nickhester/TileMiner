using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private GlobalInventory globalInventory;

	void Start ()
	{
		// initialize stuff
		globalInventory = new GlobalInventory();

		// launch next scene
		DontDestroyOnLoad(gameObject);
		SceneManager.LoadScene("Level Selection");
	}

	public int GetTechLevel(GlobalInventory.Tech techName)
	{
		return globalInventory.TechStatus[techName];
	}

	public void AddNewTechPieces(int n)
	{
		globalInventory.AddTechPieces(n);
	}
}
