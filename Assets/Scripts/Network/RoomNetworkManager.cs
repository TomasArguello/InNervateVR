using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomNetworkManager : MonoBehaviour, IInRoomCallbacks
{
    private NetworkManager NetworkManager;
    private MenuManager RoomMenuManager;
    public InteractionMuscleLabelManager interactionMuscleLabelManager;

    public GameObject OnlineMenu;
    public GameObject OfflineMenu;

    public TextMeshProUGUI RoomName;
    public Text PlayerCount;

    public bool LocalPlayerHasInput = true;

    private void Awake()
    {
        NetworkManager = FindObjectOfType<NetworkManager>();
        RoomMenuManager = FindObjectOfType<MenuManager>();
        interactionMuscleLabelManager = FindObjectOfType<InteractionMuscleLabelManager>();

        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnEnable()
    {
        if (!NetworkManager.OfflineMode)
        {
            OnlineMenu.SetActive(true);
            OfflineMenu.SetActive(false);
            if (PhotonNetwork.InRoom)
            {
                PopulateRoomInfo();

                /***** NETWORK ROOM SETUP *****/

                // Synchronize local UI to the state of room
                RoomMenuManager.ChangeUIScreen((UIScreens)PhotonNetwork.CurrentRoom.CustomProperties["CurrentScreen"]);

                // Room owner vs regular player
                if (PhotonNetwork.IsMasterClient) // room owner
                {
                    
                }
                else // regular player
                {
                    if (!(bool)PhotonNetwork.CurrentRoom.CustomProperties["AllowInput"])
                    {
                        DisableLocalPlayerInput();
                    }
                }
            }
        }
        else
        {
            OnlineMenu.SetActive(false);
            OfflineMenu.SetActive(true);
            RoomName.text = "Offline";
        }
    }

    private void OnDisable()
    {
        EnableLocalPlayerInput();
        RoomMenuManager.Reset();
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }



    #region Room Info Population

    private void PopulateRoomInfo()
    {
        RoomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerCount();
    }

    private void UpdatePlayerCount()
    {
        PlayerCount.text = ((int)PhotonNetwork.CurrentRoom.PlayerCount).ToString() + "/" + ((int)PhotonNetwork.CurrentRoom.MaxPlayers).ToString();
    }

    private void Update()
    {
        /*
        if ((RoomMenuManager.GetCurrentScreen() == UIScreens.MainMenu) && (!NetworkManager.OfflineMode) && (PhotonNetwork.CurrentRoom != null))
        {
            UpdatePlayerCount();
        }
        */
    }

    #endregion



    #region Room Management

    private void EnableLocalPlayerInput()
    {
        LocalPlayerHasInput = true;
        Wacki.LaserPointerInputModule inputModule = FindObjectOfType<Wacki.LaserPointerInputModule>();
        if (inputModule != null)
        {
            inputModule.enabled = true;
        }
    }

    private void DisableLocalPlayerInput()
    {
        LocalPlayerHasInput = false;
        /*
        Wacki.LaserPointerInputModule inputModule = FindObjectOfType<Wacki.LaserPointerInputModule>();
        if (inputModule != null)
        {
            inputModule.enabled = false;
        }*/
        if (Wacki.LaserPointerInputModule.instance != null)
        {
            Wacki.LaserPointerInputModule.instance.RemoveController(GameObject.Find("LeftControllerAnchor").transform.GetChild(0).gameObject.GetComponent<Wacki.IUILaserPointer>());
            Wacki.LaserPointerInputModule.instance.RemoveController(GameObject.Find("RightControllerAnchor").transform.GetChild(0).gameObject.GetComponent<Wacki.IUILaserPointer>());
        }
    }

    public void ChangeUIScreen(int newUIScreen)
    {
        try
        {
            ChangeUIScreen((UIScreens)newUIScreen);
        }
        catch
        {
            Debug.LogError("UI screen doesn't exist. Check integer value.");

        }
    }

    public void ChangeUIScreen(UIScreens newUIScreen)
    {
        if (!NetworkManager.OfflineMode)
        {
            var updatedProps = new Hashtable()
            {
                {"CurrentScreen", newUIScreen}
            };
            var expectedProps = new Hashtable()
            {
                {"CurrentScreen", RoomMenuManager.GetCurrentScreen()}
            };
            Debug.Log("The ChangeUIScreen function has been called and is about to change the CurrentScreen property");
            PhotonNetwork.CurrentRoom.SetCustomProperties(updatedProps, expectedProps);
        }
        else
        {
            RoomMenuManager.ChangeUIScreen(newUIScreen);
        }
    }

    #endregion

    

    #region Photon Callback Methods

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient && !(bool)PhotonNetwork.CurrentRoom.CustomProperties["AllowInput"])
        {
            EnableLocalPlayerInput();
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();
    }

    public void OnPlayerLeftRoom(Player oldPlayer)
    {
        UpdatePlayerCount();
    }

    public void OnRoomPropertiesUpdate(Hashtable changedProps)
    {
        Debug.Log("The room properties were changed! This is before the if statement");
        if (changedProps.ContainsKey("CurrentScreen"))
        {
            RoomMenuManager.ChangeUIScreen((UIScreens)changedProps["CurrentScreen"]);
            Debug.Log("The CurrentScreen property has been changed!");
        }
        
    }

    public void OnPlayerPropertiesUpdate (Player targetPlayer, Hashtable changedProps)
    {

    }


    #endregion
}
