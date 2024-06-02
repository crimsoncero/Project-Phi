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
    
    [Header("Room Properties")]
    [SerializeField, Range(1, 6)] private int _playerCount = 4;
    [SerializeField] private Maps _map = Maps.Random;
    [SerializeField] private int _matchTime = 120;
    [SerializeField] private int _scoreGoal = 0;
    [SerializeField] private string _name = "Match";
    [SerializeField] private WeaponSpawnPattern _weaponSpawnPattern = WeaponSpawnPattern.Normal;

    public int PlayerCount
    {
        get { return _playerCount; }
        set
        {
            _playerCount = Mathf.Clamp(value, 1, 6);
            RoomOptions.MaxPlayers = _playerCount;
        }
    }
    public Maps Map
    {
        get { return _map; }
        set
        {
            _map = value;
            RoomOptions.CustomRoomProperties["m"] = _map;
        }
    }
    public int MatchTime
    {
        get { return _matchTime; }
        set
        {
            _matchTime = value;
            RoomOptions.CustomRoomProperties["t"] = _matchTime;
        }
    }
    public int ScoreGoal
    {
        get { return _scoreGoal; }
        set
        {
            _scoreGoal = value;
            RoomOptions.CustomRoomProperties["s"] = _scoreGoal;
        }
    }
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            RoomOptions.CustomRoomProperties["n"] = _name;
        }
    }
    public WeaponSpawnPattern WeaponSpawnPattern
    {
        get { return _weaponSpawnPattern; }
        set
        {
            _weaponSpawnPattern = value;
            RoomOptions.CustomRoomProperties["w"] = _weaponSpawnPattern;
        }
    }

    public RoomSetup(string roomID)
    {
        RoomID = roomID;
        
        RoomOptions = new RoomOptions();
        RoomOptions.PlayerTtl = _playersTtl;
        RoomOptions.EmptyRoomTtl = 0;
        RoomOptions.MaxPlayers = _playerCount;
        SetHashtable();
    }

    private void SetHashtable()
    {
        RoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()
        {
            { "m", _map },
            { "t", _matchTime },
            { "n", _name },
            { "s", _scoreGoal },
            { "w", _weaponSpawnPattern }
        };
    }





}
