using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ActionUpgrade : IAction
{
	public void Execute()
	{
		throw new NotImplementedException();
	}

	public bool IsActionValid(ref List<Requirements> _failureReason)
	{
		return true;
	}
}
