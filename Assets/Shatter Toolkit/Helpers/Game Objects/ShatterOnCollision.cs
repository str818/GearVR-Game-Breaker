// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class ShatterOnCollision : MonoBehaviour
{
	public float requiredForce = 1.0f;
	
	public float cooldownTime = 0.5f;
	
	private float timeSinceInstantiated = 0.0f;
	
	public void Update()
	{
		timeSinceInstantiated += Time.deltaTime;
	}
	
	public void OnCollisionEnter(Collision collision)
	{
		if (timeSinceInstantiated >= cooldownTime)
		{
			if (collision.impactForceSum.magnitude >= requiredForce)
			{
				// Find the new contact point
				foreach (ContactPoint contact in collision.contacts)
				{
					if (contact.otherCollider == collision.collider)
					{
						// Shatter at this contact point
						SendMessage("Shatter", contact.point, SendMessageOptions.RequireReceiver);
						
						break;
					}
				}
			}
		}
	}
}