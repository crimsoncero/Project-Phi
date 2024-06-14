using UnityEngine;
using Photon;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomSetup _quickplaySetup;

    public List<RoomInfo> RoomList { get; private set; }


    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        RoomList = new List<RoomInfo>();
    }
    private void Start()
    {
        //Connect();
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        IsConnected = true;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("Connected to " + PhotonNetwork.CurrentLobby);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        UpdateRoomList(roomList);
        foreach(RoomInfo room in RoomList)
        {
            Debug.Log(room.Name);
        }
        
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

    public void CreateRoom(RoomSetup roomSetup)
    {
        PhotonNetwork.CreateRoom(roomSetup.RoomID, roomSetup.RoomOptions);
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
        PhotonNetwork.JoinRandomRoom();
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
        Debug.Log("Failed Joining Random");
        PhotonNetwork.CreateRoom(_quickplaySetup.RoomID, _quickplaySetup.RoomOptions);
    }

    


}
