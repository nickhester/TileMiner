using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ActionOptionMenu : MonoBehaviour
{
	private List<NamedActionSet> actionSets;
	[SerializeField] private Button singleButton;

	public void Initialize(List<NamedActionSet> _actionSets)
	{
		actionSets = _actionSets;

		actionSets.Add(new NamedActionSet("Cancel", new ActionCancel()));

		// create buttons
		LayoutGroup layout = transform.GetComponentInChildren<LayoutGroup>();
		bool atLeastOneActionIsInvalid = false;
		for (int i = 0; i < actionSets.Count; i++)
		{
			GameObject buttonObject = Instantiate(singleButton.gameObject) as GameObject;
			buttonObject.transform.SetParent(layout.transform);
			buttonObject.GetComponentInChildren<Text>().text = actionSets[i].name;
			int currentIteration = i;
			Button buttonComponent = buttonObject.GetComponent<Button>();
			buttonComponent.onClick.AddListener(delegate { ExecuteAction(currentIteration); });

			// check if button should be disabled
			for (int j = 0; j < actionSets[i].actions.Count; j++)
			{
				if (!actionSets[i].actions[j].IsActionValid())
				{
					buttonComponent.interactable = false;
					atLeastOneActionIsInvalid = true;
				}
			}
		}

		// determine if it can just do an automatic action
		if (_actionSets.Count == 2
			&& _actionSets[0].canBeDefaultIfOnlyOption
			&& !atLeastOneActionIsInvalid)
		{
			ExecuteAction(0);
		}
	}

	public void ExecuteAction(int _index)
	{
		for (int j = 0; j < actionSets[_index].actions.Count; j++)
		{
			actionSets[_index].actions[j].Execute();
		}

		Destroy(gameObject);
	}
}
