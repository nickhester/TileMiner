using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hawk : Actor
{
	float pathTargetWeight = 1.0f;
	
	[SerializeField] float speed = 1.0f;
	
	int damage = 3;

	protected override void Start()
	{
		base.Start();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		
		// apply movement toward path steps
		Vector2 pathTarget = Vector2.zero;
		if (pathManager.GetPathTargetPosition(transform.position, ref pathTarget))
		{
			Debug.DrawLine(transform.position, pathTarget);

			transform.Translate((pathTarget - (Vector2)transform.position) * Time.deltaTime * speed);
		}
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "City")
		{
			ReportHitCity(damage);
			Destroy(this.gameObject);
		}
	}
}
