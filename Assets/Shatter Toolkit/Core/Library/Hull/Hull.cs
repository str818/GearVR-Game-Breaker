// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

public class Hull
{
	private static float smallestValidLength = 0.01f;
	private static float smallestValidRatio = 0.05f;
	
	private Object key = new Object();
	
	private IList<Vector3> vertices;
	private IList<Vector3> normals;
	private IList<Vector4> tangents;
	private IList<Vector2> uvs;
	
	private IList<Point> vertexToPointMap;
	
	private IList<Point> points;
	private IList<Edge> edges;
	private IList<Triangle> triangles;
	
	public Hull(Hull reference)
	{
		int vertexCount = reference.vertices.Count * 2;
		
		vertices = new List<Vector3>(vertexCount);
		normals = new List<Vector3>(vertexCount);
		tangents = new List<Vector4>(vertexCount);
		uvs = new List<Vector2>(vertexCount);
		
		vertexToPointMap = new List<Point>(vertexCount);
		
		points = new List<Point>(reference.points.Count * 2);
		edges = new List<Edge>(reference.edges.Count * 2);
		triangles = new List<Triangle>(reference.triangles.Count * 2);
	}
	
	public Hull(Mesh mesh)
	{
		// Initialize lists
		vertices = new List<Vector3>(mesh.vertices);
		normals = new List<Vector3>(mesh.normals);
		tangents = new List<Vector4>(mesh.tangents);
		uvs = new List<Vector2>(mesh.uv);
		
		vertexToPointMap = new List<Point>(vertices.Count);
		
		points = new List<Point>();
		edges = new List<Edge>();
		triangles = new List<Triangle>();
		
		// Add points
		for (int i = 0; i < vertices.Count; i++)
		{
			Point uniquePoint;
			
			AddUniquePoint(vertices[i], out uniquePoint);
			
			vertexToPointMap.Add(uniquePoint);
		}
		
		// Add edges and triangles
		int[] indices = mesh.triangles;
		
		for (int i = 0; i < indices.Length / 3; i++)
		{
			int triangle = i * 3;
			
			AddTriangle(indices[triangle + 0], indices[triangle + 1], indices[triangle + 2]);
		}
	}
	
	private void AddUniquePoint(Vector3 position, out Point uniquePoint)
	{
		// Is there already a point with this position?
		foreach (Point point in points)
		{
			if (point.position == position)
			{
				uniquePoint = point;
				
				return;
			}
		}
		
		// No existing point was found, create a new one
		uniquePoint = new Point(position);
		
		points.Add(uniquePoint);
	}
	
	private void AddUniqueEdge(Point point0, Point point1, out Edge uniqueEdge)
	{
		// Is there already an edge between these points?
		foreach (Edge edge in edges)
		{
			if ((edge.point0 == point0 && edge.point1 == point1) ||
			    (edge.point0 == point1 && edge.point1 == point0))
			{
				uniqueEdge = edge;
				
				return;
			}
		}
		
		// No existing edge was found, create a new one
		uniqueEdge = new Edge(point0, point1);
		
		edges.Add(uniqueEdge);
	}
	
	private void AddTriangle(int vertex0, int vertex1, int vertex2)
	{
		Point point0 = vertexToPointMap[vertex0];
		Point point1 = vertexToPointMap[vertex1];
		Point point2 = vertexToPointMap[vertex2];
		
		Edge edge0, edge1, edge2;
		
		AddUniqueEdge(point0, point1, out edge0);
		AddUniqueEdge(point1, point2, out edge1);
		AddUniqueEdge(point2, point0, out edge2);
		
		Triangle triangle = new Triangle(vertex0, vertex1, vertex2, point0, point1, point2, edge0, edge1, edge2);
		
		triangles.Add(triangle);
	}
	
	private void AddVertex(Vector3 vertex, Vector3 normal, Vector4 tangent, Vector2 uv, Point point, out int index)
	{
		index = vertices.Count;
		
		vertices.Add(vertex);
		normals.Add(normal);
		tangents.Add(tangent);
		uvs.Add(uv);
		
		vertexToPointMap.Add(point);
	}
	
	public bool IsEmpty
	{
		get
		{
			lock (key)
			{
				return points.Count < 4 || edges.Count < 6 || triangles.Count < 4;
			}
		}
	}
	
	public void Clear()
	{
		lock (key)
		{
			vertices.Clear();
			normals.Clear();
			tangents.Clear();
			uvs.Clear();
			
			vertexToPointMap.Clear();
			
			points.Clear();
			edges.Clear();
			triangles.Clear();
		}
	}
	
