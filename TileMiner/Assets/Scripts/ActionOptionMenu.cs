using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionOptionMenu : MonoBehaviour
{
	[SerializeField] private Button singleButton;

	public void Initialize(List<NamedActionSet> _actionSets, Player _player)
	{
		_actionSets.Add(new NamedActionSet("Cancel", new ActionCancel()));

		// create buttons
		LayoutGroup layout = transform.GetComponentInChildren<LayoutGroup>();
		bool atLeastOneActionIsInvalid = false;
		for (int i = 0; i < _actionSets.Count; i++)
		{
			GameObject buttonObject = Instantiate(singleButton.gameObject) as GameObject;
			buttonObject.transform.SetParent(layout.transform);
			Text buttonText = buttonObject.GetComponentInChildren<Text>();
			buttonText.text = _actionSets[i].name;
			int currentIteration = i;
			Button buttonComponent = buttonObject.GetComponent<Button>();
			buttonComponent.onClick.AddListener(delegate { _player.ExecuteAction(currentIteration, _actionSets); });

			// check if button should be disabled
			for (int j = 0; j < _actionSets[i].actions.Count; j++)
			{
				string reason = "";
				if (!_actionSets[i].actions[j].IsActionValid(ref reason))
				{
					buttonComponent.interactable = false;
					atLeastOneActionIsInvalid = true;
					buttonText.text += "\n(" + reason.Trim() + ")";
				}
			}
		}

		// determine if it can just do an automatic action
		if (_actionSets.Count == 2
			&& _actionSets[0].canBeDefaultIfOnlyOption
			&& !atLeastOneActionIsInvalid)
		{
			_player.ExecuteAction(0, _actionSets);
		}
	}
}
