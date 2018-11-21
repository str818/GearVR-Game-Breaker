using UnityEngine;
using System.Collections;

public class LogicGlobalFracturing : MonoBehaviour
{
    [HideInInspector]
    public static bool HelpVisible = true;

    void Start()
    {
        HelpVisible = true;
    }

    public static void GlobalGUI()
    {
        GUI.Box(new Rect(0, 0, 400, 420), "");
        GUI.Box(new Rect(0, 0, 400, 420), "-----Ultimate Fracturing & Destruction Tool-----");
        GUILayout.Space(40);
        GUILayout.Label("Press F1 to show/hide this help window");
        GUILayout.Label("Press 1-" + Application.levelCount + " to select different sample scenes");
        GUILayout.Space(20);
    }

    void Update()
    {
        for(int i = 0; i < Application.levelCount; i++)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i)) Application.LoadLevel(i);
        }

        if(Input.GetKeyDown(KeyCode.F1))
        {
            HelpVisible = !HelpVisible;
        }
    }
}
