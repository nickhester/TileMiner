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

		LayoutGroup layout = transform.GetComponentInChildren<LayoutGroup>();
		for (int i = 0; i < actionSets.Count; i++)
		{
			GameObject go = Instantiate(singleButton.gameObject) as GameObject;
			go.transform.SetParent(layout.transform);
			go.GetComponentInChildren<Text>().text = actionSets[i].name;
			int currentIteration = i;
			go.GetComponent<Button>().onClick.AddListener(delegate { ExecuteAction(currentIteration); });
		}

		if (_actionSets.Count == 1 && _actionSets[0].canBeDefaultIfOnlyOption)
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
