using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Requirements
{
	public enum BuildRequirement
	{																// detail description:
		REQUIRES_ENOUGH_RESOURCES,									// n/a
		TILE_BELOW_MUST_SUPPORT_WEIGHT,								// n/a
		STRUCTURE_REQUIRED_FOR_WEIGHT,		// n/a
		REQUIRES_NEARBY_TILE,										// type based on tile type enum
		REQUIRES_CERTAIN_HEIGHT,									// height required, negative being underground
		REQUIRES_BEING_ON_CERTAIN_TILE,								// tile based on tile type enum
		REQUIRES_ENOUGH_POPULATION									// n/a
	}

	public BuildRequirement requirement;
	public int detail = -1;
	public string description = "";

	public Requirements(BuildRequirement requirement)
	{
		this.requirement = requirement;
	}

	public Requirements(BuildRequirement requirement, int detail)
	{
		this.requirement = requirement;
		this.detail = detail;
	}

	public Requirements(BuildRequirement requirement, int detail, string description)
	{
		this.requirement = requirement;
		this.detail = detail;
		this.description = description;
	}

	public override string ToString()
	{
		if (string.IsNullOrEmpty(description))
		{
			switch (requirement)
			{
				case BuildRequirement.REQUIRES_ENOUGH_RESOURCES:
					return "Not enough Resources.";
				case BuildRequirement.TILE_BELOW_MUST_SUPPORT_WEIGHT:
					return "Can't support weight.";
				case BuildRequirement.STRUCTURE_REQUIRED_FOR_WEIGHT:
					return "Structures Rely on this.";
				case BuildRequirement.REQUIRES_NEARBY_TILE:
					return "Must be built near a " + Tile.GetTileNameByEnum((Tile.TileType)detail);
				case BuildRequirement.REQUIRES_CERTAIN_HEIGHT:
					return "Requires height: " + detail;
				case BuildRequirement.REQUIRES_BEING_ON_CERTAIN_TILE:
					return "Must be built on a " + Tile.GetTileNameByEnum((Tile.TileType)detail);
				case BuildRequirement.REQUIRES_ENOUGH_POPULATION:
					return "Population can't sustain this.";
				default:
					return "*REQUIREMENT ERROR: description not found!*";
			}
		}
		else
		{
			return description;
		}
	}
}
