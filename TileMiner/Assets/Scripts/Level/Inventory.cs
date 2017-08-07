using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
	List<Resource> resourceList = new List<Resource>();
	
	public void AddResource(Resource _resource)
	{
		bool foundResource = false;
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetResourceType() == _resource.GetResourceType())
			{
				foundResource = true;
				resourceList[i].Add(_resource.GetAmount());
			}
		}

		if (!foundResource)
		{
			resourceList.Add(new Resource(_resource.GetAmount(), _resource.GetResourceType()));
		}

		EventBroadcast.Instance.TriggerEvent(EventBroadcast.Event.RESOURCE_VALUES_UPDATED);
	}

	public int GetResource(Resource.ResourceType _resourceType)
	{
		for (int i = 0; i < resourceList.Count; i++)
		{
			if (resourceList[i].GetResourceType() == _resourceType)
			{
				return resourceList[i].GetAmount();
			}
		}

		return 0;
	}
}
