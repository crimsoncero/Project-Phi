using UnityEngine;
using Photon;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;

public class ConnectionManager : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom("A Room");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        Debug.Log("Room created successfully!");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinRoom(string roomID)
    {
        PhotonNetwork.JoinRoom(roomID);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
    }



}
