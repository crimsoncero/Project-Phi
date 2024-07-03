using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ship Config List", menuName = "Scriptable Objects/Ship/Ship Config List")]
public class ShipConfigList : ScriptableObject
{
    [field: SerializeField] public List<SpaceshipConfig> ConfigList { get; private set; }

    public SpaceshipConfig GetConfig(int ConfigID)
    {
        return ConfigList.Find((c) => c.ID == ConfigID);
    }

    /// <summary>
    /// Retrieves the config of the player.
    /// </summary>
    /// <returns></returns>
    public SpaceshipConfig GetPlayerConfig(Player player)
    {
        return GetConfig(player.GetShipConfigID());
    }
}
