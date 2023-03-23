using UnityEngine;
using Photon.Pun;

public class PlatformManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ViveCameraRig;
    [SerializeField]
    private GameObject OculusCameraRig;
    [SerializeField]
    private Transform cameraRigSpawnPoint;
    [SerializeField]
    private Canvas mainCanvas;
    [SerializeField]
    private GameObject newlySpawnedCameraRig;

    public enum Platform
    {
        Oculus,
        Vive,
        Unknown
    }
    private Platform platform;
    public Platform GetPlatform() { return platform; }

    private void Awake()
    {
        mainCanvas = GameObject.Find("Main Canvas").GetComponent<Canvas>();
    }

    private void Start()
    {
        InitializeCameraRig(OculusCameraRig);
        platform = Platform.Oculus;

        /*
        if (UnityEngine.XR.XRSettings.loadedDeviceName == "OpenVR")
        {
            InitializeCameraRig(ViveCameraRig);
            platform = Platform.Vive;
        }
        else if (UnityEngine.XR.XRSettings.loadedDeviceName == "Oculus")
        {
            InitializeCameraRig(OculusCameraRig);
            platform = Platform.Oculus;
        }
        else
        {
            Debug.LogError("VR Device not supported");
            platform = Platform.Unknown;
        }
        */
    }

    private void Update()
    {
        /*
        if(newlySpawnedCameraRig == null)
        {
            return;
        }
        else  if(mainCanvas.worldCamera != newlySpawnedCameraRig.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Camera>())
        {
            mainCanvas.worldCamera = newlySpawnedCameraRig.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Camera>();
            Debug.Log("The worldCamera was forcibly changed!");
        }
        */
    }

    public void InitializeCameraRig(GameObject cameraRig)
    {
        GameObject[] tempPlayers = GameObject.FindGameObjectsWithTag("Player");

        if (tempPlayers.Length > 0)
        {
            for (int i = tempPlayers.Length - 1; i >= 0; i--)
            {
                Debug.Log("The following player was destroyed! Player " + tempPlayers[0].name);
                Destroy(tempPlayers[i]);
            }
        }

        newlySpawnedCameraRig = Instantiate(cameraRig, cameraRigSpawnPoint.position, cameraRigSpawnPoint.rotation);
        //newlySpawnedCameraRig.transform.GetChild(0).transform.GetChild(2).GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        newlySpawnedCameraRig.transform.GetChild(1).transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        //cameraRig.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Camera>().enabled = true;
        //cameraRig.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<AudioListener>().enabled = true;
        mainCanvas.worldCamera = newlySpawnedCameraRig.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Camera>();
        Debug.Log("This is done from Platform Manager! The camera of the " + mainCanvas.name + " is " + mainCanvas.worldCamera.name);
    }
}
