using System;
using UnityEngine;

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
    [SerializeField] private RocketPod _rocketPod;
    [SerializeField] private ProximityMine _mineDispenser;
    [SerializeField] private DoomLaser _doomLaser;


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
}
