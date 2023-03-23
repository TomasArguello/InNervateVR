using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationStartUp : MonoBehaviour
{
    //CUSTOM SCRIPT MADE BY TOMAS SINCE WE HAVE TO DO EVERYTHING OURSELVES
    //IN ORDER TO TELEPORT, THE TELEPORT AREA NEEDS A TELEPORT PROVIDER, WHICH AN XR RIG SHOULD HAVE
    public TeleportationProvider teleportProvider;
    public TeleportationArea teleportArea;
    
    // Start is called before the first frame update
    void Start()
    {
        teleportArea = GetComponent<TeleportationArea>();
    }

    // Update is called once per frame
    void Update()
    {
        if(teleportProvider == null)
        {
            teleportProvider = GameObject.Find("Generic VR player(Clone)").transform.GetChild(0).GetComponent<TeleportationProvider>();
            teleportArea.teleportationProvider = teleportProvider;
        }
    }
}
