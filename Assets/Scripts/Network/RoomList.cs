using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField]
    private NetworkManager NetworkManager;
    public GameObject RoomListItemPrefab;
    private RoomListItem SelectedRoom;
    [SerializeField]
    private Button JoinButton;

    [Header("List Settings")]
    public bool ShowFullRooms = true;
    public void SetShowFullRooms(bool show) { ShowFullRooms = show; ChangeFilters(); }

    public enum SortType
    {
        None,
        Name
    }
    public SortType SortBy = SortType.None;

    private void OnEnable()
    {
        Reset();
        Deselect();
    }

    public void Deselect()
    {
        if (SelectedRoom != null)
        {
            SelectedRoom.Deselect();
            SelectedRoom = null;
        }
        if (JoinButton != null)
            JoinButton.interactable = false;
    }

    public void Clear()
    {
        foreach (Transform child in gameObject.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void Reset()
    {
        Deselect();
        Clear();
        FillList();
    }

    private void FillList()
    {
        if (NetworkManager != null)
        {
            List<RoomInfo> Rooms = NetworkManager.GetRooms();
            foreach (RoomInfo RoomInfo in Rooms)
            {
                CreateRoomListItem(RoomInfo);
            }
            Sort();
        }
    }

    private void CreateRoomListItem(RoomInfo RoomInfo)
    {
        if (RoomListItemPrefab != null && ShouldList(RoomInfo))
        {
            RoomListItem RoomListItem = (Instantiate(RoomListItemPrefab, gameObject.transform) as GameObject).GetComponent<RoomListItem>();
            if (RoomListItem != null)
            {
                RoomListItem.Populate(RoomInfo);
            }
        }
    }
    public void AddRoom(RoomInfo RoomInfo)
    {
        CreateRoomListItem(RoomInfo);
        Sort();
    }

    private void UpdateRoomListItem(RoomInfo RoomInfo)
    {
        List<RoomListItem> Items = gameObject.GetComponentsInChildren<RoomListItem>().ToList();
        foreach (RoomListItem Item in Items)
        {
            if (Item.Name == RoomInfo.Name)
            {
                Item.Populate(RoomInfo);
                break;
            }
        }
    }
    public void UpdateRoom(RoomInfo RoomInfo)
    {
        UpdateRoomListItem(RoomInfo);
        Sort();
    }

    private void DestroyRoomListItem(RoomInfo RoomInfo)
    {
        List<RoomListItem> Items = gameObject.GetComponentsInChildren<RoomListItem>().ToList();
        foreach (RoomListItem Item in Items)
        {
            if (Item.Name == RoomInfo.Name)
            {
                Destroy(Item.gameObject);
                break;
            }
        }
    }
    public void RemoveRoom(RoomInfo RoomInfo)
    {
        DestroyRoomListItem(RoomInfo);
    }

    private void Sort()
    {
        if (SortBy != SortType.None)
        {
            List<RoomListItem> Items = gameObject.GetComponentsInChildren<RoomListItem>().ToList();
            List<RoomListItem> SortedItems = Items;

            if (SortBy == SortType.Name)
            {
                SortedItems = Items.OrderBy(item => item.Name).ToList();
            }

            for (int i = 0; i < SortedItems.Count; i++)
            {
                SortedItems[i].gameObject.transform.SetSiblingIndex(i);
            }
        }
    }

    private bool ShouldList(RoomInfo RoomInfo)
    {
        // check filters
        if (!ShowFullRooms && (RoomInfo.PlayerCount == (int)RoomInfo.MaxPlayers))
            return false;
        
        return true;
    }

    public void ChangeFilters()
    {
        string PreviousSelection = SelectedRoom.Name;
        Reset();
        List<RoomListItem> Items = gameObject.GetComponentsInChildren<RoomListItem>().ToList();
        foreach (RoomListItem Item in Items)
        {
            if (Item.Name == PreviousSelection)
            {
                Item.Select();
                break;
            }
        }
    }

    public string GetSelectedRoom()
    {
        if (SelectedRoom != null)
        {
            return SelectedRoom.Name;
        }
        return "";
    }

    public void SelectRoom(RoomListItem Room)
    {
        if (SelectedRoom != null)
            SelectedRoom.Deselect();
        SelectedRoom = Room;
        if (JoinButton != null)
            JoinButton.interactable = true;
    }
}
