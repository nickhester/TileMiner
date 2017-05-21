using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EventBroadcast : MonoBehaviour
{
	private Dictionary<Event, List<IEventSubscriber>> subscriptions = new Dictionary<Event, List<IEventSubscriber>>();

	public enum Event
	{
		PLAYER_ACTION,
		PLAYER_COLLECTED_DIRT,
		PLAYER_COLLECTED_STONE,
		PLAYER_SELECTED_STONE,
		RESOURCE_VALUES_UPDATED
	}

	public void SubscribeToEvent(Event _event, IEventSubscriber _subscriber)
	{
		if (!subscriptions.ContainsKey(_event))
		{
			List<IEventSubscriber> newList = new List<IEventSubscriber>();
			subscriptions.Add(_event, newList);
		}
		subscriptions[_event].Add(_subscriber);
	}

	public void TriggerEvent(Event _event)
	{
		if (subscriptions.ContainsKey(_event))
		{
			int numLoops = subscriptions[_event].Count;
			for (int i = 0; i < numLoops; i++)
			{
				IEventSubscriber s = subscriptions[_event][i];
				bool isEventValid = true;
				if (s != null)
				{
					MonoBehaviour m = s as MonoBehaviour;

					if (s != null && m != null)
						s.InformOfEvent(_event);
					else
						isEventValid = false;
				}
				else
					isEventValid = false;

				if (!isEventValid)
				{
					subscriptions[_event].RemoveAt(i);

					i--;
					numLoops--;
				}
			}
		}
	}
}
