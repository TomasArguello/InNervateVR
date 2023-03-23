using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    private RoomList RoomList;
    private bool Selected;
    [SerializeField]
    private Button SelectButton;

    public string Name { get; private set; }
    private RoomInfo Info;
    private bool Full;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI RoomName;
    [SerializeField]
    private TextMeshProUGUI PlayerCount;

    [Header("Visualization")]
    public GameObject SelectedVisual;
    public GameObject DisabledVisual;
    public bool LowerTextOpacityOnDisable = true;
    public byte DefaultOpacity = (byte) 255;
    public byte DisabledOpacity = (byte) 127;

    private void Awake()
    {
        RoomList = gameObject.GetComponentInParent<RoomList>(true);
        SelectButton.onClick.AddListener(Select);
        Deselect();
        Enable();
    }

    public void Populate(RoomInfo RoomInfo)
    {
        // update stored properties
        Info = RoomInfo;
        Name = RoomInfo.Name;
        Full = (RoomInfo.PlayerCount == RoomInfo.MaxPlayers);

        // update UI listing
        RoomName.text = Name;
        PlayerCount.text = ((int)RoomInfo.PlayerCount).ToString() + "/" + ((int)RoomInfo.MaxPlayers).ToString();

        // check if room is available to join
        if (Full || !RoomInfo.IsOpen)
        {
            Disable();
        }
        else
        {
            Enable();
        }
    }

    public void Select()
    {
        if (RoomList != null)
        {
            RoomList.SelectRoom(this);
            Selected = true;
            if (SelectedVisual != null)
                SelectedVisual.SetActive(true);
        }
    }

    public void Deselect()
    {
        Selected = false;
        if (SelectedVisual != null)
            SelectedVisual.SetActive(false);
    }

    public void Enable()
    {
        SelectButton.interactable = true;
        if (DisabledVisual != null)
            DisabledVisual.SetActive(false);
        if (LowerTextOpacityOnDisable)
        {
            RoomName.color = new Color32(RoomName.faceColor.r, RoomName.faceColor.g, RoomName.faceColor.b, DefaultOpacity);
        }
    }

    public void Disable()
    {
        if (Selected)
            RoomList.Deselect();
        
        SelectButton.interactable = false;
        if (DisabledVisual != null)
            DisabledVisual.SetActive(true);
        if (LowerTextOpacityOnDisable)
        {
            RoomName.color = new Color32(RoomName.faceColor.r, RoomName.faceColor.g, RoomName.faceColor.b, DisabledOpacity);
        }
    }
}
