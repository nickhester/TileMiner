using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceMineral : Resource
{
	public ResourceMineral(int _amount) : base(_amount)
	{
	}

	public override string GetName()
	{
		return "Mineral";
	}
}
