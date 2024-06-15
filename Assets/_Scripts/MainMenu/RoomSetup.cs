using Photon.Realtime;
using System;
using System.Collections;
using UnityEngine;

public enum Maps
{
    Random,
    Graveyard,
    B,
    C
}

public enum WeaponSpawnPattern
{
    Normal,
    None,
    LaserOnly
}


[CreateAssetMenu(fileName = "RoomSetup", menuName = "Scriptable Objects/RoomSetup")]
public class RoomSetup : ScriptableObject
{
    public RoomOptions RoomOptions { get; private set; }
    [field: SerializeField] public string RoomID { get; private set; } = "R";

    [Tooltip("If a client disconnects, this actor is inactive first and removed after this timeout. In milliseconds.")]
    [SerializeField] private int _playersTtl = 5_000;
    
    public RoomProperties RoomProperties { get; set; }

    public RoomSetup(string roomID)
    {
        RoomID = roomID;
        RoomProperties = new RoomProperties();
        RoomOptions = new RoomOptions();
        RoomOptions.PlayerTtl = _playersTtl;
        RoomOptions.EmptyRoomTtl = 0;
        RoomOptions.MaxPlayers = RoomProperties.PlayerCount;
        RoomOptions.CustomRoomProperties = RoomProperties.Hashtable;
    }

    public static RoomSetup CreateRoomInstance(string roomID)
    {
        var data = CreateInstance<RoomSetup>();
        data.RoomID = roomID;
        data.Init(data);
        return data;
    }

    private  void Init(RoomSetup data)
    {
        data.RoomProperties = new RoomProperties();
        data.RoomOptions = new RoomOptions();
        data.RoomOptions.PlayerTtl = _playersTtl;
        data.RoomOptions.EmptyRoomTtl = 0;
        data.RoomOptions.MaxPlayers = RoomProperties.PlayerCount;
        data.RoomOptions.CustomRoomProperties = RoomProperties.Hashtable;
    }
}
