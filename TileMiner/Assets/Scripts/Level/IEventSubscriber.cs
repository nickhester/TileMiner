using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IEventSubscriber
{
	void InformOfEvent(EventBroadcast.Event _event);
}
