using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
	public abstract void ExecuteAction(NamedActionSet _actionSet);

	protected void ExecuteActionBase(NamedActionSet _actionSet)
	{
		bool standIn = false;
		ExecuteActionBase(_actionSet, ref standIn);
	}

	protected void ExecuteActionBase(NamedActionSet _actionSet, ref bool isActionCancel)
	{
		isActionCancel = false;
		for (int i = 0; i < _actionSet.actions.Count; i++)
		{
			IAction a = _actionSet.actions[i];

			if (a.GetType() == typeof(ActionCancel))
			{
				isActionCancel = true;
			}

			_actionSet.actions[i].Execute();
		}
	}
}