	public Mesh GetMesh()
	{
		lock (key)
		{
			if (!IsEmpty)
			{
				// Create vertex array
				Vector3[] vertices = new Vector3[this.vertices.Count];
				Vector3[] normals = new Vector3[this.normals.Count];
				Vector4[] tangents = new Vector4[this.tangents.Count];
				Vector2[] uvs = new Vector2[this.uvs.Count];
				
				this.vertices.CopyTo(vertices, 0);
				this.normals.CopyTo(normals, 0);
				this.tangents.CopyTo(tangents, 0);
				this.uvs.CopyTo(uvs, 0);
				
				// Create index array
				int[] indices = new int[triangles.Count * 3];
				
				int count = 0;
				
				foreach (Triangle triangle in triangles)
				{
					indices[count++] = triangle.vertex0;
					indices[count++] = triangle.vertex1;
					indices[count++] = triangle.vertex2;
				}
				
				// Create output mesh
				Mesh mesh = new Mesh();
				
				mesh.vertices = vertices;
				mesh.normals = normals;
				mesh.tangents = tangents;
				mesh.uv = uvs;
				mesh.triangles = indices;
				
				return mesh;
			}
			
			return null;
		}
	}
	
	public void Split(Vector3 localPointOnPlane, Vector3 localPlaneNormal, bool fillCut, UvMapper uvMapper, out Hull a, out Hull b)
	{
		lock (key)
		{
			if (localPlaneNormal == Vector3.zero)
			{
				localPlaneNormal = Vector3.up;
			}
			
			a = new Hull(this);
			b = new Hull(this);
			
			SetIndices();
			
			bool[] pointAbovePlane;
			
			AssignPoints(a, b, localPointOnPlane, localPlaneNormal, out pointAbovePlane);
			
			int[] oldToNewVertex;
			
			AssignVertices(a, b, pointAbovePlane, out oldToNewVertex);
			
			bool[] edgeIntersectsPlane;
			EdgeHit[] edgeHits;
			
			AssignEdges(a, b, pointAbovePlane, localPointOnPlane, localPlaneNormal, out edgeIntersectsPlane, out edgeHits);
			
			IList<Edge> cutEdgesA, cutEdgesB;
			
			AssignTriangles(a, b, pointAbovePlane, edgeIntersectsPlane, edgeHits, oldToNewVertex, out cutEdgesA, out cutEdgesB);
			
			if (fillCut)
			{
				SortCutEdges(cutEdgesA, cutEdgesB);
				
				FillCutEdges(a, b, cutEdgesA, cutEdgesB, localPlaneNormal, uvMapper);
			}
			
			ValidateOutput(a, b, localPlaneNormal);
			
			Clear();
		}
	}
	
	private void SetIndices()
	{
		int pointCount = 0;
		
		foreach (Point point in points)
		{
			point.index = pointCount++;
		}
		
		int edgeCount = 0;
		
		foreach (Edge edge in edges)
		{
			edge.index = edgeCount++;
		}
	}
	
	private void AssignPoints(Hull a, Hull b, Vector3 pointOnPlane, Vector3 planeNormal, out bool[] pointAbovePlane)
	{
		pointAbovePlane = new bool[points.Count];
		
		foreach (Point point in points)
		{
			bool abovePlane = Vector3.Dot(point.position - pointOnPlane, planeNormal) >= 0.0f;
			
			pointAbovePlane[point.index] = abovePlane;
			
			if (abovePlane)
			{
				// Assign this point to hull A
				a.points.Add(point);
			}
			else
			{
				// Assign this point to hull B
				b.points.Add(point);
			}
		}
	}
	
	private void AssignVertices(Hull a, Hull b, bool[] pointAbovePlane, out int[] oldToNewVertex)
	{
		oldToNewVertex = new int[vertices.Count];
		
		for (int i = 0; i < vertices.Count; i++)
		{
			Point correspondingPoint = vertexToPointMap[i];
			
			if (pointAbovePlane[correspondingPoint.index])
			{
				// Assign this vertex to hull A
				a.AddVertex(vertices[i], normals[i], tangents[i], uvs[i], correspondingPoint, out oldToNewVertex[i]);
			}
			else
			{
				// Assign this vertex to hull B
				b.AddVertex(vertices[i], normals[i], tangents[i], uvs[i], correspondingPoint, out oldToNewVertex[i]);
			}
		}
	}
	
