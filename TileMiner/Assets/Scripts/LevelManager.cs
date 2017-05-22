using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
	private LevelGenerator levelGenerator;
	
	void Start ()
	{
		Initialize();
	}

	public void Initialize()
	{
		LevelDefinition levelDefinition = new LevelDefinition(200);

		// define each tile type's level generation settings		// tile type				base prob	increase prob	depth start		depth end	guarantee #
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.DIRT,		1.0f,		0.0f,			0,				999,		0));
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.DIRT2,		0.1f,		0.02f,			15,				999,		0));
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.ENERGY_WELL,	0.1f,		0.02f,			45,				55,			5));
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.GOLD_VEIN,	0.02f,		0.01f,			10,				55,			0));
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.STONE,		0.2f,		0.037f,			2,				999,		0));
		levelDefinition.AddTileGenerationInfo(new TileGenerationInfo(Tile.TileType.STONE2,		0.05f,		0.08f,			25,				999,		0));

		levelGenerator = GetComponent<LevelGenerator>();
		levelGenerator.Initialize(levelDefinition);
	}
}
