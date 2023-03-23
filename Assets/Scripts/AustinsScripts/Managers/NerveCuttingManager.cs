using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public enum NerveCategories { Median_Ulnar, Musculocutaneous, Radial, Suprascapular };

[System.Serializable]
public struct Nerves
{
    public string Name;
    public NerveCategories NerveCategory;
    public SkinnedMeshRenderer NerveMesh;
    public SwapTextures NerveTextures;
    public GameObject NerveColliders;
    public Button NerveButton;
}

public class NerveCuttingManager : Manager
{
    [Header("UI Settings")]
    [SerializeField] GameObject CentralPanel;
    [SerializeField] Toggle HealthyLegToggle;
    [SerializeField] Slider OpacitySlider;
    [Header("Canine Legs")]
    [SerializeField] GameObject HealthyLeg;
    [SerializeField] GameObject NerveCuttingLeg;
    [Header("Nerve Cutting Settings")]
    [SerializeField] NerveCategories CurrentNerveCategory;
    [SerializeField] Nerves[] NerveArray;
    [SerializeField] legAnimController LegAnimController;
    public PhotonView PV; //Added by Tomas

    Dictionary<NerveCategories, Nerves> NerveDictionary;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        NerveDictionary = new Dictionary<NerveCategories, Nerves>();

        foreach(Nerves nerve in NerveArray)
        {
            NerveDictionary.Add(nerve.NerveCategory, nerve);
            ChangeNerveVisibility(nerve, (nerve.NerveCategory == CurrentNerveCategory));
        }
        if (PV != null)
        {
            Debug.Log("NerveCuttingManager has a PV!");
        }
    }

    public override void InitiateSection()
    {
        ChangeVisibility(true);
        PV.RPC("InitSectSync", RpcTarget.Others);
    }

    public override void TerminateSection()
    {
        ChangeVisibility(false);
        PV.RPC("TermSectSync", RpcTarget.Others);
    }

    void ChangeVisibility(bool IsVisible)
    {
        if (IsVisible && HealthyLegToggle.isOn)
            CentralPanel.SetActive(true);
        else
            CentralPanel.SetActive(false);

        NerveCuttingLeg.SetActive(IsVisible);

        if (!IsVisible)
            OpacitySlider.value = OpacitySlider.maxValue;
    }

    public void ChangeNerveCategory(int newNerveElement)
    {
        NerveCategories newCategory = (NerveCategories)newNerveElement;
        Nerves oldNerve;
        Nerves newNerve;

        if (newCategory == CurrentNerveCategory)
            return;

        if (NerveDictionary.TryGetValue(CurrentNerveCategory, out oldNerve) && NerveDictionary.TryGetValue(newCategory, out newNerve))
        {
            ChangeNerveVisibility(oldNerve, false);
            ChangeNerveVisibility(newNerve, true);
        }

        CurrentNerveCategory = newCategory;
        LegAnimController.setHealthy();

        PV.RPC("ChangeNerveCatSync", RpcTarget.Others, newNerveElement);
    }

    void ChangeNerveVisibility(Nerves nerve, bool IsVisible)
    {
        nerve.NerveMesh.enabled = IsVisible;
        nerve.NerveColliders.SetActive(IsVisible);
        nerve.NerveButton.interactable = !IsVisible;
        nerve.NerveTextures.setHealthyTexture();
    }

    public void ChangeCentralPanelVisibility(bool isVisible)
    {
        if (isVisible && HealthyLegToggle.isOn)
        {
            CentralPanel.SetActive(true);
        }
        else
        {
            CentralPanel.SetActive(false);
        }

        if (PV.IsMine)
        {

        }
    }

    //Added by Tomas for syncing purposes
    [PunRPC]
    public void InitSectSync()
    {
        Debug.Log("InitSectSync of NerveCuttingManager has been activated.");
        ChangeVisibility(true);
        Debug.Log("InitSectSync of NerveCuttingManager worked!");
    }

    [PunRPC]
    public void TermSectSync()
    {
        Debug.Log("TermSectSync of NerveCuttingManager has been activated.");
        ChangeVisibility(false);
        Debug.Log("TermSectSync of NerveCuttingManager worked!");
    }

    [PunRPC]
    public void ChangeNerveCatSync(int newNerveElem)
    {
        Debug.Log("ChangeNerveCatSync of NerveCuttingManager has been activated.");
        //ChangeNerveCategory(newNerveElem);
        NerveCategories newCategory = (NerveCategories)newNerveElem;
        Nerves oldNerve;
        Nerves newNerve;

        if (newCategory == CurrentNerveCategory)
            return;

        if (NerveDictionary.TryGetValue(CurrentNerveCategory, out oldNerve) && NerveDictionary.TryGetValue(newCategory, out newNerve))
        {
            ChangeNerveVisibility(oldNerve, false);
            ChangeNerveVisibility(newNerve, true);
        }

        CurrentNerveCategory = newCategory;
        LegAnimController.setHealthy();
        Debug.Log("ChangeNerveCatSync of NerveCuttingManager has worked!");
    }

    [PunRPC]
    public void ChangeNerveVisibilitySync(string nerveName, bool isVisible)
    {

    }

    [PunRPC]
    public void ChangeCentralPanelVisibilitySync(bool isVisible)
    {

    }
}
