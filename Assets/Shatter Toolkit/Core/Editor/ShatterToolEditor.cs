// Shatter Toolkit
// Copyright 2011 Gustav Olsson
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShatterTool))]
public class ShatterToolEditor : Editor
{
	private static System.String generationLimitTooltip =	"Gets or sets the generation limit of this ShatterTool instance. This value restricts how many times a game object may be shattered using ShatterTool.Shatter(). A game object will only be able to shatter if ShatterTool.Generation is less than ShatterTool.GenerationLimit.";
	private static System.String cutsTooltip =				"Gets or sets the number of times the game object will be cut when ShatterTool.Shatter() occurs.";
	private static System.String fillCutTooltip =			"Gets or sets whether the cut region should be triangulated. If true, the connected UvMapper component will control the vertex properties of the filled area. When the ShatterTool is used on double-sided meshes with zero thickness, such as planes, this value should be false.";
	private static System.String preSplitMsgTooltip =		"Gets or sets whether a PreSplit(Plane[] planes) message should be sent to the original game object prior to a split occurs. The supplied object will be the array of Planes that will be used to split the game object.";
	private static System.String postSplitMsgTooltip =		"Gets or sets whether a PostSplit(GameObject[] newGameObjects) message should be sent to the original game object after a split has occured. The message will be sent before destroying the original game object. The supplied object will be an array of all new GameObjects created during the split.";
	
	public override void OnInspectorGUI()
	{
		ShatterTool source = (ShatterTool)target;
		
		EditorGUILayout.BeginVertical();
		
		// Generation
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Generation", source.Generation.ToString());
		EditorGUILayout.EndHorizontal();
		
		// GenerationLimit
		EditorGUILayout.BeginHorizontal();
		source.GenerationLimit = EditorGUILayout.IntSlider(new GUIContent("Generation Limit", generationLimitTooltip), source.GenerationLimit, 1, 30);
		EditorGUILayout.EndHorizontal();
		
		// Cuts
		EditorGUILayout.BeginHorizontal();
		source.Cuts = EditorGUILayout.IntSlider(new GUIContent("Cuts", cutsTooltip), source.Cuts, 1, 25);
		EditorGUILayout.EndHorizontal();
		
		// FillCut
		EditorGUILayout.BeginHorizontal();
		source.FillCut = EditorGUILayout.Toggle(new GUIContent("Fill Cut", fillCutTooltip), source.FillCut);
		EditorGUILayout.EndHorizontal();
		
		// SendPreSplitMessage
		EditorGUILayout.BeginHorizontal();
		source.SendPreSplitMessage = EditorGUILayout.Toggle(new GUIContent("Pre Split msg", preSplitMsgTooltip), source.SendPreSplitMessage);
		EditorGUILayout.EndHorizontal();
		
		// SendPostSplitMessage
		EditorGUILayout.BeginHorizontal();
		source.SendPostSplitMessage = EditorGUILayout.Toggle(new GUIContent("Post Split msg", postSplitMsgTooltip), source.SendPostSplitMessage);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.EndVertical();
	}
}