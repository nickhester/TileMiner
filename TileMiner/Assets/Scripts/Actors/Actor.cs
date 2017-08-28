using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	protected Rigidbody2D rigidBody;
	protected PathManager pathManager;
	protected City city;

	public int maxHealth = 1;
	protected int health = 1;

	virtual protected void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		pathManager = FindObjectOfType<PathManager>();
		health = maxHealth;
	}

	virtual protected void Update() { }

	virtual protected void FixedUpdate() { }

	protected void ReportHitCity(int damage)
	{
		City.Instance.HitCity(damage);
	}

	public void GetHit(int damage)
	{
		health -= damage;

		if (health <= 0)
			Destroy(gameObject);
	}
}
