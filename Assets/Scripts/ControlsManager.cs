using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    private PlatformManager PlatformManager;
    private PlatformManager.Platform Platform;

    private NetworkManager NetworkManager;
    private RoomNetworkManager RoomNetworkManager;
    private Confirmation LeaveRoomConfirmation;
    [HideInInspector]
    public Button CurrentBackButton;

    private void Awake()
    {
        PlatformManager = FindObjectOfType<PlatformManager>();
        NetworkManager = FindObjectOfType<NetworkManager>();
        RoomNetworkManager = FindObjectOfType<RoomNetworkManager>();
    }

    void Start()
    {
        Platform = PlatformManager.GetPlatform();
        LeaveRoomConfirmation = NetworkManager.LeaveRoomConfirmation;
        CurrentBackButton = null;
    }

    void Update()
    {
        if (Platform == PlatformManager.Platform.Oculus) // Oculus controller mappings
        {
            if (OVRInput.GetDown(OVRInput.Button.Start)) // Start = Leave Room
            {
                NetworkManager.LeaveRoom();
            }

            if (OVRInput.GetDown(OVRInput.Button.Two)) // B = Cancel/Back
            {
                if (LeaveRoomConfirmation != null && LeaveRoomConfirmation.IsDisplaying() && LeaveRoomConfirmation.gameObject.activeInHierarchy)
                {
                    LeaveRoomConfirmation.Cancel();
                }
                else if (CurrentBackButton != null && RoomNetworkManager.LocalPlayerHasInput)
                {
                    CurrentBackButton.onClick.Invoke();
                }
            }
        }
    }
}
