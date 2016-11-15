using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ResourceDirt : Resource
{
	public ResourceDirt(int _amount) : base(_amount)
	{
	}

	public override string GetName()
	{
		return "Dirt";
	}
}
