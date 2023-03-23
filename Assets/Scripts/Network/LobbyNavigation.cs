using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyNavigation : MonoBehaviour
{
    private NetworkManager NetworkManager;

    public GameObject StartPanel;
    public GameObject ConnectedPannel;
    public GameObject CreateRoomPanel;
    public GameObject JoinRoomPanel;

    private void Awake()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start()
    {
        NavigateToStart();
        //CreateRoomPanel = GameObject.Find("CreateRoomPanel");
        //JoinRoomPanel = GameObject.Find("JoinRoomPanel");
    }

    private void DisableAllPanels()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void NavigateTo(GameObject panel)
    {
        DisableAllPanels();
        panel.SetActive(true);
    }

    public void NavigateToStart()
    {
        NavigateTo(StartPanel);
    }

    public void NavigateToConnected()
    {
        NavigateTo(ConnectedPannel);
    }
}