	private void AssignEdges(Hull a, Hull b, bool[] pointAbovePlane, Vector3 pointOnPlane, Vector3 planeNormal, out bool[] edgeIntersectsPlane, out EdgeHit[] edgeHits)
	{
		edgeIntersectsPlane = new bool[edges.Count];
		edgeHits = new EdgeHit[edges.Count];
		
		foreach (Edge edge in edges)
		{
			bool abovePlane0 = pointAbovePlane[edge.point0.index];
			bool abovePlane1 = pointAbovePlane[edge.point1.index];
			
			if (abovePlane0 && abovePlane1)
			{
				// Assign this edge to hull A
				a.edges.Add(edge);
			}
			else if (!abovePlane0 && !abovePlane1)
			{
				// Assign this edge to hull B
				b.edges.Add(edge);
			}
			else
			{
				// Split edge
				float denominator = Vector3.Dot(edge.line, planeNormal);
				float scalar = Mathf.Clamp01(Vector3.Dot(pointOnPlane - edge.point0.position, planeNormal) / denominator);
				
				Vector3 intersection = edge.point0.position + edge.line * scalar;
				
				// Create new points
				Point pointA = new Point(intersection);
				Point pointB = new Point(intersection);
				
				a.points.Add(pointA);
				b.points.Add(pointB);
				
				// Create new edges
				Edge splitA, splitB;
				
				if (pointAbovePlane[edge.point0.index])
				{
					splitA = new Edge(pointA, edge.point0);
					splitB = new Edge(pointB, edge.point1);
				}
				else
				{
					splitA = new Edge(pointA, edge.point1);
					splitB = new Edge(pointB, edge.point0);
				}
				
				a.edges.Add(splitA);
				b.edges.Add(splitB);
				
				// Set flags
				edgeIntersectsPlane[edge.index] = true;
				
				edgeHits[edge.index] = new EdgeHit();
				edgeHits[edge.index].scalar = scalar;
				edgeHits[edge.index].splitA = splitA;
				edgeHits[edge.index].splitB = splitB;
			}
		}
	}
	
