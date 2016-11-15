using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IAction
{
	void Execute();

	bool IsActionValid();
}
