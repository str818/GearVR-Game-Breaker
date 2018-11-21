// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class TableLeg : MonoBehaviour
{
	public void PreSplit(Plane[] planes)
	{
		// Disconnect this leg from the table
		if (transform.parent != null)
		{
			transform.parent = null;
			
			gameObject.AddComponent<Rigidbody>();
		}
	}
}