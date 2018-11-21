// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class MouseInstantiate : MonoBehaviour
{
	public GameObject prefabToInstantiate;
	
	public float speed = 7.0f;
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0) && prefabToInstantiate != null)
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			GameObject newGameObject = (GameObject)Instantiate(prefabToInstantiate, mouseRay.origin, Quaternion.identity);
			
			if (newGameObject.GetComponent<Rigidbody>() != null)
			{
				newGameObject.GetComponent<Rigidbody>().velocity = mouseRay.direction * speed;
			}
		}
	}
}