	private void AssignTriangles(Hull a, Hull b, bool[] pointAbovePlane, bool[] edgeIntersectsPlane, EdgeHit[] edgeHits, int[] oldToNewVertex, out IList<Edge> cutEdgesA, out IList<Edge> cutEdgesB)
	{
		cutEdgesA = new List<Edge>();
		cutEdgesB = new List<Edge>();
		
		foreach (Triangle triangle in triangles)
		{
			bool abovePlane0 = pointAbovePlane[triangle.point0.index];
			bool abovePlane1 = pointAbovePlane[triangle.point1.index];
			bool abovePlane2 = pointAbovePlane[triangle.point2.index];
			
			if (abovePlane0 && abovePlane1 && abovePlane2)
			{
				// Assign this triangle to hull A
				triangle.vertex0 = oldToNewVertex[triangle.vertex0];
				triangle.vertex1 = oldToNewVertex[triangle.vertex1];
				triangle.vertex2 = oldToNewVertex[triangle.vertex2];
				
				a.triangles.Add(triangle);
			}
			else if (!abovePlane0 && !abovePlane1 && !abovePlane2)
			{
				// Assign this triangle to hull B
				triangle.vertex0 = oldToNewVertex[triangle.vertex0];
				triangle.vertex1 = oldToNewVertex[triangle.vertex1];
				triangle.vertex2 = oldToNewVertex[triangle.vertex2];
				
				b.triangles.Add(triangle);
			}
			else
			{
				// Split triangle
				Point topPoint;
				Edge edge0, edge1, edge2;
				int vertex0, vertex1, vertex2;
				
				if (edgeIntersectsPlane[triangle.edge0.index] && edgeIntersectsPlane[triangle.edge1.index])
				{
					topPoint = triangle.point1;
					edge0 = triangle.edge0;
					edge1 = triangle.edge1;
					edge2 = triangle.edge2;
					vertex0 = triangle.vertex0;
					vertex1 = triangle.vertex1;
					vertex2 = triangle.vertex2;
				}
				else if (edgeIntersectsPlane[triangle.edge1.index] && edgeIntersectsPlane[triangle.edge2.index])
				{
					topPoint = triangle.point2;
					edge0 = triangle.edge1;
					edge1 = triangle.edge2;
					edge2 = triangle.edge0;
					vertex0 = triangle.vertex1;
					vertex1 = triangle.vertex2;
					vertex2 = triangle.vertex0;
				}
				else
				{
					topPoint = triangle.point0;
					edge0 = triangle.edge2;
					edge1 = triangle.edge0;
					edge2 = triangle.edge1;
					vertex0 = triangle.vertex2;
					vertex1 = triangle.vertex0;
					vertex2 = triangle.vertex1;
				}
				
				EdgeHit edgeHit0 = edgeHits[edge0.index];
				EdgeHit edgeHit1 = edgeHits[edge1.index];
				
				// Convert edge hit scalars to the triangle winding order
				float scalar0 = topPoint == edge0.point1 ? edgeHit0.scalar : 1.0f - edgeHit0.scalar;
				float scalar1 = topPoint == edge1.point0 ? edgeHit1.scalar : 1.0f - edgeHit1.scalar;
				
				Edge cutEdgeA, cutEdgeB;
				
				if (pointAbovePlane[topPoint.index])
				{
					// Assign top triangle to hull A, bottom triangle to hull B
					cutEdgeA = new Edge(edgeHit1.splitA.point0, edgeHit0.splitA.point0);
					cutEdgeB = new Edge(edgeHit1.splitB.point0, edgeHit0.splitB.point0);
					
					a.edges.Add(cutEdgeA);
					b.edges.Add(cutEdgeB);
					
					SplitTriangle(a, b, edgeHit0.splitA, edgeHit1.splitA, cutEdgeA, edgeHit0.splitB, edgeHit1.splitB, cutEdgeB, edge2, vertex0, vertex1, vertex2, scalar0, scalar1, oldToNewVertex);
				}
				else
				{
					// Assign top triangle to hull B, bottom triangle to hull A
					cutEdgeA = new Edge(edgeHit0.splitA.point0, edgeHit1.splitA.point0);
					cutEdgeB = new Edge(edgeHit0.splitB.point0, edgeHit1.splitB.point0);
					
					a.edges.Add(cutEdgeA);
					b.edges.Add(cutEdgeB);
					
					SplitTriangle(b, a, edgeHit0.splitB, edgeHit1.splitB, cutEdgeB, edgeHit0.splitA, edgeHit1.splitA, cutEdgeA, edge2, vertex0, vertex1, vertex2, scalar0, scalar1, oldToNewVertex);
				}
				
				cutEdgesA.Add(cutEdgeA);
				cutEdgesB.Add(cutEdgeB);
			}
		}
	}
	
