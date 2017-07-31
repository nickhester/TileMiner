using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Actor
{
	float movementPower = 10.0f;
	float initialImpulse = 0.01f;
	float wingFlapSpeed = 12.0f;
	float wingFlapRisingTendency = 0.5f;

	float sideMovementPhaseSpeed = 4.0f;
	float sideMovementPower = 0.05f;
	float sideMovementCurrentAmount = 1.0f;
	float sideMovementRange = 5.0f;
	float sideMovementChangeInterval = 6.0f;
	float sideMovementChangeCounter = 0.0f;

	protected override void Start()
	{
		base.Start();

		rigidBody.AddForce(new Vector2(Random.Range(-initialImpulse, initialImpulse), Random.Range(-initialImpulse, initialImpulse)), ForceMode2D.Impulse);
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();

		rigidBody.AddForce(Vector2.up * movementPower * Time.deltaTime
			* (Mathf.Sin(Time.time * wingFlapSpeed) + wingFlapRisingTendency)       // add varying upward force to simulate flapping wings
			+ (Vector2.right * sideMovementPower * Mathf.Sin(Time.time * sideMovementPhaseSpeed * sideMovementCurrentAmount)));      // add side-to-side movement

		// change side movement amount so it doesn't just go back and forth in the same place forever
		sideMovementChangeCounter += Time.deltaTime;
		if (sideMovementChangeCounter > sideMovementChangeInterval)
		{
			sideMovementChangeCounter = 0.0f;
			sideMovementCurrentAmount = Random.Range(-sideMovementRange, sideMovementRange);
		}
	}

}
