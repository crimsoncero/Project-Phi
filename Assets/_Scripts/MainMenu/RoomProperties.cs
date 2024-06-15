

using Photon.Realtime;
using System;
using UnityEngine;

[Serializable]
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
        get { return (int)Hashtable["p"]; }
        set
        {
            _playerCount = Mathf.Clamp(value, 1, 6);
            Hashtable["p"] = _playerCount;
        }
    }
    public Maps Map
    {
        get { return (Maps)Hashtable["m"]; }
        set
        {
            _map = value;
            Hashtable["m"] = _map;
        }
    }
    public int MatchTime
    {
        get { return (int)Hashtable["t"]; }
        set
        {
            _matchTime = value;
            Hashtable["t"] = _matchTime;
        }
    }
    public int ScoreGoal
    {
        get { return (int)Hashtable["s"]; }
        set
        {
            _scoreGoal = value;
            Hashtable["s"] = _scoreGoal;
        }
    }
    public string Name
    {
        get { return (string)Hashtable["n"]; }
        set
        {
            _name = value;
            Hashtable["n"] = _name;
        }
    }
    public WeaponSpawnPattern WeaponSpawnPattern
    {
        get { return (WeaponSpawnPattern)Hashtable["w"]; }
        set
        {
            _weaponSpawnPattern = value;
            Hashtable["w"] = _weaponSpawnPattern;
        }   
    }




}
