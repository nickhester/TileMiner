﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NamedActionSet
{
	public string name;
	public List<IAction> actions;
	public bool canBeDefaultIfOnlyOption = false;

	public NamedActionSet(string _name, List<IAction> _actions)
	{
		name = _name;
		actions = _actions;
	}

	public NamedActionSet(string _name, List<IAction> _actions, bool _canBeDefaultIfOnlyOption)
	{
		name = _name;
		actions = _actions;
		canBeDefaultIfOnlyOption = _canBeDefaultIfOnlyOption;
	}

	public NamedActionSet(string _name, IAction _singleAction)
	{
		List<IAction> a = new List<IAction>();
		a.Add(_singleAction);
		name = _name;
		actions = a;
	}

	public bool DoesActionSetHaveCost(ref string costDescription)
	{
		bool retVal = false;
		foreach (IAction action in actions)
		{
			ActionAdjustResources aar = action as ActionAdjustResources;
			if (aar != null)
			{
				if (aar.GetResourceAdjustmentAmount() < 0)
				{
					retVal = true;
					costDescription += -aar.GetResourceAdjustmentAmount() + " " + aar.GetResourceAdjustmentName().ToLower() + " ";
				}
			}
		}
		return retVal;
	}

	public override string ToString()
	{
		return name;
	}
}
