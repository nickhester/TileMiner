using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadShot : Defense
{
	[SerializeField] private Projectile projectilePrefab;
	float spreadAmount = 15f;

	protected override void AttackTarget()
	{
		Debug.DrawLine(transform.position, currentTarget.transform.position);

		// shoot 3 projectile spread
		Vector2 vector = currentTarget.transform.position - transform.position;
		vector.Normalize();
		float rotZ = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;

		Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rotZ));
		Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rotZ + spreadAmount));
		Instantiate(projectilePrefab, transform.position, Quaternion.Euler(0, 0, rotZ - spreadAmount));
	}
}
