
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// An extension class to player to help set and retrieve custom properties.
/// </summary>
public static class PlayerProperties
{
    public const string SHIP_CONFIG = "c";
    public const string KILLS = "k";

    public static bool SetShipConfigID(this Player player, int id)
    {
        if (player == null) return false;
        if (id < 0) return false;

        Hashtable t = new() { { SHIP_CONFIG, id } };


        return player.SetCustomProperties(t);
    }
    public static int GetShipConfigID(this Player player)
    {
        if (player == null) return -1;

        if (player.CustomProperties.ContainsKey(SHIP_CONFIG))
            return (int) player.CustomProperties[SHIP_CONFIG];
        
        return -1;
    }

    public static bool SetPlayerKills(this Player player, int kills)
    {
        if (player == null) return false;
        if(kills < 0) return false;

        Hashtable t = new() { { KILLS, kills } };

        return player.SetCustomProperties(t);
    }

    public static int GetPlayerKills(this Player player)
    {
        if (player.CustomProperties.ContainsKey(KILLS))
            return (int)player.CustomProperties[KILLS];

        return -1;
    }
    public static Hashtable Init()
    {
        Hashtable t = new()
        {
            { SHIP_CONFIG, -1 },
            { KILLS, 0}
        };

        return t;
    }
}
