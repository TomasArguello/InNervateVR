using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
using Photon.Realtime;

public class RoomCreator : MonoBehaviour
{
    private MenuManager RoomMenuManager;
    public InteractionMuscleLabelManager interactionMuscleLabelManager;

    [Header("Fixed Room Settings")]
    public int MaxPlayers = 5;
    [Tooltip("Amount of time in milliseconds to wait before destroying a room after it becomes empty. If another player joins the room during this time, it will not be destroyed.")]
    public int EmptyRoomDestroyTimer = 50000;

    [Header("Custom Room Inputs")]
    public Text RoomName;
    public Toggle AllowInput;

    private void Awake()
    {
        RoomMenuManager = FindObjectOfType<MenuManager>();
        interactionMuscleLabelManager = FindObjectOfType<InteractionMuscleLabelManager>();
    }
    
    public string GetRoomName()
    {
        return RoomName.text;
    }

    public RoomOptions GetRoomOptions()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = System.Convert.ToByte(MaxPlayers);
        roomOptions.EmptyRoomTtl = EmptyRoomDestroyTimer;
        roomOptions.BroadcastPropsChangeToAll = true;
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            {"AllowInput", AllowInput.isOn},
            {"CurrentScreen", RoomMenuManager.GetStartScreen()}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "AllowInput" };
        roomOptions.IsVisible = true;
        return roomOptions;
    }
}
