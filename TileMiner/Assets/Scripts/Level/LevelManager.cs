using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	private LevelGenerator levelGenerator;
	
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

	public void Initialize(string levelName)
	{
		LevelDefinition levelDefinition = GenerateDefinitionFromLevelResource(levelName);

		if (levelDefinition == null)
		{
			levelDefinition = GenerateDefinitionFromLevelResource("Level 1");
			Debug.LogWarning("No resource found. defaulting to level 1");
		}

		levelGenerator = GetComponent<LevelGenerator>();
		levelGenerator.Initialize(levelDefinition, GetTechSettingsDefinition(levelName));
	}

	List<TechSettingsDefinition> GetTechSettingsDefinition(string levelName)
	{
		List<TechSettingsDefinition> listOfSettings = new List<TechSettingsDefinition>();
		TechSettingsDefinition techSetting;

		if (levelName == "Level 1")
		{
			techSetting = new TechSettingsDefinition(Tile.TileType.BOMB);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("numTilesRadiusExplosion", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.DRILL_RIG);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("numTilesLifetime", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.MINERAL_FARM);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("intervalToFarm", 0);
			techSetting.propertyLevels.Add("numTileSpawnMax", 0);
			listOfSettings.Add(techSetting);
		}
		else if (levelName == "Level 2")
		{
			techSetting = new TechSettingsDefinition(Tile.TileType.BOMB);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("numTilesRadiusExplosion", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.DRILL_RIG);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("numTilesLifetime", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.MINERAL_FARM);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("intervalToFarm", 0);
			techSetting.propertyLevels.Add("numTileSpawnMax", 0);
			listOfSettings.Add(techSetting);
		}
		else if (levelName == "Level 3")
		{
			techSetting = new TechSettingsDefinition(Tile.TileType.BOMB);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("numTilesRadiusExplosion", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.DRILL_RIG);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("numTilesLifetime", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.MINERAL_FARM);
			techSetting.propertyLevels.Add("isAvailable", 0);
			techSetting.propertyLevels.Add("intervalToFarm", 0);
			techSetting.propertyLevels.Add("numTileSpawnMax", 0);
			listOfSettings.Add(techSetting);
		}
		else if (levelName == "Level 4")
		{
			techSetting = new TechSettingsDefinition(Tile.TileType.BOMB);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("numTilesRadiusExplosion", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.DRILL_RIG);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("numTilesLifetime", 0);
			listOfSettings.Add(techSetting);

			techSetting = new TechSettingsDefinition(Tile.TileType.MINERAL_FARM);
			techSetting.propertyLevels.Add("isAvailable", 1);
			techSetting.propertyLevels.Add("intervalToFarm", 0);
			techSetting.propertyLevels.Add("numTileSpawnMax", 0);
			listOfSettings.Add(techSetting);
		}

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
		
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.DIRT, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.DIRT2, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.ENERGY_WELL, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.GOLD_VEIN, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.STONE, ref levelInfoDict));
		levelDefinition.AddTileGenerationInfo(GenerateInfoForOneType(Tile.TileType.STONE2, ref levelInfoDict));
		
		return levelDefinition;
	}

	TileGenerationInfo GenerateInfoForOneType(Tile.TileType tiletype, ref Dictionary<string, Dictionary<string, string>> levelInfoDict)
	{
		string baseProbability = "baseProbability".ToLower();
		string increaseProbabilityPerRow = "increaseProbabilityPerRow".ToLower();
		string depthRangeStart = "depthRangeStart".ToLower();
		string depthRangeEnd = "depthRangeEnd".ToLower();
		string guaranteeAtLeast = "guaranteeAtLeast".ToLower();

		// define each tile type's level generation settings
		string tileString = Tile.GetTileNameByEnum(tiletype).ToLower();
		return new TileGenerationInfo(tiletype,
										float.Parse(levelInfoDict[tileString][baseProbability]),
										float.Parse(levelInfoDict[tileString][increaseProbabilityPerRow]),
										int.Parse(levelInfoDict[tileString][depthRangeStart]),
										int.Parse(levelInfoDict[tileString][depthRangeEnd]),
										int.Parse(levelInfoDict[tileString][guaranteeAtLeast]));
	}
}
