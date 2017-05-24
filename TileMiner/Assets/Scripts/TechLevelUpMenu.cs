using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechLevelUpMenu : MonoBehaviour
{
	public List<TechSettings> techSettings = new List<TechSettings>();
	[SerializeField] private GameObject techLayoutGroupPrefab;
	[SerializeField] private GameObject techPropertyButtonPrefab;
	private List<TechSettings> spawnedSettingsGroups = new List<TechSettings>();

	void Start()
	{
		Transform LayoutGroupObjectTransform = transform.GetChild(0);

		foreach (TechSettings tech in techSettings)
		{
			GameObject spawnedTechObject = Instantiate(techLayoutGroupPrefab) as GameObject;
			spawnedTechObject.transform.SetParent(LayoutGroupObjectTransform);
			spawnedTechObject.transform.localScale = Vector3.one;

			spawnedTechObject.GetComponentInChildren<Text>().text = Tile.GetTileNameByEnum(tech.tileType);

			for (int i = 0; i < tech.propertyNames.Count; i++)
			{
				GameObject spawnedPropertyObject = Instantiate(techPropertyButtonPrefab) as GameObject;
				spawnedPropertyObject.transform.SetParent(spawnedTechObject.transform);
				spawnedPropertyObject.transform.localScale = Vector3.one;

				spawnedPropertyObject.GetComponentInChildren<Text>().text = "Upgrade Property: " + tech.propertyNames[i];
			}
		}
	}
}
