using Photon.Realtime;
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







}
