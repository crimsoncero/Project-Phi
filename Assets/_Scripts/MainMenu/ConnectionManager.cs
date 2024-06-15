using UnityEngine;
using Photon;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System;
using System.Net.NetworkInformation;
using System.Linq;

public class ConnectionManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private RoomSetup _quickplaySetup;
    [SerializeField] private GameObject RoomListObject;
    [SerializeField] private Transform RoomListGrid;

    public List<RoomInfo> RoomList { get; private set; }
    public List<RoomListObject> roomListObjects { get; private set; }


    public bool IsConnected { get; private set; } = false;

    private void Awake()
    {
        RoomList = new List<RoomInfo>();
        roomListObjects = new List<RoomListObject>();

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
                {
                    // Remove room from list if not listed anymore.
                    RoomList.Remove(room);
                    int index = roomListObjects.FindIndex((r) => r.RoomName == room.Name);
                    Destroy(roomListObjects[index].gameObject);
                    roomListObjects.Remove(roomListObjects[index]);
                }
                else
                {
                    // Update existing room
                    RoomList[RoomList.FindIndex((r) => r.Name == room.Name)] = room;
                    roomListObjects[roomListObjects.FindIndex((r) => r.RoomName == room.Name)].updateData(room);
                }
            
            }
            else
            {
                // Add the new room to the list.
                RoomList.Add(room);
                GameObject newRoomObject = Instantiate(RoomListObject, RoomListGrid);
                RoomListObject objectRef = newRoomObject.GetComponent<RoomListObject>();
                objectRef.RoomName = room.Name;
                objectRef.manager = this;
                objectRef.updateData(room);
                roomListObjects.Add(objectRef);
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

        Room room = PhotonNetwork.CurrentRoom;
        RoomProperties props = new RoomProperties(room.CustomProperties);

        Debug.Log("Match Properties:");
        Debug.Log("ID: " + room.Name);
        Debug.Log($"Name: {props.Name}");
        Debug.Log($"Map: {props.Map}");
        Debug.Log($"Player Count: {props.PlayerCount}");
        Debug.Log($"Match Time: {props.MatchTime}");
        Debug.Log($"Score Goal: {props.ScoreGoal}");
        Debug.Log($"Spawn Pattern: {props.WeaponSpawnPattern}");
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
        PhotonNetwork.CreateRoom(AssignRoomID(), _quickplaySetup.RoomOptions);
    }





    /// <summary>
    /// Generate RoomID by using the DateTime.Now and concatenating it with
    /// the macAddress, then returning its Hash Code
    /// </summary>
    /// <returns></returns>
    public string AssignRoomID()
    {
        string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string macAddress = GetMacAddress();

        if (macAddress == null)
        {
            Debug.LogError("Failed to retrieve MAC address.");
            return "ERROR";
        }

        string concatenatedString = dateTimeString + macAddress;
        return Math.Abs(concatenatedString.GetHashCode()).ToString();
    }

    private string GetMacAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var networkInterface in networkInterfaces)
        {
            if (networkInterface.OperationalStatus != OperationalStatus.Up)
            {
                continue;
            }
            var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
            if (addressBytes.Length == 6)
            {
                return string.Join(":", addressBytes.Select(b => b.ToString("X2")));
            }
        }
        return null;
    }



}
