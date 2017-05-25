using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour {

	string levelName;

	public void LoadLevelByName(string levelName)
	{
		DontDestroyOnLoad(gameObject);
		this.levelName = levelName;
		SceneManager.LoadScene("Level Base");
	}

	public string GetQueuedLevel()
	{
		Destroy(gameObject, 0.01f);

		return levelName;
	}
}
