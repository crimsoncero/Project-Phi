using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public event Action OnUpdatedRoomList;

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

    private void RegisterCustomTypes()
    {
        PhotonPeer.RegisterType(typeof(HitData), (byte)'H', HitData.SerializeHitData, HitData.DeserializeHitData);
        PhotonPeer.RegisterType(typeof(EndGamePlayerData), (byte)'E', EndGamePlayerData.Serialize, EndGamePlayerData.Deserialize);
    }


    // General settings to setup when connecting to the game.
    private void SettingsSetup()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.LocalPlayer.CustomProperties = PlayerProperties.Init();
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
    public void LeaveRoom(bool becomeInactive)
    {
        PhotonNetwork.LeaveRoom(becomeInactive);
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

        RegisterCustomTypes();
    }
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log($"{PhotonNetwork.NickName} Joined Lobby " + PhotonNetwork.CurrentLobby);
        RoomList = new List<RoomInfo>();
        SettingsSetup();

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

        OnUpdatedRoomList?.Invoke();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PlayerPrefsHandler.SetRoomId(PhotonNetwork.CurrentRoom.Name);
        Debug.Log($"Joined Room Successfuly, Room Name: {PhotonNetwork.CurrentRoom.Name}");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room:\n return code: " + returnCode + "\nmessage: " + message);
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
