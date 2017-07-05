using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanderConfiguration : MonoBehaviour {

	private Dictionary<string, string> toolsInSlots;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}
	
	public void SetToolsInSlots(Dictionary<string, string> _toolsInSlots)
	{
		toolsInSlots = _toolsInSlots;
	}
}
