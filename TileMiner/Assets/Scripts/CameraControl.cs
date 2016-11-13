using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
	[SerializeField]private float cameraScrollAmount = 0.1f;
	
	void Start ()
	{
		
	}
	
	void Update ()
	{
		float d = Input.GetAxis("Mouse ScrollWheel");
		if (d > 0f)
		{
			transform.Translate(0.0f, cameraScrollAmount, 0.0f);
		}
		else if (d < 0f)
		{
			transform.Translate(0.0f, -cameraScrollAmount, 0.0f);
		}
	}
}
