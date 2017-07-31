using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
	protected Rigidbody2D rigidBody;

	virtual protected void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
	}

	virtual protected void Update() { }

	virtual protected void FixedUpdate() { }
}
