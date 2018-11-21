// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class ShatterTool : MonoBehaviour
{
	[SerializeField]
	private int generation = 1;
	
	[SerializeField]
	private int generationLimit = 3;
	
	[SerializeField]
	private int cuts = 2;
	
	[SerializeField]
	private bool fillCut = true;
	
	[SerializeField]
	private bool sendPreSplitMessage = false;
	
	[SerializeField]
	private bool sendPostSplitMessage = false;
	
	private Hull hull;
	
	private UvMapper uvMapper;
	
	/// <summary>
	/// Gets or sets the current generation of this ShatterTool instance.
	/// By default, a game object is of generation 1. When a game object
	/// is shattered using ShatterTool.Shatter() all new game objects
	/// will be considered of generation 2, and so on.
	/// For example, this value can be used to vary the color of a
	/// game object depending on how many times it has been shattered.
	/// </summary>
	public int Generation
	{
		get { return generation; }
		set { generation = Mathf.Max(value, 1); }
	}
	
	/// <summary>
	/// Gets or sets the generation limit of this ShatterTool instance.
	/// This value restricts how many times a game object may be shattered
	/// using ShatterTool.Shatter(). A game object will only be able to shatter
	/// if ShatterTool.Generation is less than ShatterTool.GenerationLimit.
	/// </summary>
	public int GenerationLimit
	{
		get { return generationLimit; }
		set { generationLimit = Mathf.Max(value, 1); }
	}
	
	/// <summary>
	/// Gets or sets the number of times the game object will be cut when ShatterTool.Shatter() occurs.
	/// </summary>
	public int Cuts
	{
		get { return cuts; }
		set { cuts = Mathf.Max(value, 1); }
	}
	
	/// <summary>
	/// Gets or sets whether the cut region should be triangulated.
	/// If true, the connected UvMapper component will control the vertex properties of the filled area.
	/// When the ShatterTool is used on double-sided meshes with zero thickness, such as planes, this value
	/// should be false.
	/// </summary>
	public bool FillCut
	{
		get { return fillCut; }
		set { fillCut = value; }
	}
	
	/// <summary>
	/// Gets or sets whether a PreSplit(Plane[] planes) message should be sent to the original game object prior to a split occurs.
	/// The supplied object will be the array of Planes that will be used to split the game object.
	/// </summary>
	public bool SendPreSplitMessage
	{
		get { return sendPreSplitMessage; }
		set { sendPreSplitMessage = value; }
	}
	
	/// <summary>
	/// Gets or sets whether a PostSplit(GameObject[] newGameObjects) message should be sent to the original game object
	/// after a split has occured. The message will be sent before destroying the original game object.
	/// The supplied object will be an array of all new GameObjects created during the split.
	/// </summary>
	public bool SendPostSplitMessage
	{
		get { return sendPostSplitMessage; }
		set { sendPostSplitMessage = value; }
	}
	
	/// <summary>
	/// Gets or sets the connected UvMapper. See the documentation of ShatterTool.FillCut.
	/// This value only needs to be set if a script replaces a UvMapper component after
	/// a game object has been instantiated.
	/// </summary>
	public UvMapper UvMapper
	{
		get { return uvMapper; }
		set { uvMapper = value; }
	}
	
	/// <summary>
	/// Determines whether this game object is of the first generation. (Generation == 1)
	/// </summary>
	public bool IsFirstGeneration
	{
		get { return generation == 1; }
	}
	
	/// <summary>
	/// Determines whether this game object is of the last generation. (Generation >= GenerationLimit)
	/// </summary>
	public bool IsLastGeneration
	{
		get { return generation >= generationLimit; }
	}
	
	public void Awake()
	{
		uvMapper = GetComponent<UvMapper>();
	}
	
	public void Start()
	{
		if (hull == null)
		{
			hull = new Hull(GetComponent<MeshFilter>().mesh);
		}
	}
	
	/// <summary>
	/// Shatters the game object at a point, instantiating the pieces as new
	/// game objects (clones of the original) and destroying the original game object when finished.
	/// If the game object has reached the generation limit, nothing will happen.
	/// Apart from taking the generation into account, this is equivalent to calling
	/// ShatterTool.Split() using randomly generated planes passing through the point.
	/// </summary>
	/// <param name="point">
	/// The world-space point.
	/// </param>
	public void Shatter(Vector3 point)
	{
		if (!IsLastGeneration)
		{
			// Increase generation
			generation++;
			
			// Split the hull using randomly generated planes passing through the point
			Plane[] planes = new Plane[cuts];
			
			for (int i = 0; i < planes.Length; i++)
			{
				planes[i] = new Plane(Random.onUnitSphere, point);
			}
			
			Split(planes);
		}
	}
	
	/// <summary>
	/// Splits the game object using an array of planes, instantiating the pieces as new
	/// game objects (clones of the original) and destroying the original game object when finished.
	/// </summary>
	/// <param name="planes">
	/// An array of world-space planes with unit-length normals.
	/// </param>
	public void Split(Plane[] planes)
	{
		if (sendPreSplitMessage)
		{
			SendMessage("PreSplit", planes, SendMessageOptions.DontRequireReceiver);
		}
		
		if (planes != null && planes.Length > 0 && hull != null && !hull.IsEmpty)
		{
			if (uvMapper != null)
			{
				Plane[] localPlanes;
				
				CreateLocalPlanes(planes, out localPlanes);
				
				IList<Hull> newHulls;
				
				CreateNewHulls(localPlanes, out newHulls);
				
				GameObject[] newGameObjects;
				
				CreateNewGameObjects(newHulls, out newGameObjects);
				
				if (sendPostSplitMessage)
				{
					SendMessage("PostSplit", newGameObjects, SendMessageOptions.DontRequireReceiver);
				}
				
				Destroy(gameObject);
			}
			else
			{
				Debug.LogWarning(name + " has no UvMapper attached! Please attach a UvMapper to use the ShatterTool.", this);
			}
		}
	}
	
	private void CreateLocalPlanes(Plane[] planes, out Plane[] localPlanes)
	{
		localPlanes = new Plane[planes.Length];
		
		for (int i = 0; i < planes.Length; i++)
		{
			Plane plane = planes[i];
			
			Vector3 localPoint = transform.InverseTransformPoint(plane.normal * -plane.distance);
			Vector3 localNormal = transform.InverseTransformDirection(plane.normal);
			
			localNormal.Scale(transform.localScale);
			
			localNormal.Normalize();
			
			localPlanes[i] = new Plane(localNormal, localPoint);
		}
	}
	
	private void CreateNewHulls(Plane[] localPlanes, out IList<Hull> newHulls)
	{
		newHulls = new List<Hull>();
		
		// Add the starting hull
		newHulls.Add(hull);
		
		foreach (Plane plane in localPlanes)
		{
			int previousHullCount = newHulls.Count;
			
			for (int i = 0; i < previousHullCount; i++)
			{
				Hull previousHull = newHulls[0];
				
				// Split the previous hull
				Hull a, b;
				
				previousHull.Split(plane.normal * -plane.distance, plane.normal, fillCut, uvMapper, out a, out b);
				
				// Update the list
				newHulls.Remove(previousHull);
				
				if (!a.IsEmpty)
				{
					newHulls.Add(a);
				}
				
				if (!b.IsEmpty)
				{
					newHulls.Add(b);
				}
			}
		}
	}
	
	private void CreateNewGameObjects(IList<Hull> newHulls, out GameObject[] newGameObjects)
	{
		// Get new meshes
		Mesh[] newMeshes = new Mesh[newHulls.Count];
		float[] newVolumes = new float[newHulls.Count];
		
		float totalVolume = 0.0f;
		
		for (int i = 0; i < newHulls.Count; i++)
		{
			Mesh mesh = newHulls[i].GetMesh();
			Vector3 size = mesh.bounds.size;
			float volume = size.x * size.y * size.z;
			
			newMeshes[i] = mesh;
			newVolumes[i] = volume;
			
			totalVolume += volume;
		}
		
		// Create new game objects
		newGameObjects = new GameObject[newHulls.Count];
		
		for (int i = 0; i < newHulls.Count; i++)
		{
			Hull newHull = newHulls[i];
			Mesh newMesh = newMeshes[i];
			float volume = newVolumes[i];
			
			GameObject newGameObject = (GameObject)Instantiate(gameObject);
			
			// Set shatter tool
			ShatterTool newShatterTool = newGameObject.GetComponent<ShatterTool>();
			
			if (newShatterTool != null)
			{
				newShatterTool.hull = newHull;
			}
			
			// Set mesh filter
			MeshFilter newMeshFilter = newGameObject.GetComponent<MeshFilter>();
			
			if (newMeshFilter != null)
			{
				newMeshFilter.mesh = newMesh;
			}
			
			// Set mesh collider
			MeshCollider newMeshCollider = newGameObject.GetComponent<MeshCollider>();
			
			if (newMeshCollider != null)
			{
				newMeshCollider.sharedMesh = newMesh;
			}
			
			// Set rigidbody
			Rigidbody newRigidbody = newGameObject.GetComponent<Rigidbody>();
			
			if (newRigidbody != null)
			{
				newRigidbody.mass = GetComponent<Rigidbody>().mass * (volume / totalVolume);
				
				if (!newRigidbody.isKinematic)
				{
					newRigidbody.velocity = GetComponent<Rigidbody>().GetPointVelocity(newRigidbody.worldCenterOfMass);
					
					newRigidbody.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
				}
			}
			
			newGameObjects[i] = newGameObject;
		}
	}
}