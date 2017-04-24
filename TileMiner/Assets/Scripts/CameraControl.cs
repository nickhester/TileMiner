using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
	[SerializeField] private float cameraScrollAmount = 0.1f;
	private EventBroadcast eventBroadcast;

	// camera dragging
	private bool isDragging = false;
	private bool isMouseDown = false;
	private Vector3 anchoredPointOnScreen;
	private Vector3 anchoredPointInSpace;
	private float timeSinceDraggingEnded = 0.0f;
	[SerializeField] private float timeToDrag = 0.1f;
	private float timeToDragCounter = 0.0f;
	[SerializeField] private float distanceToDrag = 0.1f;
	private float distanceToDragCounter = 0.0f;

	void Start ()
	{
		eventBroadcast = GameObject.FindObjectOfType<EventBroadcast>();
	}
	
	void Update ()
	{
		if (Input.GetMouseButtonDown(0))
		{
			timeToDragCounter = 0.0f;
			distanceToDragCounter = 0.0f;
			anchoredPointOnScreen = Input.mousePosition;
			isMouseDown = true;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			isDragging = false;
			isMouseDown = false;
		}
		else
		{
			if (isDragging)
			{
				// move the camera so the point in space is under the mouse
				Vector3 actualPointInSpace = GetPointerPositionOnPlayPlane();
				float difference = actualPointInSpace.y - anchoredPointInSpace.y;
				// this simple method works as long as the camera is orthographic. If it's changed to perspective later, it'll have to be scaled for perspective somehow.
				transform.Translate(0.0f, -difference, 0.0f);
			}
			else if (isMouseDown)
			{
				bool readyToStartDragging = false;

				// check if has been held for long enough to be considered a drag
				timeToDragCounter += Time.deltaTime;
				if (timeToDragCounter >= timeToDrag)
				{
					readyToStartDragging = true;
				}

				// check if has dragged far enough to be considered a drag
				if (Vector3.Distance(anchoredPointOnScreen, Input.mousePosition) > distanceToDrag)
				{
					readyToStartDragging = true;
				}

				if (readyToStartDragging)
				{
					anchoredPointInSpace = GetPointerPositionOnPlayPlane();
					isDragging = true;
					timeSinceDraggingEnded = 0.0f;
				}
			}
			else
			{
				timeSinceDraggingEnded += Time.deltaTime;
			}
			
		}


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

	public void GetDraggingStatus(ref bool isDragging, ref float timeSinceDraggingEnded)
	{
		isDragging = this.isDragging;
		timeSinceDraggingEnded = this.timeSinceDraggingEnded;
	}

	Vector3 GetPointerPositionOnPlayPlane()
	{
		Ray pointerRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane playPlane = new Plane(Vector3.back, 0.0f);
		float intersectDistance = 0.0f;
		playPlane.Raycast(pointerRay, out intersectDistance);
		return pointerRay.GetPoint(intersectDistance);
	}
}
