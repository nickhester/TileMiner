using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defense : MonoBehaviour
{
	float targetCheckInterval = 1.0f;
	float targetCheckCounter = 0.0f;
	[SerializeField] private float range;

	float attackInterval = 2.0f;
	float attackCounter = 0.0f;

	Actor currentTarget;

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

	void AttackTarget()
	{
		Debug.DrawLine(transform.position, currentTarget.transform.position);

		Destroy(currentTarget.gameObject);
	}

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
