using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HealthyLeg : MonoBehaviour {

    public Animator legAnim;
    public PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void playAnimation()
    {
        legAnim.Play("NoMovement");
        if (PV.IsMine)
        {
            PV.RPC("playAnimSync", RpcTarget.Others);
        }

    }

    [PunRPC]
    public void playAnimSync()
    {
        legAnim.Play("NoMovement");
        Debug.Log("The Healthy Leg has animated!");
    }
}
