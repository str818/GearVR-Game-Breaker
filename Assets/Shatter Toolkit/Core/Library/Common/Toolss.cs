// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public static class Toolss
{
	public static bool IsPointInsideTriangle(ref Vector3 point, ref Vector3 triangle0, ref Vector3 triangle1, ref Vector3 triangle2)
	{
		Vector3 normal = Vector3.Cross(triangle1 - triangle0, triangle2 - triangle0);
		
		return IsPointInsideTriangle(ref point, ref triangle0, ref triangle1, ref triangle2, ref normal);
	}
	
	public static bool IsPointInsideTriangle(ref Vector3 point, ref Vector3 triangle0, ref Vector3 triangle1, ref Vector3 triangle2, ref Vector3 triangleNormal)
	{
		// Discard zero-size triangles; slower but more logical than considering the triangle edges as outside
		if (Vector3.Cross(triangle1 - triangle0, triangle2 - triangle0) == Vector3.zero)
		{
			return false;
		}
		
		Vector3 pointTo0 = triangle0 - point;
		Vector3 pointTo1 = triangle1 - point;
		Vector3 pointTo2 = triangle2 - point;
		
		if (Vector3.Dot(Vector3.Cross(pointTo0, pointTo1), triangleNormal) < 0.0f ||
		    Vector3.Dot(Vector3.Cross(pointTo1, pointTo2), triangleNormal) < 0.0f ||
		    Vector3.Dot(Vector3.Cross(pointTo2, pointTo0), triangleNormal) < 0.0f)
		{
			return false;
		}
		
		return true;
	}
}