using UnityEngine;
using TMPro;
using Photon;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System;
using System.Net.NetworkInformation;
using System.Linq;

public class RoomListObject : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI Name;
    [SerializeField] TextMeshProUGUI ID;
    [SerializeField] TextMeshProUGUI PlayerCount;

    private string roomName;
    public string RoomName { get { return roomName; } set { roomName = value; } }
    public ConnectionManager manager; 
    public void updateData(RoomInfo roomInfo)
    {
        RoomProperties props = new RoomProperties(roomInfo.CustomProperties);
        Name.text = props.Name;
        PlayerCount.text = roomInfo.PlayerCount +" / " +roomInfo.MaxPlayers;
        ID.text = roomInfo.Name; 
    }

    public void JoinRoom()
    {
        manager.DirectJoin(roomName);
    }
}
