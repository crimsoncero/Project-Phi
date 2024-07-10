using Photon.Pun;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public enum WeaponEnum
{
    Lazgun = 0,
    Autocannon = 1,
    RocketPod = 2,
    MineDispenser = 3,
    DoomLaser = 4,
}

[CreateAssetMenu(fileName = "WeaponList", menuName = "Scriptable Objects/Weapons/WeaponList")]
public class WeaponList : ScriptableObject
{
    [SerializeField] private Lazgun _lazgun;
    
    [SerializeField] private Autocannon _autocannon;
    [SerializeField] private int _autocannonSpawnWeight;
    
    [SerializeField] private RocketPod _rocketPod;
    [SerializeField] private int _rocketPodSpawnWeight;
    
    [SerializeField] private ProximityMine _mineDispenser;
    [SerializeField] private int _mineDispenserSpawnWeight;
   
    [SerializeField] private DoomLaser _doomLaser;
    [SerializeField] private int _doomLaserSpawnWeight;


 

    public Weapon GetWeapon(WeaponEnum weaponEnum)
    {
        switch (weaponEnum)
        {
            case WeaponEnum.Lazgun:
                return _lazgun;
            case WeaponEnum.Autocannon:
                return _autocannon;
            case WeaponEnum.RocketPod:
                return _rocketPod;
            case WeaponEnum.MineDispenser:
                return _mineDispenser;
            case WeaponEnum.DoomLaser:
                return _doomLaser;
            default:
                return null;
        }
    }

    public WeaponEnum GetWeaponEnum(Weapon weapon)
    {
        switch(weapon)
        {
            case Lazgun:
                return WeaponEnum.Lazgun;
            case Autocannon:
                return WeaponEnum.Autocannon;
            case RocketPod:
                return WeaponEnum.RocketPod;
            case ProximityMine:
                return WeaponEnum.MineDispenser;
            case DoomLaser:
                return WeaponEnum.DoomLaser;
            default:
                throw new NotImplementedException("Need to add the new weapon class to the list.");
        }
    }

    /// <summary>
    /// Returns a random weapon from the list using the weights specified in the SO.
    /// </summary>
    /// <returns></returns>
    public Weapon GetRandomWeapon()
    {
        if (!PhotonNetwork.IsMasterClient) return null; // only the master client can do random operations.
        
        int totalWeight = _autocannonSpawnWeight + _rocketPodSpawnWeight + _mineDispenserSpawnWeight + _doomLaserSpawnWeight;
        int autoCannonMax = _autocannonSpawnWeight;
        int rocketPodMax = _rocketPodSpawnWeight + autoCannonMax;
        int mineMax = _mineDispenserSpawnWeight + rocketPodMax;
        int doomLaserMax = _doomLaserSpawnWeight + mineMax;


        int randomNum = Random.Range(0,totalWeight);

        if (randomNum < autoCannonMax)
            return _autocannon;
        else if (randomNum < rocketPodMax)
            return _rocketPod;
        else if (randomNum < mineMax)
            return _mineDispenser;
        else if (randomNum < doomLaserMax)
            return _doomLaser;
        else
            throw new Exception("Value out of range");

    }

    public WeaponEnum GetRandomWeaponEnum() => GetWeaponEnum(GetRandomWeapon());

}
