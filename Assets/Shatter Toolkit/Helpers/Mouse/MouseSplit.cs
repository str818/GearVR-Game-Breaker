// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class MouseSplit : MonoBehaviour
{
	public int raycastCount = 10;
	
	private bool started = false;
	private Vector3 start, end;
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			start = Input.mousePosition;
			
			started = true;
		}
		
		if (Input.GetMouseButtonUp(0) && started)
		{
			end = Input.mousePosition;
			
			// Calculate the world-space line
			Camera mainCamera = Camera.main;
			
			float near = mainCamera.near;
			
			Vector3 line = mainCamera.ScreenToWorldPoint(new Vector3(end.x, end.y, near)) - mainCamera.ScreenToWorldPoint(new Vector3(start.x, start.y, near));
			
			// Find game objects to split by raycasting at points along the line
			for (int i = 0; i < raycastCount; i++)
			{
				Ray ray = mainCamera.ScreenPointToRay(Vector3.Lerp(start, end, (float)i / raycastCount));
				
				RaycastHit hit;
				
				if (Physics.Raycast(ray, out hit))
				{
					Plane splitPlane = new Plane(Vector3.Normalize(Vector3.Cross(line, ray.direction)), hit.point);
					
					hit.collider.SendMessage("Split", new Plane[] { splitPlane }, SendMessageOptions.DontRequireReceiver);
				}
			}
			
			started = false;
		}
	}
}