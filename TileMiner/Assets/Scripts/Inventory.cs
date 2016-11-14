using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	List<Resource> resourceList = new List<Resource>();
	Dictionary<Resource, int> inventory = new Dictionary<Resource, int>();

	public Inventory()
	{
		resourceList.Add(new ResourceDirt());
	}

	public void AddDirt(int value)
	{
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetType() == typeof(ResourceDirt))
			{
				ResourceDirt dirt = (ResourceDirt)resourceList[i];
				dirt.Add(value);
				MonoBehaviour.print(dirt.GetName() + ": " + dirt.GetAmount());
				return;
			}
		}
	}

	public void AddResource(Resource _resource, int _amount)
	{
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetType().Equals(_resource.GetType()))
			{
				resourceList[i].Add(_amount);
				MonoBehaviour.print(resourceList[i].GetName() + ": " + resourceList[i].GetAmount());
			}
		}
	}
}
