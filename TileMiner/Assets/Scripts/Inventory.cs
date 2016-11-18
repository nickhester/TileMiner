using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	List<Resource> resourceList = new List<Resource>();

	public Inventory()
	{
		resourceList.Add(new ResourceDirt(0));
	}

	public void AddResource(Resource _resource)
	{
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetType() == _resource.GetType())
			{
				resourceList[i].Add(_resource.GetAmount());
				//MonoBehaviour.print(resourceList[i].GetName() + ": " + resourceList[i].GetAmount());
			}
		}
	}

	public int GetResource(System.Type _resourceType)
	{
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetType() == _resourceType)
			{
				return resourceList[i].GetAmount();
			}
		}

		Debug.LogError("Attempted to get resource from inventory that doesn't exist");
		return -1;
	}
}
