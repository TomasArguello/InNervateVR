using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomEditor(typeof(RoomNetworkManager))]
public class RoomNetworkManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RoomNetworkManager roomNetworkManager = (RoomNetworkManager)target;
        EditorGUILayout.HelpBox("This is used to select which module to use from the Inspector of the RoomMenuManager Gameobject.", MessageType.Info);

        if(GUILayout.Button("Return to Main Menu"))
        {
            roomNetworkManager.ChangeUIScreen(1);
        }
        if (GUILayout.Button("Label Module"))
        {
            roomNetworkManager.ChangeUIScreen(2);
        }
        if(GUILayout.Button("Nerve Cutting Module"))
        {
            roomNetworkManager.ChangeUIScreen(3);
        }
        if(GUILayout.Button("Diagnosing Module"))
        {
            roomNetworkManager.ChangeUIScreen(4);
        }
    }
}
#endif
