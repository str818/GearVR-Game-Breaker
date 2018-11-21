// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class Edge
{
	public int index;
	public Point point0, point1;
	public Vector3 line;
	
	public Edge(Point point0, Point point1)
	{
		this.point0 = point0;
		this.point1 = point1;
		this.line = point1.position - point0.position;
	}
}