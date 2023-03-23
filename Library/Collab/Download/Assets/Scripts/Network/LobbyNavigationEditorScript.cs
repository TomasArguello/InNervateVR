using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LobbyNavigation))]
public class LobbyNavigationEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("These custom Inspector buttons are used to test the connectivity of rooms without putting on the Oculus headset.", MessageType.Info);

        LobbyNavigation lobbyNavigation = (LobbyNavigation)target;

        if (GUILayout.Button("Create"))
        {
            lobbyNavigation.NavigateTo(lobbyNavigation.CreateRoomPanel);
        }

        if (GUILayout.Button("Join"))
        {
            lobbyNavigation.NavigateTo(lobbyNavigation.JoinRoomPanel);
        }

    }
}
