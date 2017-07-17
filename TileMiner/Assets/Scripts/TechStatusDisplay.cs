using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechStatusDisplay : MonoBehaviour
{
	void Start()
	{
		GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
		GlobalInventory gi = gameManager.GetGlobalInventory();
		var nextTech = gi.NumTechRequiredToUnlockNextTech();

		string displayText;

		displayText = string.Format("{0}{1}{2}\nYou have collected {3} Alien Tech. {4}",
								(gi.TechStatus[Tile.TileType.BOMB] == 1 ?			Tile.GetTileNameByEnum(Tile.TileType.BOMB) + " UNLOCKED!\n" : ""),
								(gi.TechStatus[Tile.TileType.DRILL_RIG] == 1 ?		Tile.GetTileNameByEnum(Tile.TileType.DRILL_RIG) + " UNLOCKED!\n" : ""),
								(gi.TechStatus[Tile.TileType.MINERAL_FARM] == 1 ?	Tile.GetTileNameByEnum(Tile.TileType.MINERAL_FARM) + " UNLOCKED!\n" : ""),
								gi.NumTechPieces, 
								(nextTech.Value >= 0 ? string.Format(
																"{0} more needed to unlock next technology: {1}", 
																nextTech.Value.ToString(), 
																Tile.GetTileNameByEnum(nextTech.Key)) : ""));

		Text text = GetComponent<Text>();
		text.text = displayText;
	}
}
