    
using Photon.Realtime;
using SeraphUtil;
using System;
using UnityEngine;

public enum Maps
{
    Random = 0,
    Graveyard = 1,
    AsteroidField = 2,
}

public enum WeaponSpawnPattern
{
    All = 0,
    PrimaryOnly = 1,
}

public class RoomSettings
{
    public static readonly int MAXPLAYERS = 6;
    public static readonly int MINPLAYERS = 1;

    public RoomOptions RoomOptions {  get; private set; }
    public string RoomName { get; private set; }
    
    public RoomProperties RoomProperties { get; private set; }

    public int MaxPlayers
    {
        get { return RoomOptions.MaxPlayers; }
        set { RoomOptions.MaxPlayers = value; }
    }

    public RoomSettings()
    {
        GenerateRoomName();
        RoomProperties = new RoomProperties();
        RoomOptions = new RoomOptions();
        MaxPlayers = 4;
        RoomOptions.PlayerTtl = 0;
        RoomOptions.EmptyRoomTtl = 0;
        RoomOptions.PublishUserId = true;
        RoomOptions.CustomRoomPropertiesForLobby = new string[]{RoomProperties.MAP_PROP_KEY} ;
        RoomOptions.CleanupCacheOnLeave = false;
        UpdateRoomProperties();

    }

    public void UpdateRoomProperties()
    {
        RoomOptions.CustomRoomProperties = RoomProperties.Hashtable;
        RoomOptions.CustomRoomPropertiesForLobby = RoomProperties.Hashtable.GetStringKeys().ToArray();
    }

    public void CreateRoomUsingSettings(ConnectionManager con)
    {
        UpdateRoomProperties();
        con.CreateRoom(this);
    }

    public string GenerateRoomName()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string macAddress = Utility.GetMacAddress();
        if(string.IsNullOrEmpty(macAddress))
        {
            macAddress = "Error";
        }

        string str = dateTime + macAddress;
        RoomName =  Mathf.Abs(str.GetHashCode()).ToString();
        return RoomName;
    }


}




