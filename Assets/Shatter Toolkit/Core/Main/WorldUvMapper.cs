// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class WorldUvMapper : UvMapper
{
	/// <summary>
	/// Determines the scale to be applied to the uv-coordinates. (1,1) will repeat the texture once for every local-space unit, (2,2) twice and so on.
	/// </summary>
	public Vector2 scale = Vector2.one;
	
	public override void Map(IList<Vector3> points, Vector3 planeNormal, out Vector4[] tangentsA, out Vector4[] tangentsB, out Vector2[] uvsA, out Vector2[] uvsB)
	{
		// Calculate texture direction vectors
		Vector3 u = Vector3.Cross(planeNormal, Vector3.up);
		
		if (u == Vector3.zero)
		{
			u = Vector3.Cross(planeNormal, Vector3.forward);
		}
		
		Vector3 v = Vector3.Cross(u, planeNormal);
		
		u.Normalize();
		v.Normalize();
		
		// Set tangents
		Vector4 tangentA = new Vector4(u.x, u.y, u.z, 1.0f);
		Vector4 tangentB = new Vector4(u.x, u.y, u.z, -1.0f);
		
		tangentsA = new Vector4[points.Count];
		tangentsB = new Vector4[points.Count];
		
		for (int i = 0; i < points.Count; i++)
		{
			tangentsA[i] = tangentA;
			tangentsB[i] = tangentB;
		}
		
		// Set uvs
		Vector2[] uvs = new Vector2[points.Count];
		
		Vector2 min = Vector2.zero;
		
		for (int i = 0; i < points.Count; i++)
		{
			Vector3 point = points[i];
			
			uvs[i].x = Vector3.Dot(point, u);
			uvs[i].y = Vector3.Dot(point, v);
			
			if (i == 0)
			{
				min = uvs[i];
			}
			else
			{
				min = Vector2.Min(uvs[i], min);
			}
		}
		
		for (int i = 0; i < points.Count; i++)
		{
			uvs[i] -= min;
			
			uvs[i].x *= scale.x;
			uvs[i].y *= scale.y;
		}
		
		uvsA = uvs;
		uvsB = uvs;
	}
}