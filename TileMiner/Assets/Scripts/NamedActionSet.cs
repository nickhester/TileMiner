using UnityEngine;
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
}
