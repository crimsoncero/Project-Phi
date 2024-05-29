using UnityEngine;
using Photon;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public List<RoomInfo> RoomList { get; private set; }


    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        RoomList = new List<RoomInfo>();
    }
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        IsConnected = true;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        UpdateRoomList(roomList);
        
    }
    private void UpdateRoomList(List<RoomInfo> updatedRooms)
    {
        foreach (RoomInfo room in updatedRooms)
        {
            if (RoomList.Contains(room))
            {
                if (room.RemovedFromList)
                    // Remove room from list if not listed anymore.
                    RoomList.Remove(room);
                else
                    // Update existing room
                    RoomList[RoomList.FindIndex((r) => r.Name == room.Name)] = room;
            }
            else
            {
                // Add the new room to the list.
                RoomList.Add(room);
            }
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(System.DateTime.Now.ToString());
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created successfully!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("Creating room failed");
    }

    public void Quickplay()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void DirectJoin(string roomID)
    {
        PhotonNetwork.JoinRoom(roomID);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined room");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("failed joining room");

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    



}
