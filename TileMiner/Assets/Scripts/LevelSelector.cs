using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	public void LoadLevelByName(string levelName)
	{
		SceneManager.LoadScene(levelName);
	}
}
