// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseForce), typeof(MouseSplit), typeof(MouseShatter))]
public class MouseGUI : MonoBehaviour
{
	public int defaultSelection = 0;
	
	private MouseForce mouseForce;
	private MouseSplit mouseSplit;
	private MouseShatter mouseShatter;
	
	private int toolbarSelection = 0;
	private System.String[] toolbarLabels = { "Mouse force (Click and drag)", "Mouse split (Click and drag, release)", "Mouse shatter (Click)" };
	
	public void Awake()
	{
		mouseForce = GetComponent<MouseForce>();
		mouseSplit = GetComponent<MouseSplit>();
		mouseShatter = GetComponent<MouseShatter>();
		
		toolbarSelection = defaultSelection;
		
		SelectTool();
	}
	
	public void OnGUI()
	{
		toolbarSelection = GUI.Toolbar(new Rect(10, 10, Screen.width - 20, 20), toolbarSelection, toolbarLabels);
		
		if (GUI.changed)
		{
			SelectTool();
		}
	}
	
	private void SelectTool()
	{
		mouseForce.enabled = false;
		mouseSplit.enabled = false;
		mouseShatter.enabled = false;
		
		if (toolbarSelection == 0)
		{
			mouseForce.enabled = true;
		}
		else if (toolbarSelection == 1)
		{
			mouseSplit.enabled = true;
		}
		else if (toolbarSelection == 2)
		{
			mouseShatter.enabled = true;
		}
	}
}