	private void SplitTriangle(Hull topHull, Hull bottomHull, Edge topEdge0, Edge topEdge1, Edge topCutEdge, Edge bottomEdge0, Edge bottomEdge1, Edge bottomCutEdge, Edge bottomEdge2, int vertex0, int vertex1, int vertex2, float scalar0, float scalar1, int[] oldToNewVertex)
	{
		Vector3 n0 = normals[vertex0];
		Vector3 n1 = normals[vertex1];
		Vector3 n2 = normals[vertex2];
		
		Vector4 t0 = tangents[vertex0];
		Vector4 t1 = tangents[vertex1];
		Vector4 t2 = tangents[vertex2];
		
		Vector2 uv0 = uvs[vertex0];
		Vector2 uv1 = uvs[vertex1];
		Vector2 uv2 = uvs[vertex2];
		
		// Calculate the cut vertex data by interpolating original triangle values
		Vector3 cutNormal0 = new Vector3();
		
		cutNormal0.x = n0.x + (n1.x - n0.x) * scalar0;
		cutNormal0.y = n0.y + (n1.y - n0.y) * scalar0;
		cutNormal0.z = n0.z + (n1.z - n0.z) * scalar0;
		
		cutNormal0.Normalize();
		
		Vector3 cutNormal1 = new Vector3();
		
		cutNormal1.x = n1.x + (n2.x - n1.x) * scalar1;
		cutNormal1.y = n1.y + (n2.y - n1.y) * scalar1;
		cutNormal1.z = n1.z + (n2.z - n1.z) * scalar1;
		
		cutNormal1.Normalize();
		
		Vector4 cutTangent0 = new Vector4();
		
		cutTangent0.x = t0.x + (t1.x - t0.x) * scalar0;
		cutTangent0.y = t0.y + (t1.y - t0.y) * scalar0;
		cutTangent0.z = t0.z + (t1.z - t0.z) * scalar0;
		
		cutTangent0.Normalize();
		cutTangent0.w = t0.w;
		
		Vector4 cutTangent1 = new Vector4();
		
		cutTangent1.x = t1.x + (t2.x - t1.x) * scalar1;
		cutTangent1.y = t1.y + (t2.y - t1.y) * scalar1;
		cutTangent1.z = t1.z + (t2.z - t1.z) * scalar1;
		
		cutTangent1.Normalize();
		cutTangent1.w = t1.w;
		
		Vector2 cutUv0 = new Vector2();
		
		cutUv0.x = uv0.x + (uv1.x - uv0.x) * scalar0;
		cutUv0.y = uv0.y + (uv1.y - uv0.y) * scalar0;
		
		Vector2 cutUv1 = new Vector2();
		
		cutUv1.x = uv1.x + (uv2.x - uv1.x) * scalar1;
		cutUv1.y = uv1.y + (uv2.y - uv1.y) * scalar1;
		
		// Add the cut vertices to the hulls
		int topCutVertex0, topCutVertex1;
		
		topHull.AddVertex(topEdge0.point0.position, cutNormal0, cutTangent0, cutUv0, topEdge0.point0, out topCutVertex0);
		
		topHull.AddVertex(topEdge1.point0.position, cutNormal1, cutTangent1, cutUv1, topEdge1.point0, out topCutVertex1);
		
		int bottomCutVertex0, bottomCutVertex1;
		
		bottomHull.AddVertex(bottomEdge0.point0.position, cutNormal0, cutTangent0, cutUv0, bottomEdge0.point0, out bottomCutVertex0);
		
		bottomHull.AddVertex(bottomEdge1.point0.position, cutNormal1, cutTangent1, cutUv1, bottomEdge1.point0, out bottomCutVertex1);
		
		// Create the top of the original triangle
		Triangle topTriangle = new Triangle(topCutVertex0, oldToNewVertex[vertex1], topCutVertex1, topEdge0.point0, topEdge0.point1, topEdge1.point0, topEdge0, topEdge1, topCutEdge);
		
		topHull.triangles.Add(topTriangle);
		
		// Create the bottom of the original triangle
		Edge bottomCrossEdge = new Edge(bottomEdge0.point1, bottomEdge1.point0);
		
		Triangle bottomTriangle0 = new Triangle(oldToNewVertex[vertex0], bottomCutVertex0, bottomCutVertex1, bottomEdge0.point1, bottomEdge0.point0, bottomEdge1.point0, bottomEdge0, bottomCutEdge, bottomCrossEdge);
		Triangle bottomTriangle1 = new Triangle(oldToNewVertex[vertex0], bottomCutVertex1, oldToNewVertex[vertex2], bottomEdge0.point1, bottomEdge1.point0, bottomEdge1.point1, bottomCrossEdge, bottomEdge1, bottomEdge2);
		
		bottomHull.edges.Add(bottomCrossEdge);
		bottomHull.triangles.Add(bottomTriangle0);
		bottomHull.triangles.Add(bottomTriangle1);
	}
	
	private void SortCutEdges(IList<Edge> edgesA, IList<Edge> edgesB)
	{
		Edge start = null;
		
		for (int i = 0; i < edgesA.Count; i++)
		{
			if (start == null)
			{
				start = edgesA[i];
			}
			else
			{
				Edge previous = edgesA[i - 1];
				
				for (int j = i; j < edgesA.Count; j++)
				{
					Edge edgeA = edgesA[j];
					
					// Does this edge continue the previous one?
					if (previous.point1 == edgeA.point0)
					{
						// Swap edges
						Edge currentEdgeA = edgesA[i];
						
						edgesA[i] = edgeA;
						edgesA[j] = currentEdgeA;
						
						Edge currentEdgeB = edgesB[i];
						
						edgesB[i] = edgesB[j];
						edgesB[j] = currentEdgeB;
						
						// Does this edge end the current loop?
						if (edgeA.point1 == start.point0)
						{
							start = null;
						}
						
						break;
					}
				}
			}
		}
	}
	
