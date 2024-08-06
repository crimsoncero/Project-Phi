using UnityEngine;

/// <summary>
/// A class to control the player prefs in the game.
/// </summary>
public static class PlayerPrefsHandler
{
    public const string NameKey = "Name";
    public const string RoomIdKey = "RoomID";
    public const string GuidKey = "Guid";

    public static bool HasGuidKey()
    {
        return PlayerPrefs.HasKey(GuidKey);
    }
    public static void SetGuid(string name)
    {
        PlayerPrefs.SetString(GuidKey, name);
    }
    public static string GetGuid()
    {
        if (HasGuidKey())
            return PlayerPrefs.GetString(GuidKey);
        else
            return "";
    }

    public static bool HasNameKey()
    {
        return PlayerPrefs.HasKey(NameKey);
    }
    public static void SetName(string name)
    {
        PlayerPrefs.SetString(NameKey, name);
    }
    public static string GetName()
    {
        if(HasNameKey())
            return PlayerPrefs.GetString(NameKey);
        else
            return "";
    }

    public static bool HasRoomIdKey()
    {
        return PlayerPrefs.HasKey(RoomIdKey);
    }
    public static void SetRoomId(string roomID)
    {
        PlayerPrefs.SetString(RoomIdKey, roomID);
    }
    public static string GetRoomId()
    {
        if(HasRoomIdKey())
            return PlayerPrefs.GetString(RoomIdKey);
        return "";
    }



}
