using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class InteractionMuscleLabelManager : Manager
{
    [Header("UI References")]
    [SerializeField] Text TitleUI;
    [SerializeField] Text DescriptionUI;
    [SerializeField] LabelButton[] ButtonUI;
    [Header("Canine Labeling Leg")]
    [SerializeField] GameObject LabelLeg;
    [SerializeField] InteractionMuscleLabel[] InteractionLabelArray;

    public InteractionMuscleLabel[] GetInteractionLabelArray { get { return InteractionLabelArray; } }
    private LabelCategory CurrentCategory;
    public InteractionMuscleLabel CurrentLabel;
    public PhotonView PV;
    public RoomNetworkManager roomNetworkManager;

    private void Start()
    {
        roomNetworkManager = FindObjectOfType<RoomNetworkManager>();
        PV = GetComponent<PhotonView>();
        if(PV != null)
        {
            Debug.Log("InteractionMuscleLabelManager has a PV!");
        }
    }

    public override void InitiateSection()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            LabelLeg.SetActive(true);

            SetTitleAndDescriptionAlpha(0.0f);
            ResetUIElements(CurrentCategory);
        }
        else
        {
            LabelLeg.SetActive(true);

            SetTitleAndDescriptionAlpha(0.0f);
            ResetUIElements(CurrentCategory);
            Debug.Log("InitiateSection worked. Hopefully it syncs");
            PV.RPC("InitSectionSync", RpcTarget.Others);
        }
        
    }

    public override void TerminateSection()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
            }

            LabelLeg.SetActive(false);
        }
        else
        {
            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
            }

            LabelLeg.SetActive(false);
            Debug.Log("TerminateSection worked. Hopefully it syncs");
            PV.RPC("TermSectionSync", RpcTarget.Others);
        }
    }

    public void ChangeLabel(InteractionMuscleLabel newLabel)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            if (CurrentLabel == newLabel) return;

            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
            }

            SetTitleAndDescriptionAlpha(0.0f);
            TitleUI.text = newLabel.GetLabelTitle;
            DescriptionUI.text = newLabel.GetLabelDescription;
            Debug.Log(TitleUI.text);
            Debug.Log(DescriptionUI.text);
            FadeTitleAndDescription(1.0f, 0.5f);
            //roomNetworkManager.UpdateCurrentLabel(newLabel); //Important for syncing 
            CurrentLabel = newLabel;
        }
        
        else
        {
            if (CurrentLabel == newLabel) return;

            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
            }

            SetTitleAndDescriptionAlpha(0.0f);
            TitleUI.text = newLabel.GetLabelTitle;
            DescriptionUI.text = newLabel.GetLabelDescription;
            Debug.Log(TitleUI.text);
            Debug.Log(DescriptionUI.text);
            FadeTitleAndDescription(1.0f, 0.5f);
            //roomNetworkManager.UpdateCurrentLabel(newLabel); //Important for syncing 
            CurrentLabel = newLabel;
            PV.RPC("ChangeLabelSync", RpcTarget.Others, newLabel);
        }
    }

    public void ChangeLabelCategory(int newElement)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            LabelCategory newCategory = (LabelCategory)newElement;

            if (CurrentCategory == newCategory) return;

            ResetUIElements(newCategory);
        }
        else
        {
            LabelCategory newCategory = (LabelCategory)newElement;

            if (CurrentCategory == newCategory) return;

            ResetUIElements(newCategory);

            //Added by Tomas
            Debug.Log("ChangeLabelCategory worked. Hopefully it syncs");
            PV.RPC("ChangeLabelCategorySync", RpcTarget.Others, newElement);
        }
        
    }

    private void ResetUIElements(LabelCategory newCategory)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
                CurrentLabel = null;
            }

            foreach (InteractionMuscleLabel label in InteractionLabelArray)
            {
                label.InteractionState = (label.GetLabelCategory == newCategory);
            }

            FadeTitleAndDescription(0.0f, 0.25f);

            foreach (LabelButton button in ButtonUI)
            {
                button.UIButton.interactable = (button.ButtonCategory != newCategory);
            }

            CurrentCategory = newCategory;
        }
        else
        {
            if (CurrentLabel != null)
            {
                CurrentLabel.UnSelect();
                CurrentLabel = null;
            }

            foreach (InteractionMuscleLabel label in InteractionLabelArray)
            {
                label.InteractionState = (label.GetLabelCategory == newCategory);
            }

            FadeTitleAndDescription(0.0f, 0.25f);

            foreach (LabelButton button in ButtonUI)
            {
                button.UIButton.interactable = (button.ButtonCategory != newCategory);
            }

            CurrentCategory = newCategory;
            PV.RPC("ResetUIElemSync", RpcTarget.Others, newCategory);

        }
    }

    private void FadeTitleAndDescription(float alpha, float time)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            TitleUI.CrossFadeAlpha(alpha, time, false);
            Debug.Log("From FadeTitleAndDescription: The alpha of " + TitleUI.name + " is: " + TitleUI.canvasRenderer.GetAlpha());
            DescriptionUI.CrossFadeAlpha(alpha, time, false);
            Debug.Log("From FadeTitleAndDescription: The alpha of " + DescriptionUI.name + " is: " + DescriptionUI.canvasRenderer.GetAlpha());
        }
        else
        {
            TitleUI.CrossFadeAlpha(alpha, time, false);
            Debug.Log("From FadeTitleAndDescription: The alpha of " + TitleUI.name + " is: " + TitleUI.canvasRenderer.GetAlpha());
            DescriptionUI.CrossFadeAlpha(alpha, time, false);
            Debug.Log("From FadeTitleAndDescription: The alpha of " + DescriptionUI.name + " is: " + DescriptionUI.canvasRenderer.GetAlpha());
            Debug.Log("FadeTitleAndDescription worked. Hopefully it syncs");
            PV.RPC("FadeTitleSync", RpcTarget.Others, alpha, time);
        }
    }

    private void SetTitleAndDescriptionAlpha(float alpha)
    {
        if (!PhotonNetwork.IsConnectedAndReady)
        {
            TitleUI.canvasRenderer.SetAlpha(alpha);
            DescriptionUI.canvasRenderer.SetAlpha(alpha);
        }
        else
        {
            TitleUI.canvasRenderer.SetAlpha(alpha);
            DescriptionUI.canvasRenderer.SetAlpha(alpha);
            PV.RPC("SetTitleSync", RpcTarget.Others, alpha);
        }
        
    }

    //Added by Tomas for syncing purposes
    [PunRPC]
    public void InitSectionSync()
    {
        Debug.Log("The RPC InitSectionSync was received at least");
        //InitiateSection();
        LabelLeg.SetActive(true);

        SetTitleAndDescriptionAlpha(0.0f);
        ResetUIElements(CurrentCategory);
        Debug.Log("InitSectionSync worked");
    }

    [PunRPC]
    public void TermSectionSync()
    {
        Debug.Log("The RPC TermSectionSync was received at least");
        //TerminateSection();
        if (CurrentLabel != null)
        {
            CurrentLabel.UnSelect();
        }

        LabelLeg.SetActive(false);
        Debug.Log("TermSectionSync worked");
    }

    [PunRPC]
    public void ChangeLabelSync(InteractionMuscleLabel newLabel)
    {
        Debug.Log("The RPC ChangeLabelSync was received at least");
        //ChangeLabel(newLabel);
        if (CurrentLabel == newLabel) return;

        if (CurrentLabel != null)
        {
            CurrentLabel.UnSelect();
        }

        SetTitleAndDescriptionAlpha(0.0f);
        TitleUI.text = newLabel.GetLabelTitle;
        DescriptionUI.text = newLabel.GetLabelDescription;
        Debug.Log(TitleUI.text);
        Debug.Log(DescriptionUI.text);
        FadeTitleAndDescription(1.0f, 0.5f);
        //roomNetworkManager.UpdateCurrentLabel(newLabel); //Important for syncing 
        CurrentLabel = newLabel;
        Debug.Log("ChangeLabelSync worked");
    }

    [PunRPC]
    public void ChangeLabelCategorySync(int NewElement)
    {
        Debug.Log("The ChangeLableCategorySync RPC was received at least");
        //ChangeLabelCategory(NewElement);
        LabelCategory newCategory = (LabelCategory)NewElement;

        if (CurrentCategory == newCategory) return;

        ResetUIElements(newCategory);
        Debug.Log("ChangeLabelCategorySync worked!");
    }

    [PunRPC]
    public void ResetUIElemSync(LabelCategory newCategory)
    {
        Debug.Log("The ResetUIElemSync RPC was received at least");
        //ResetUIElements(newCategory);
        if (CurrentLabel != null)
        {
            CurrentLabel.UnSelect();
            CurrentLabel = null;
        }

        foreach (InteractionMuscleLabel label in InteractionLabelArray)
        {
            label.InteractionState = (label.GetLabelCategory == newCategory);
        }

        FadeTitleAndDescription(0.0f, 0.25f);

        foreach (LabelButton button in ButtonUI)
        {
            button.UIButton.interactable = (button.ButtonCategory != newCategory);
        }

        CurrentCategory = newCategory;
        Debug.Log("ResetUIElemSync worked!");
    }

    [PunRPC]
    public void FadeTitleSync(float alpha, float time)
    {
        Debug.Log("The FadeTitleSync RPC was received at least");
        //FadeTitleAndDescription(alpha, time);
        TitleUI.CrossFadeAlpha(alpha, time, false);
        Debug.Log("From FadeTitleAndDescription: The alpha of " + TitleUI.name + " is: " + TitleUI.canvasRenderer.GetAlpha());
        DescriptionUI.CrossFadeAlpha(alpha, time, false);
        Debug.Log("From FadeTitleAndDescription: The alpha of " + DescriptionUI.name + " is: " + DescriptionUI.canvasRenderer.GetAlpha());
        Debug.Log("FadeTitleSync worked!");
    }

    [PunRPC]
    public void SetTitleSync(float alpha)
    {
        Debug.Log("The SetTitleSync RPC was received at least");
        //SetTitleAndDescriptionAlpha(alpha);
        TitleUI.canvasRenderer.SetAlpha(alpha);
        DescriptionUI.canvasRenderer.SetAlpha(alpha);
        Debug.Log("SetTitleSync worked!");
    }
}