	private void FillCutEdges(Hull a, Hull b, IList<Edge> edgesA, IList<Edge> edgesB, Vector3 planeNormal, UvMapper uvMapper)
	{
		// Create outline data
		int outlineEdgeCount = edgesA.Count;
		
		Vector3[] outlinePoints = new Vector3[outlineEdgeCount];
		int[] outlineEdges = new int[outlineEdgeCount * 2];
		
		int startIndex = 0;
		
		for (int i = 0; i < outlineEdgeCount; i++)
		{
			int currentIndex = i;
			int nextIndex = (i + 1) % outlineEdgeCount;
			
			Edge current = edgesA[currentIndex];
			Edge next = edgesA[nextIndex];
			
			// Set point
			outlinePoints[i] = current.point0.position;
			
			// Set edge
			outlineEdges[i * 2 + 0] = currentIndex;
			
			if (current.point1 == next.point0)
			{
				outlineEdges[i * 2 + 1] = nextIndex;
			}
			else
			{
				outlineEdges[i * 2 + 1] = startIndex;
				
				startIndex = nextIndex;
			}
		}
		
		// Triangulate
		int[] newEdges, newTriangles, newTriangleEdges;
		
		ITriangulator triangulator = new Triangulator(outlinePoints, outlineEdges, planeNormal);
		
		triangulator.Fill(out newEdges, out newTriangles, out newTriangleEdges);
		
		// Calculate vertex properties
		Vector3 normalA = -planeNormal;
		Vector3 normalB = planeNormal;
		Vector4[] tangentsA, tangentsB;
		Vector2[] uvsA, uvsB;
		
		uvMapper.Map(outlinePoints, planeNormal, out tangentsA, out tangentsB, out uvsA, out uvsB);
		
		// Create new vertices
		int[] verticesA = new int[outlineEdgeCount];
		int[] verticesB = new int[outlineEdgeCount];
		
		for (int i = 0; i < outlineEdgeCount; i++)
		{
			a.AddVertex(outlinePoints[i], normalA, tangentsA[i], uvsA[i], edgesA[i].point0, out verticesA[i]);
			
			b.AddVertex(outlinePoints[i], normalB, tangentsB[i], uvsB[i], edgesB[i].point0, out verticesB[i]);
		}
		
		// Create new edges
		for (int i = 0; i < newEdges.Length / 2; i++)
		{
			int point0 = newEdges[i * 2 + 0];
			int point1 = newEdges[i * 2 + 1];
			
			Edge edgeA = new Edge(edgesA[point0].point0, edgesA[point1].point0);
			Edge edgeB = new Edge(edgesB[point0].point0, edgesB[point1].point0);
			
			edgesA.Add(edgeA);
			edgesB.Add(edgeB);
			
			a.edges.Add(edgeA);
			b.edges.Add(edgeB);
		}
		
		// Create new triangles
		for (int i = 0; i < newTriangles.Length / 3; i++)
		{
			int point0 = newTriangles[i * 3 + 0];
			int point1 = newTriangles[i * 3 + 1];
			int point2 = newTriangles[i * 3 + 2];
			
			int edge0 = newTriangleEdges[i * 3 + 0];
			int edge1 = newTriangleEdges[i * 3 + 1];
			int edge2 = newTriangleEdges[i * 3 + 2];
			
			Triangle triangleA = new Triangle(verticesA[point0], verticesA[point2], verticesA[point1], edgesA[point0].point0, edgesA[point2].point0, edgesA[point1].point0, edgesA[edge2], edgesA[edge1], edgesA[edge0]);
			Triangle triangleB = new Triangle(verticesB[point0], verticesB[point1], verticesB[point2], edgesB[point0].point0, edgesB[point1].point0, edgesB[point2].point0, edgesB[edge0], edgesB[edge1], edgesB[edge2]);
			
			a.triangles.Add(triangleA);
			b.triangles.Add(triangleB);
		}
	}
	
	private void ValidateOutput(Hull a, Hull b, Vector3 planeNormal)
	{
		float lengthA = a.LengthAlongAxis(planeNormal);
		float lengthB = b.LengthAlongAxis(planeNormal);
		
		float sum = lengthA + lengthB;
		
		if (sum < smallestValidLength)
		{
			a.Clear();
			b.Clear();
		}
		else if (lengthA / sum < smallestValidRatio)
		{
			a.Clear();
		}
		else if (lengthB / sum < smallestValidRatio)
		{
			b.Clear();
		}
	}
	
	private float LengthAlongAxis(Vector3 axis)
	{
		float min = 0.0f;
		float max = 0.0f;
		
		for (int i = 0; i < points.Count; i++)
		{
			float distance = Vector3.Dot(points[i].position, axis);
			
			if (i == 0)
			{
				min = distance;
				max = distance;
			}
			else
			{
				min = Mathf.Min(distance, min);
				max = Mathf.Max(distance, max);
			}
		}
		
		return max - min;
	}
}