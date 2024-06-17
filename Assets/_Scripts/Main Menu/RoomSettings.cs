
using Photon.Realtime;
using SeraphUtil;
using System;
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

public class RoomSettings
{
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
        RoomName = GenerateRoomName();
        RoomProperties = new RoomProperties();
        RoomOptions = new RoomOptions();
        MaxPlayers = 4;
        RoomOptions.PlayerTtl = 5000;

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

    private string GenerateRoomName()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string macAddress = Utility.GetMacAddress();
        if(string.IsNullOrEmpty(macAddress))
        {
            macAddress = "Error";
        }

        string str = dateTime + macAddress;
        return str.GetHashCode().ToString();
    }


}




