using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    private static ConnectionManager _instance;
    public static ConnectionManager Instance { get { return _instance; } }

    
    public List<RoomInfo> RoomList {  get; private set; }

    public bool IsConnected { get { return PhotonNetwork.IsConnectedAndReady; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            RoomList = new List<RoomInfo>();
        }
    }

    // Actions
    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }
    public void CreateRoom(RoomSettings settings)
    {
        PhotonNetwork.CreateRoom(settings.RoomName, settings.RoomOptions, TypedLobby.Default);
    }
    public void QuickPlay()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }
    // Callbacks
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log($"{PhotonNetwork.NickName} Connected to Master");
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log($"{PhotonNetwork.NickName} Joined Lobby " + PhotonNetwork.CurrentLobby);

    }
    public override void OnRoomListUpdate(List<RoomInfo> updatedRooms)
    {
        base.OnRoomListUpdate(updatedRooms);
        foreach(var room in updatedRooms)
        {
            // room exists in room list, hence was either updated or removed.
            if (RoomList.Contains(room))
            {
                if (room.RemovedFromList)
                    RoomList.Remove(room);
                else
                    RoomList[RoomList.FindIndex((r) => r.Name == room.Name)] = room;

            }
            else // New room to add to the list.
            {
                RoomList.Add(room);
            }
        }
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log($"Joined Room Successfuly, Room Name: {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        RoomSettings settings = new RoomSettings();
        settings.CreateRoomUsingSettings(this);
    }
    public override void OnLeftLobby()
    {
        // The game is only in one lobby, if by chance the player got removed from the lobby, they will disconnect from the server and then have a chance to reconnect again.
        base.OnLeftLobby();
        PhotonNetwork.Disconnect();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Disconnected from Photon");
    }





}
