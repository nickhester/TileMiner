using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInventory
{
	public int NumTechPieces = 0;

	public Dictionary<Tech, int> TechStatus;

	public enum Tech
	{
		Bomb,
		DrillRig,
		MineralFarm,
	}

	public GlobalInventory()
	{
		// initialize all tech
		TechStatus = new Dictionary<Tech, int>();
		TechStatus.Add(Tech.Bomb, 0);
		TechStatus.Add(Tech.DrillRig, 0);
		TechStatus.Add(Tech.MineralFarm, 0);
	}

	public void AddTechPieces(int n)
	{
		NumTechPieces += n;

		if (NumTechPieces >= 5)
		{
			TechStatus[Tech.Bomb] = 1;
			MonoBehaviour.print("Bomb unlocked");
		}

		if (NumTechPieces >= 10)
		{
			TechStatus[Tech.DrillRig] = 1;
			MonoBehaviour.print("DrillRig unlocked");
		}

		if (NumTechPieces >= 15)
		{
			TechStatus[Tech.MineralFarm] = 1;
			MonoBehaviour.print("MineralFarm unlocked");
		}
	}
}
