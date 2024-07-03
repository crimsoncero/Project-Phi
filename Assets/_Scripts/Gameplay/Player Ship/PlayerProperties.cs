
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// An extension class to player to help set and retrieve custom properties.
/// </summary>
public static class PlayerProperties
{
    public const string SHIP_CONFIG = "c";

    public static bool SetShipConfigID(this Player player, int id)
    {
        if (player == null) return false;
        if (id < 0) return false;

        Hashtable t = new Hashtable() { { SHIP_CONFIG, id } };

        return player.SetCustomProperties(t);
    }
    public static int GetShipConfigID(this Player player)
    {
        if (player.CustomProperties.ContainsKey(SHIP_CONFIG))
            return (int) player.CustomProperties[SHIP_CONFIG];
        
        return -1;
    }

    public static Hashtable Init()
    {
        Hashtable t = new Hashtable()
        {
            { SHIP_CONFIG, -1 },
        };

        return t;
    }
}
