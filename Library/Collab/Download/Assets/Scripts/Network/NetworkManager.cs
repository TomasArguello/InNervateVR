using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Scene Setup")]
    public GameObject LobbyUI;
    private LobbyNavigation LobbyNavigator;
    public GameObject RoomUI;
    [SerializeField]
    private RoomList RoomList;
    [SerializeField]
    public RoomCreator RoomCreator;

    [SerializeField]
    private Keyboard UsernameKeyboard;
    public Text[] UsernameDisplays = new Text[0];
    [SerializeField]
    private Keyboard RoomNameKeyboard;

    public Confirmation LeaveRoomConfirmation;
    public GameObject LoadingPopup;
    public GameObject ConnectionFailedMessage;
    public GameObject CreateRoomFailedMessage;
    public GameObject JoinRoomFailedMessage;

    [Header("Network Settings")]
    [SerializeField]
    [Tooltip("NOT IMPLEMENTED. If true, multiple users can have the same username. If false, a user cannot connect until they have entered a username not in use by any currently connected users.")]
    private bool AllowDuplicateUsernames = true; // not implemented
    private Dictionary<string, RoomInfo> CachedRoomInfo = new Dictionary<string, RoomInfo>();
    public bool OfflineMode { get; private set; } = true;


    private void Awake()
    {
        OfflineMode = true;
        LobbyNavigator = LobbyUI.GetComponent<LobbyNavigation>();
        SwitchUI(false);
    }

    private void Start()
    {
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (ConnectionFailedMessage != null)
            ConnectionFailedMessage.SetActive(false);
    }

    public void ConnectToPhotonServer()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    #region  Photon Callback Methods

    public override void OnConnected()
    {
        Debug.Log("OnConnected is called. The server is available.");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to the Master Server with player name: " + PhotonNetwork.NickName + ".");
        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("DISCONNECTED!");
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (!OfflineMode) // unintentional disconnect
        {
            OfflineMode = true;
            SwitchUI(false);
            if (ConnectionFailedMessage != null)
                ConnectionFailedMessage.SetActive(true);
        }
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room with the name " + PhotonNetwork.CurrentRoom.Name + ".");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("The local player " + PhotonNetwork.NickName + " joined room: " + PhotonNetwork.CurrentRoom.Name + ". Player count: " + PhotonNetwork.CurrentRoom.PlayerCount);
        SwitchUI(true);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("The local player " + PhotonNetwork.NickName + " left a room.");
        //SwitchUI(false);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby.");
        CachedRoomInfo.Clear();
        if (RoomList != null)
            RoomList.Clear();
        SwitchUI(false);
    }

    public override void OnLeftLobby()
    {
        Debug.Log("Left Lobby.");
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        LobbyNavigator.NavigateToStart();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate called.");
        UpdateCachedRoomList(roomList);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed with message:");
        Debug.Log(message);
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (CreateRoomFailedMessage != null)
            CreateRoomFailedMessage.SetActive(true);
    }

    public override void OnJoinRoomFailed (short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed with message:");
        Debug.Log(message);
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (JoinRoomFailedMessage != null)
            JoinRoomFailedMessage.SetActive(true);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed with message:");
        Debug.Log(message);
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (JoinRoomFailedMessage != null)
            JoinRoomFailedMessage.SetActive(true);
    }

    #endregion


    #region UI Callback Methods

    public void Connect()
    {
        if (UsernameKeyboard != null)
        {
            if (!UsernameKeyboard.Validate())
            {
                UsernameKeyboard.ShowInvalid();
                return;
            }
            else
            {
                SetNickname(UsernameKeyboard.GetEntry());
            }
        }

        OfflineMode = false;
        if (ConnectionFailedMessage != null)
            ConnectionFailedMessage.SetActive(false);
        if (!PhotonNetwork.IsConnected)
        {
            if (LoadingPopup != null)
                LoadingPopup.SetActive(true);
            ConnectToPhotonServer();
        }
        else if (!PhotonNetwork.InLobby)
        {
            if (LoadingPopup != null)
                LoadingPopup.SetActive(true);
            PhotonNetwork.JoinLobby();
        }
        else
        {
            LobbyNavigator.NavigateToConnected();
        }
    }

    public void ContinueOffline()
    {
        OfflineMode = true;
        if (ConnectionFailedMessage != null)
            ConnectionFailedMessage.SetActive(false);
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
        SwitchUI(true);
    }

    public void JoinSelectedRoom()
    {
        if (RoomList != null && RoomList.GetSelectedRoom() != "")
        {
            if (JoinRoomFailedMessage != null)
                JoinRoomFailedMessage.SetActive(false);
            if (LoadingPopup != null)
                LoadingPopup.SetActive(true);
            PhotonNetwork.JoinRoom(RoomList.GetSelectedRoom());
        }
    }

    public void CreateRoom()
    {
        if (RoomCreator != null)
        {
            if (CreateRoomFailedMessage != null)
                CreateRoomFailedMessage.SetActive(false);
            if (RoomNameKeyboard != null)
            {
                if (!RoomNameKeyboard.Validate())
                {
                    RoomNameKeyboard.ShowInvalid();
                    return;
                }
            }
            if (LoadingPopup != null)
                LoadingPopup.SetActive(true);
            PhotonNetwork.CreateRoom(RoomCreator.GetRoomName(), RoomCreator.GetRoomOptions(), null);
        }
    }

    public void LeaveRoom()
    {
        if ((OfflineMode && RoomUI.activeInHierarchy) || (!OfflineMode && PhotonNetwork.InRoom))
        {
            if (LeaveRoomConfirmation != null && !LeaveRoomConfirmation.IsDisplaying())
            {
                LeaveRoomConfirmation.Display();
            }
            else
            {
                LeaveRoomWithoutConfirmation();
            }
        }
    }

    private void LeaveRoomWithoutConfirmation()
    {
        if ((OfflineMode && RoomUI.activeInHierarchy) || (!OfflineMode && PhotonNetwork.InRoom))
        {
            if (LeaveRoomConfirmation != null)
                LeaveRoomConfirmation.Hide();
            
            if (OfflineMode)
            {
                SwitchUI(false);
            }
            else
            {
                if (LoadingPopup != null)
                    LoadingPopup.SetActive(true);
                PhotonNetwork.LeaveRoom();
            }
        }
    }

    #endregion


    #region Unique Functions

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = roomList[i];
            if (info.RemovedFromList)
            {
                RemoveRoomFromCache(info);
            }
            else
            {
                AddRoomToCache(info);
            }
        }

        Debug.Log(CachedRoomInfo.Count + " rooms available: ");
        foreach (KeyValuePair<string, RoomInfo> entry in CachedRoomInfo)
        {
            Debug.Log("Room: " + entry.Value.Name);
        }
    }

    private void AddRoomToCache(RoomInfo info)
    {
        if (RoomList != null)
        {
            if (!CachedRoomInfo.ContainsKey(info.Name))
                RoomList.AddRoom(info);
            else
                RoomList.UpdateRoom(info);
        }
        CachedRoomInfo[info.Name] = info;
    }

    private void UpdateRoomInCache(RoomInfo info)
    {
        if (CachedRoomInfo.ContainsKey(info.Name))
        {
            CachedRoomInfo[info.Name] = info;

            if (RoomList != null)
            {
                //RoomList.UpdateList(GetRooms());
            }
        }
    }

    private void RemoveRoomFromCache(RoomInfo info)
    {
        CachedRoomInfo.Remove(info.Name);

        if (RoomList != null)
        {
            RoomList.RemoveRoom(info);
        }
    }

    public List<RoomInfo> GetRooms()
    {
        return CachedRoomInfo.Values.ToList();
    }

    private void SwitchUI(bool joinedRoom)
    {
        LobbyUI.SetActive(!joinedRoom);
        RoomUI.SetActive(joinedRoom);
        if (LoadingPopup != null)
            LoadingPopup.SetActive(false);
        if (LeaveRoomConfirmation != null)
            LeaveRoomConfirmation.Hide();
        
        if (!joinedRoom)
        {
            if (!OfflineMode && PhotonNetwork.InLobby)
                LobbyNavigator.NavigateToConnected();
            else
                LobbyNavigator.NavigateToStart();
        }
    }

    public bool DoesRoomExist(string roomName)
    {
        return CachedRoomInfo.ContainsKey(roomName);
    }

    public bool UsernameTaken(string username)
    {
        if (AllowDuplicateUsernames)
            return false;
        // not implemented - not sure if possible to get list of nicknames of all connected players, only players in a particular room
        return false;
    }

    public void SetNickname(string nickname)
    {
        PhotonNetwork.NickName = nickname;
        foreach (Text t in UsernameDisplays)
        {
            t.text = nickname;
        }
    }

    public string GetNickname()
    {
        return PhotonNetwork.NickName;
    }

    public bool IsConnected()
    {
        return PhotonNetwork.IsConnected;
    }

    public bool InLobby()
    {
        return PhotonNetwork.InLobby;
    }

    #endregion
}
