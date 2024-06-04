

using Photon.Realtime;
using UnityEngine;

public class RoomProperties
{
    public ExitGames.Client.Photon.Hashtable Hashtable { get; private set; }

    private int _playerCount = 4;
    private Maps _map = Maps.Random;
    private int _matchTime = 120;
    private int _scoreGoal = 0;
    private string _name = "Match";
    private WeaponSpawnPattern _weaponSpawnPattern = WeaponSpawnPattern.Normal;

    
    public RoomProperties()
    {
        Hashtable = new ExitGames.Client.Photon.Hashtable()
        {
            { "m", _map },
            { "t", _matchTime },
            { "n", _name },
            { "s", _scoreGoal },
            { "w", _weaponSpawnPattern },
            { "p", _playerCount }
        };
    }

    public RoomProperties(ExitGames.Client.Photon.Hashtable hashtable)
    {
        Hashtable = hashtable;
    }


    public int PlayerCount
    {
        get { return _playerCount; }
        set
        {
            _playerCount = Mathf.Clamp(value, 1, 6);
            Hashtable["p"] = _playerCount;
        }
    }
    public Maps Map
    {
        get { return _map; }
        set
        {
            _map = value;
            Hashtable["m"] = _map;
        }
    }
    public int MatchTime
    {
        get { return _matchTime; }
        set
        {
            _matchTime = value;
            Hashtable["t"] = _matchTime;
        }
    }
    public int ScoreGoal
    {
        get { return _scoreGoal; }
        set
        {
            _scoreGoal = value;
            Hashtable["s"] = _scoreGoal;
        }
    }
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            Hashtable["n"] = _name;
        }
    }
    public WeaponSpawnPattern WeaponSpawnPattern
    {
        get { return _weaponSpawnPattern; }
        set
        {
            _weaponSpawnPattern = value;
            Hashtable["w"] = _weaponSpawnPattern;
        }   
    }




}
