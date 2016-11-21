using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	List<Resource> resourceList = new List<Resource>();
	EventBroadcast eventBroadcast;

	public Inventory()
	{
		resourceList.Add(new ResourceMineral(0));
		eventBroadcast = GameObject.FindObjectOfType<EventBroadcast>();
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

		eventBroadcast.TriggerEvent(EventBroadcast.Event.RESOURCE_VALUES_UPDATED);
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
