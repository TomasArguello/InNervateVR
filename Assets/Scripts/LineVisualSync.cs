using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LineVisualSync : MonoBehaviour
{
    public PhotonView PV;
    public LineRenderer lineRend;
    public Transform lineOrigin;
    public Transform lineEnd;

    private void Awake()
    {
        PV = transform.root.GetComponent<PhotonView>();
        lineRend = GetComponent<LineRenderer>();
        lineRend.enabled = false;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (PV.IsMine)
            {
                lineRend.enabled = false;
            }
            else
            {
                lineRend.enabled = true;
                lineRend.SetPosition(0, lineOrigin.position);
                lineRend.SetPosition(1, lineEnd.position);
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PV.IsMine && PhotonNetwork.InRoom)
            {
                lineRend.enabled = true;
                lineRend.SetPosition(0, lineOrigin.position);
                lineRend.SetPosition(1, lineEnd.position);
            }
            else
            {
                lineRend.enabled = false;
            }
        }
    }


}
