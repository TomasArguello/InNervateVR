using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
//using Crosstales.RTVoice;

/**
 This script will be executed as long as a new player is instantiated
 */
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{
    public string VoiceName;
    public GameObject LocalXRRigGameObject;
    public GameObject MainAvatarGameObject;
    public Canvas mainCanvas, overlayCanvas;
    public Color headsetColor;
    public PhotonView PV;

    private void Awake()
    {
        mainCanvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        Debug.Log("what is mainCanvas? Its " + mainCanvas.name);
        overlayCanvas = GameObject.Find("Canvas - Overlay").GetComponent<Canvas>();
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady && PhotonNetwork.InRoom)
        {
            disableInputTest(LocalXRRigGameObject);
        }
        /*
        mainCanvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
        Debug.Log("what is mainCanvas? Its " + mainCanvas.name);
        overlayCanvas = GameObject.Find("Canvas - Overlay").GetComponent<Canvas>();
        */

        int colorCode;

        if (PhotonNetwork.IsConnectedAndReady)
        {
            switch (PhotonNetwork.LocalPlayer.ActorNumber)
            {
                case 1:
                    headsetColor = Color.red;
                    colorCode = 1;
                    break;
                case 2:
                    headsetColor = Color.blue;
                    colorCode = 2;
                    break;
                case 3:
                    headsetColor = Color.green;
                    colorCode = 3;
                    break;
                default:
                    headsetColor = Color.white;
                    colorCode = 0;
                    break;
            }

            PV.RPC("SyncColor", RpcTarget.AllBuffered, colorCode);

            /*
            if(PhotonNetwork.LocalPlayer.ActorNumber != PV.Controller.ActorNumber)
            {
                transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color = headsetColor;
            }
            else
            {
                return;
            }
            */
            //PV.RPC("SyncColor", RpcTarget.OthersBuffered, colorCode);
        }

        else
        {
            headsetColor = Color.cyan;
            transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color = headsetColor;
        }
    }

    public void disableInputTest(GameObject LocalXRRig)
    {
        if (photonView.IsMine)
        {
            LocalXRRig.SetActive(true);
            Debug.Log("The state of LocalXRRigGameObject is " + LocalXRRig.activeSelf);
            MainAvatarGameObject.AddComponent<AudioListener>();
            mainCanvas.worldCamera = LocalXRRig.transform.GetChild(0).transform.GetChild(0).GetComponent<Camera>();
        }
        else
        {
            Debug.Log("The state of LocalXRRigGameObject is " + LocalXRRig.activeSelf + " and the PhotonView is not mine!");
            LocalXRRig.SetActive(false);
        }
        //gameObject.GetComponent<OVRManager>().enabled = false;
    }

    public void disableInput()
    {
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
        {
            //gameObject.GetComponent<OVRManager>().enabled = true;
            LocalXRRigGameObject.SetActive(true);
        }
        else
        {
            //Setup the player
            if (PhotonNetwork.IsConnectedAndReady)
            {
                LocalXRRigGameObject.SetActive(false);
                Debug.Log("For some reason, PhotonNetwork.IsConnectedAndReady is " + PhotonNetwork.IsConnectedAndReady + " but photonView.IsMine is " + photonView.IsMine);
            }
            else
            {
                Debug.Log("This OVR Camera Rig has not connected to the Internet yet, about to disable their AvatarInputConverter!");
                LocalXRRigGameObject.SetActive(false);
            }
        }
    }

    [PunRPC]
    public void SyncColor(int colorCode)
    {
        switch (colorCode)
        {
            case 1:
                headsetColor = Color.red;
                break;
            case 2:
                headsetColor = Color.blue;
                break;
            case 3:
                headsetColor = Color.green;
                break;
            default:
                headsetColor = Color.white;
                break;
        }

        transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color = headsetColor;
        if (!PV.IsMine)
        {
            transform.Rotate(new Vector3(0f, 180f, 0f));
        }
        Debug.Log("The rotation of the transform of player color " + headsetColor + " is " + transform.rotation.eulerAngles);
        //transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    [PunRPC]
    public void Speak()
    {
        Debug.Log("RPC is called");
        Speaker.Instance.Speak(
            "Hello World", null, Speaker.Instance.VoiceForName(VoiceName));
    }
    */
}
