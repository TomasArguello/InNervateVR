using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class InteractiveNerveAP : GeneralInteraction
{
    cutNerve CutNerve;
    PhotonView PV;

    protected new void Awake()
    {
        base.Awake();
        CutNerve = GetComponent<cutNerve>();
        PV = GetComponent<PhotonView>();
    }

    public override void Select(PointerEventData data)
    {
        CutNerve.CutNerve();
        PV.RPC("SelectSync", RpcTarget.Others);
    }

    public override void Select()
    {
        //base.Select();
        CutNerve.CutNerve();
        PV.RPC("SelectSync", RpcTarget.Others);
    }

    public override void UnSelect() {}
    public override void Hover(PointerEventData data) {}
    public override void UnHover(PointerEventData data) {}

    [PunRPC]
    public void SelectSync()
    {
        CutNerve.CutNerve();
    }
}
