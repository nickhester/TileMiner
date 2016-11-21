using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionCancel : IAction
{
	public void Execute()
	{
		// do nothing, just pass through!
	}

	public bool IsActionValid(ref string _failureReason)
	{
		return true;
	}
}
