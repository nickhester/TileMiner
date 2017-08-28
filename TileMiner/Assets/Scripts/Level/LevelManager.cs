using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	// static ref to singleton
	private static LevelManager instance;
	public static LevelManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<LevelManager>();
			}
			return instance;
		}
	}

	public enum LevelPhase
	{
		BUILD_CITY,
		MAIN
	}
	public LevelPhase CurrentLevelPhase = LevelPhase.BUILD_CITY;

	void Start ()
	{
		LevelSelector levelSelector = FindObjectOfType<LevelSelector>();
		if (levelSelector != null)
		{
			Initialize(levelSelector.GetQueuedLevel());
			print("loading from levelSelector");
		}
		else
		{
			Initialize("Level 1");
			print("loading default");
		}
	}

	void OnDestroy()
	{
		instance = null;
	}

	public void ReportLevelCompleted(bool success)
	{
		print("Level Complete Reported");
		if (success)
			OnLevelSucceeded();
		else
			OnLevelFailed();
	}

	public void ReportCityBuilt()
	{
		CurrentLevelPhase = LevelPhase.MAIN;
	}

	void OnLevelFailed()
	{
		OnLevelComplete();
	}

	void OnLevelSucceeded()
	{
		GameManager gm = GameObject.FindObjectOfType<GameManager>();
		gm.AddNewTechPieces(Player.Instance.GetInventory().GetResource(Resource.ResourceType.ALIEN_TECH));

		OnLevelComplete();
	}

	void OnLevelComplete()
	{
		SceneManager.LoadScene("Level Selection");
	}

	public void Initialize(string levelName)
	{
		LevelDefinition levelDefinition = GenerateDefinitionFromLevelResource(levelName);

		if (levelDefinition == null)
		{
			levelDefinition = GenerateDefinitionFromLevelResource("Level 1");
			Debug.LogWarning("No resource found. defaulting to level 1");
		}

		LevelGenerator.Instance.Initialize(levelDefinition, GetTechSettingsDefinition(levelName));
	}

	List<TechSettingsDefinition> GetTechSettingsDefinition(string levelName)
	{
		List<TechSettingsDefinition> listOfSettings = new List<TechSettingsDefinition>();
		TechSettingsDefinition techSetting;
		GameManager gm = GameObject.FindObjectOfType<GameManager>();
		if (gm == null)
			Debug.LogError("Unable to find GameManager object");

		techSetting = new TechSettingsDefinition(Tile.TileType.BOMB);
		techSetting.propertyLevels.Add("isAvailable", gm.GetTechLevel(Tile.TileType.BOMB));
		techSetting.propertyLevels.Add("numTilesRadiusExplosion", 0);
		listOfSettings.Add(techSetting);

		techSetting = new TechSettingsDefinition(Tile.TileType.DRILL_RIG);
		techSetting.propertyLevels.Add("isAvailable", gm.GetTechLevel(Tile.TileType.DRILL_RIG));
		techSetting.propertyLevels.Add("numTilesLifetime", 0);
		listOfSettings.Add(techSetting);

		techSetting = new TechSettingsDefinition(Tile.TileType.MINERAL_FARM);
		techSetting.propertyLevels.Add("isAvailable", gm.GetTechLevel(Tile.TileType.MINERAL_FARM));
		techSetting.propertyLevels.Add("intervalToFarm", 0);
		techSetting.propertyLevels.Add("numTileSpawnMax", 0);
		listOfSettings.Add(techSetting);
		
		return listOfSettings;
	}

	LevelDefinition GenerateDefinitionFromLevelResource(string levelName)
	{
		FileIO fileIO = new FileIO(levelName, "txt");
		string fileText = fileIO.GetFileText();
		if (fileText == null)
			return null;

		var levelInfoDict = new Dictionary<string, Dictionary<string, string>>();

		foreach (var line in fileText.Split('\n'))
		{
			if (line.StartsWith("//"))		// line comment
				continue;

			if (!(string.IsNullOrEmpty(line) || line.Trim().Length == 0))	// if not an empty line
			{
				string dataIdentifier = line.Split(':')[0].ToLower();
				string dataValue = line.Split(':')[1].Trim();

				string dataCategory = dataIdentifier.Split('/')[0];
				string dataProperty = dataIdentifier.Split('/')[1];

				if (!levelInfoDict.ContainsKey(dataCategory))
					// add key to external dict
					levelInfoDict.Add(dataCategory, new Dictionary<string, string>());

				levelInfoDict[dataCategory].Add(dataProperty, dataValue);
			}
		}

		LevelDefinition levelDefinition = new LevelDefinition(int.Parse(levelInfoDict["levelmanager"]["mapheight"]));
		
		// todo: do this procedurally based on what's available
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.DIRT, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.DIRT2, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.GOLD_VEIN, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.STONE, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.STONE2, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.ALIEN_TECH, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.ENERGY_WELL, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(LevelDefinitionParser.GenerateInfoForOneType(Tile.TileType.RIFT, ref levelInfoDict));

		// add wave definitions
		levelDefinition.AddWaveDefinitions(LevelDefinitionParser.GenerateWaveInfoForType(Tile.TileType.RIFT, ref levelInfoDict));

		return levelDefinition;
	}
}
