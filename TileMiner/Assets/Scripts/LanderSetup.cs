using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LanderSetup : MonoBehaviour
{
	[SerializeField] private GameObject ParentOfShipSlots;
	[SerializeField] private GameObject ParentOfAvailableTools;
	private List<Button> shipSlots = new List<Button>();
	private List<Button> availableTools = new List<Button>();
	private Button activeSlot;
	[SerializeField] private LanderConfiguration landerConfigurationObject;
	
	void Start ()
	{
		shipSlots.AddRange(ParentOfShipSlots.GetComponentsInChildren<Button>());
		availableTools.AddRange(ParentOfAvailableTools.GetComponentsInChildren<Button>());
	}

	public void SlotClicked(Object slotClicked)
	{
		GameObject goClicked = slotClicked as GameObject;
		Button buttonClicked = goClicked.GetComponent<Button>();
		activeSlot = buttonClicked;
	}

	public void ToolClicked(Object toolClicked)
	{
		GameObject goClicked = toolClicked as GameObject;
		Button buttonClicked = goClicked.GetComponent<Button>();

		if (activeSlot != null)
		{
			Text slotText = activeSlot.GetComponentInChildren<Text>();
			string slotName = slotText.text.Split('\n')[0];
			string toolName = buttonClicked.GetComponentInChildren<Text>().text;
			slotText.text = slotName + "\n" + toolName;
		}
	}

	public void StartClicked()
	{
		Dictionary<string, string> toolsInSlots = new Dictionary<string, string>();
		foreach (var slot in shipSlots)
		{
			string text = slot.GetComponentInChildren<Text>().text;
			if (text.Split('\n').Length > 1)
			{
				toolsInSlots.Add(text.Split('\n')[0], text.Split('\n')[1]);
			}
		}
		landerConfigurationObject.SetToolsInSlots(toolsInSlots);

		SceneManager.LoadScene("Level Selection");
	}
}
