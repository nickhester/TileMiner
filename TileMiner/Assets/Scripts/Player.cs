using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour
{
	private Inventory myInventory;
	public ActionOptionMenu actionOptionMenuPrefab;
	private ActionOptionMenu currentActionMenu;

	void Start ()
	{
		myInventory = new Inventory();
	}
	
	void Update ()
	{

	}

	public Inventory GetInventory()
	{
		return myInventory;
	}

	public void ProposeActions(List<NamedActionSet> _actionSets)
	{
		// create menu
		if (currentActionMenu == null)
		{
			GameObject menu = Instantiate(actionOptionMenuPrefab.gameObject) as GameObject;
			currentActionMenu = menu.GetComponent<ActionOptionMenu>();
			currentActionMenu.Initialize(_actionSets, this);
		}
		else
		{
			Debug.LogError("Old action menu not null, trying to create new one");
		}
	}

	public void ExecuteAction(int _index, List<NamedActionSet> _actionSets)
	{
		for (int j = 0; j < _actionSets[_index].actions.Count; j++)
		{
			_actionSets[_index].actions[j].Execute();
		}

		Destroy(currentActionMenu.gameObject);
	}
}
