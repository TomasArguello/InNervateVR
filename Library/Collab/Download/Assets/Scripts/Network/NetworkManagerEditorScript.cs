using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NetworkManager))]
public class NetworkManagerEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("This custom Inspector button is to test room connectivity without putting on the Oculus headset. Once the user is connected, go to the " +
            "LobbyUI gameobject! After choosing Create from the LobbyUI gameobject, choose the Create button shown below!", MessageType.Info);

        NetworkManager networkManager = (NetworkManager)target;

        if (GUILayout.Button("Connect"))
        {
            networkManager.Connect();
        }

        //This is for specifically when the user has chosen the Create option from the LobbyUI gameobject and will connect the user to the room created 
        if (GUILayout.Button("Create"))
        {
            networkManager.CreateRoom();
        }

        if (GUILayout.Button("Join"))
        {
            networkManager.JoinSelectedRoom();
        }
    }
}
