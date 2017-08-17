using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	float speed = 2.2f;

	void FixedUpdate()
	{
		transform.Translate(Vector2.right * Time.deltaTime * speed, Space.Self);
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		Tile hitTile = collision.gameObject.GetComponent<Tile>();
		if (hitTile != null || collision.gameObject.tag == "LevelEdge")
		{
			Destroy(gameObject);
		}
		else
		{
			Actor hitActor = collision.gameObject.GetComponent<Actor>();
			if (hitActor != null)
			{
				hitActor.GetHit(1);
				Destroy(gameObject);
			}
		}
	}
}