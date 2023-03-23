using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

[RequireComponent(typeof(Collider))]
public class InteractionMuscleLabel : GeneralInteraction
{
    [Header("Anatomy Settings")]
    [SerializeField] LabelCategory AnatomyLabelCategory;
    [Header("UI Settings")]
    [SerializeField] string LabelTitle;
    [SerializeField] [TextArea(2, 4)] string LabelDescription;
    public PhotonView PV;

    public bool InteractionState
    {
        get
        {
            return this.interactionState;
        }
        set
        {
            this.interactionState = value;

            interactableCollider.enabled = value;
            InteractableMesh.material.SetColor("_Color", (value) ? Color.white : new Color(1.0f, 1.0f, 1.0f, 0.3f));
            InteractableMesh.material.SetColor("_EmissionColor", (value) ? NormalEmissionColor : Color.black);
        }
    }
    private bool interactionState;
    public string GetLabelTitle { get { return LabelTitle; } }
    public string GetLabelDescription { get { return LabelDescription; } }
    public LabelCategory GetLabelCategory { get { return AnatomyLabelCategory; } }
    private InteractionMuscleLabelManager interactionMuscleLabelManager;
    private Collider interactableCollider;

    new void Awake()
    {
        base.Awake();
        interactionMuscleLabelManager = FindObjectOfType<InteractionMuscleLabelManager>();
        interactableCollider = GetComponent<Collider>();
        InteractionState = false;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV != null)
        {
            //Debug.Log( this.name + " has a PV!");
        }
    }

    // This is triggered by the InteractionLabelManager. Not by the EventTrigger component
    public override void UnSelect()
    {
        //if (PV.IsMine)
        //{
        //    PV.RPC("InteractionStateChecker", RpcTarget.Others);
        //}
        if (InteractionState)
        {
            base.UnSelect();
            PV.RPC("UnselectSync", RpcTarget.Others);
        }
    }

    public override void Select(PointerEventData data)
    {
        if (PV.IsMine)
        {
            PV.RPC("SelectConfirmation", RpcTarget.Others);
        }
        
        if (InteractionState)
        {
            if (PV.IsMine)
            {
                PV.RPC("SomethingGotSelected", RpcTarget.Others);
                PV.RPC("SelectSync", RpcTarget.Others, this.name);
            }
            base.Select(data);
            interactionMuscleLabelManager.ChangeLabel(this);
            //Debug.Log("Something got selected!");
            
        }

    }

    public override void Select()
    {
        if (InteractionState)
        {
            //PV.RPC("SomethingGotSelected", RpcTarget.Others);
            PV.RPC("SelectSync", RpcTarget.Others, this.name);
            base.Select();
            interactionMuscleLabelManager.ChangeLabel(this);
            //Debug.Log("Something got selected!");

        }
    }

    public override void Hover(PointerEventData data)
    {
        //if (PV.IsMine)
        //{
        //    PV.RPC("InteractionStateChecker", RpcTarget.Others);
        //}
        if (InteractionState)
        {
            base.Hover(data);
            Debug.Log("The " + GetLabelTitle + " has been hovered over!");
            if (PV.IsMine)
            {
                PV.RPC("HoverSync", RpcTarget.Others);
            }
        }
    }

    public override void Hover()
    {
        if (InteractionState)
        {
            base.Hover();
            Debug.Log("The " + GetLabelTitle + " has been hovered over!");
            PV.RPC("HoverSync", RpcTarget.Others);
        }
    }

    public override void UnHover(PointerEventData data)
    {
        //if (PV.IsMine)
        //{
        //    PV.RPC("InteractionStateChecker", RpcTarget.Others);
        //}
        if (InteractionState)
        {
            base.UnHover(data);
            Debug.Log("The " + GetLabelTitle + " has been UnHovered over!");
            PV.RPC("UnHoverSync", RpcTarget.Others);
        }
    }

    public override void UnHover()
    {
        if (InteractionState)
        {
            base.UnHover();
            Debug.Log("The " + GetLabelTitle + " has been UnHovered over!");
            PV.RPC("UnHoverSync", RpcTarget.Others);
        }
    }

    [PunRPC]
    public void UnselectSync()
    {
        Debug.Log("UnselectSync has been received");
        //UnSelect();
        IsSelected = false;
        InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
        Debug.Log("UnselectSync was successful.");
    }

    [PunRPC]
    public void SelectSync(string muscleLabelName)
    {
        Debug.Log("SelectSync has been received");
        Debug.Log("The muscle selected is " + muscleLabelName);
        IsSelected = true;
        InteractableMesh.material.SetColor("_EmissionColor", SelectedEmissionColor);
        interactionMuscleLabelManager.ChangeLabel(GameObject.Find(muscleLabelName).GetComponent<InteractionMuscleLabel>());
        Debug.Log("SelectSync has worked!!");
    }

    [PunRPC]
    public void HoverSync()
    {
        Debug.Log("HoverSync has been received");
        //Hover();
        if (!IsSelected)
        {
            InteractableMesh.material.SetColor("_EmissionColor", HighlightedEmissionColor);
        }
        Debug.Log("HoverSync has been successful");
    }

    [PunRPC]
    public void UnHoverSync()
    {
        Debug.Log("UnHoverSync has been received");
        //UnHover();
        if (!IsSelected)
            InteractableMesh.material.SetColor("_EmissionColor", NormalEmissionColor);
        Debug.Log("UnHoverSync has been successful");
    }

    [PunRPC]
    public void InteractionStateChecker()
    {
        Debug.Log("The Interaction State is " + InteractionState);
    }

    [PunRPC]
    public void SelectConfirmation()
    {
        Debug.Log("SELECT IS BEING ACTIVATED");
    }

    [PunRPC]
    public void SomethingGotSelected()
    {
        Debug.Log("Something got selected!!");
    }
}
