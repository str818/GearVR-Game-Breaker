// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class OutlineCreator : MonoBehaviour
{
	private List<Vector3> points = new List<Vector3>();
	private List<int> edges = new List<int>();
	private List<int> triangles = new List<int>();
	private List<int> triangleEdges = new List<int>();
	
	private bool isTriangulated = false;
	private bool isLoopClosed = false;
	private int loopStart = 0;
	
	public int LoopPointCount
	{
		get { return points.Count - loopStart; }
	}
	
	public void Reset()
	{
		points.Clear();
		edges.Clear();
		triangles.Clear();
		triangleEdges.Clear();
		
		isTriangulated = false;
		isLoopClosed = false;
		loopStart = 0;
	}
	
	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (isTriangulated)
			{
				Reset();
			}
			
			// Add the mouse world position to the list of points
			Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
			Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			
			points.Add(worldPosition);
			
			// Add an edge from the previous point to the new point
			if (LoopPointCount >= 2)
			{
				edges.Add(points.Count - 2);
				edges.Add(points.Count - 1);
			}
			
			// Set flag
			isLoopClosed = false;
		}
		else if (Input.GetMouseButtonDown(1))
		{
			if (LoopPointCount >= 3)
			{
				// Add the last edge to close the current loop
				edges.Add(points.Count - 1);
				edges.Add(loopStart);
				
				// Set flag
				isLoopClosed = true;
				
				// Start a new loop
				loopStart = points.Count;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (!isTriangulated && isLoopClosed)
			{
				// Triangulate
				int[] newEdges, newTriangles, newTriangleEdges;
				
				ITriangulator triangulator = new Triangulator(points, edges, Vector3.up);
				
				triangulator.Fill(out newEdges, out newTriangles, out newTriangleEdges);
				
				// Add the new edges and triangles
				edges.AddRange(newEdges);
				
				triangles.AddRange(newTriangles);
				
				triangleEdges.AddRange(newTriangleEdges);
				
				// Set flag
				isTriangulated = true;
			}
		}
	}
	
	public void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 500, 100),	"Please turn on Gizmos!\n" +
		        							"Create an outline by left-clicking in a clockwise order on the screen.\n" +
											"Right-click to close a loop.\n" +
											"Create a hole by left-clicking in a counter-clockwise order inside a shape.\n" +
											"Be careful not to overlap edges.\n" +
											"Press SPACE to triangulate the closed loops!");
	}
	
	public void OnDrawGizmos()
	{
		Gizmos.color = Color.black;
		
		// Draw points
		for (int i = 0; i < points.Count; i++)
		{
			Gizmos.DrawSphere(points[i], 0.1f);
		}
		
		// Draw edges
		for (int i = 0; i < edges.Count / 2; i++)
		{
			int edge = i * 2;
			
			Gizmos.DrawLine(points[edges[edge + 0]], points[edges[edge + 1]]);
		}
		
		// Draw triangles
		for (int i = 0; i < triangles.Count / 3; i++)
		{
			int triangle = i * 3;
			
			Vector3 center = (points[triangles[triangle + 0]] + points[triangles[triangle + 1]] + points[triangles[triangle + 2]]) / 3.0f;
			
			// Draw a red line from the triangle center to first triangle edge
			Gizmos.color = Color.red;
			
			Gizmos.DrawLine(center, (points[edges[triangleEdges[triangle + 0] * 2 + 0]] + points[edges[triangleEdges[triangle + 0] * 2 + 1]]) * 0.5f);
			
			// Draw a green line from the triangle center to the second triangle edge
			Gizmos.color = Color.green;
			
			Gizmos.DrawLine(center, (points[edges[triangleEdges[triangle + 1] * 2 + 0]] + points[edges[triangleEdges[triangle + 1] * 2 + 1]]) * 0.5f);
			
			// Draw a blue line from the triangle center to the third triangle edge
			Gizmos.color = Color.blue;
			
			Gizmos.DrawLine(center, (points[edges[triangleEdges[triangle + 2] * 2 + 0]] + points[edges[triangleEdges[triangle + 2] * 2 + 1]]) * 0.5f);
		}
	}
}