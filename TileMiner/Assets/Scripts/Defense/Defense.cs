using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Defense : MonoBehaviour
{
	protected float targetCheckInterval = 0.5f;
	protected float targetCheckCounter = 0.0f;
	[SerializeField] protected float range = 2.0f;

	[SerializeField] protected float attackInterval = 2.0f;
	protected float attackCounter = 0.0f;

	protected Actor currentTarget;

	void Update ()
	{
		// check for targets
		targetCheckCounter += Time.deltaTime;
		if (targetCheckCounter > targetCheckInterval)
		{
			targetCheckCounter = 0.0f;
			currentTarget = FindTarget();
		}

		// attack
		attackCounter += Time.deltaTime;
		if (attackCounter > attackInterval)
		{
			attackCounter = 0.0f;
			if (currentTarget != null)
				AttackTarget();
		}
	}

	protected virtual void AttackTarget() { }

	Actor FindTarget()
	{
		Actor[] actors = FindObjectsOfType<Actor>();
		foreach (var anActor in actors)
		{
			if (Vector2.Distance(anActor.transform.position, transform.position) < range)
			{
				return anActor;
			}
		}
		return null;
	}
}
