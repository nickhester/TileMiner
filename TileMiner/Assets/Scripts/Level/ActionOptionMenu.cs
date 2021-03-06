﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionOptionMenu : MonoBehaviour
{
	[SerializeField] private Button singleButton;

	public void Initialize(List<NamedActionSet> _actionSets)
	{
		if (_actionSets == null || _actionSets.Count == 0)
			return;

		_actionSets.Add(new NamedActionSet("Cancel", new ActionCancel()));

		// create buttons
		LayoutGroup layout = transform.GetComponentInChildren<LayoutGroup>();
		bool atLeastOneActionIsInvalid = false;
		for (int i = 0; i < _actionSets.Count; i++)
		{
			// build string description on button
			string buttonTextString = _actionSets[i].name;
			string costDescription = "";
			if (_actionSets[i].DoesActionSetHaveCost(ref costDescription))
			{
				buttonTextString += " - Cost: " + costDescription;
			}

			// check if button should be disabled
			buttonTextString += "\n";
			bool shouldButtonBeInteractable = true;
			bool isExcludedFromPlayerSelection = false;
			for (int j = 0; j < _actionSets[i].actions.Count; j++)
			{
				List<Requirements> failureReasons = new List<Requirements>();
				if (!_actionSets[i].actions[j].IsActionValid(ref failureReasons, ref isExcludedFromPlayerSelection))
				{
					shouldButtonBeInteractable = false;
					atLeastOneActionIsInvalid = true;
					foreach (Requirements req in failureReasons)
					{
						buttonTextString += " (" + req.ToString() + ") ";
					}
				}
			}

			if (isExcludedFromPlayerSelection)
			{
				continue;
			}

			// actually instantiate the object
			GameObject buttonObject = Instantiate(singleButton.gameObject) as GameObject;
			buttonObject.transform.SetParent(layout.transform);
			buttonObject.transform.localScale = Vector3.one;
			Text buttonText = buttonObject.GetComponentInChildren<Text>();
			
			buttonText.text = buttonTextString;
			NamedActionSet _actionSet = _actionSets[i];
			Button buttonComponent = buttonObject.GetComponent<Button>();
			buttonComponent.onClick.AddListener(delegate { Player.Instance.ExecuteAction(_actionSet); RemoveCurrentOptionMenu(); });

			buttonComponent.interactable = shouldButtonBeInteractable;
		}

		// determine if it can just do an automatic action
		if (_actionSets.Count == 2
			&& _actionSets[0].canBeDefaultIfOnlyOption
			&& !atLeastOneActionIsInvalid)
		{
			RemoveCurrentOptionMenu();
			Player.Instance.ExecuteAction(_actionSets[0]);
		}
	}

	void RemoveCurrentOptionMenu()
	{
		Destroy(this.gameObject);
	}
}
