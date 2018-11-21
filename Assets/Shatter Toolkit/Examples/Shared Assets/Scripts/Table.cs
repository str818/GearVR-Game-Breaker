// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
	public void PreSplit(Plane[] planes)
	{
		// Disconnect the legs from the table
		if (transform.childCount > 0)
		{
			foreach (Transform child in transform)
			{
				child.gameObject.AddComponent<Rigidbody>();
			}
			
			transform.DetachChildren();
		}
	}
}