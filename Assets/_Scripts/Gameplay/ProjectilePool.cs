using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{

    private static ProjectilePool _instance;
    public static ProjectilePool Instance { get { return _instance; } }

    [Header("Lazgun Settings:")]
    [SerializeField] private Projectile _lazerPrefab;
    [SerializeField] private Transform _lazerContainer;
    [SerializeField] private int _lazgunInitialSize;
    public ObjectPool<Projectile> LazgunPool{ get; private set; }

    [Header("Autocannon Settings:")]
    [SerializeField] private Projectile _autocannonPrefab;
    [SerializeField] private Transform _autocannonContainer;
    [SerializeField] private int _autocannonInitialSize;
    public ObjectPool<Projectile> AutocannonPool { get; private set; }

    [Header("Rocket Pod Settings:")]
    [SerializeField] private Projectile _rocketPrefab;
    [SerializeField] private Transform _rocketContainer;
    [SerializeField] private int _rocketInitialSize;
    public ObjectPool<Projectile> RocketPool { get; private set; }

    



    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        LazgunPool = new ObjectPool<Projectile>(CreateLazer, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _lazgunInitialSize);
        AutocannonPool = new ObjectPool<Projectile>(CreateAutocannonBullet, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _autocannonInitialSize);
        RocketPool = new ObjectPool<Projectile>(CreateRocket, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, false, _rocketInitialSize);
    }

    public Projectile GetProjectile(Weapon weapon)
    {
        switch (weapon)
        {
            case Lazgun:
                return LazgunPool.Get();
            case Autocannon:
                return AutocannonPool.Get();
            case RocketPod:
                return RocketPool.Get();
        }

        return null;
    }






    private Projectile CreateLazer()
    {
        return CreateProjectile(WeaponEnum.Lazgun);
    }
    private Projectile CreateAutocannonBullet()
    {
        return CreateProjectile(WeaponEnum.Autocannon);
    }
    private Projectile CreateRocket()
    {
        return CreateProjectile(WeaponEnum.RocketPod);
    }


    /// <summary>
    /// Generic projectile instantiation and setting up.
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    private Projectile CreateProjectile(WeaponEnum weapon)
    {
        Projectile projectile = null;

        switch (weapon)
        {
            case WeaponEnum.Lazgun:
                projectile = Instantiate(_lazerPrefab, _lazerContainer);
                break;
            case WeaponEnum.Autocannon:
                projectile = Instantiate(_autocannonPrefab, _autocannonContainer);
                break;
            case WeaponEnum.RocketPod:
                projectile = Instantiate(_rocketPrefab, _rocketContainer);
                break;
            case WeaponEnum.MineDispenser:
                break;

        }

        projectile.gameObject.SetActive(false);

        return projectile;
    }
    
    private void OnTakeFromPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Projectile projectile)
    {
        Destroy(projectile.gameObject);
    }


    private void OnDestroy()
    {
        Debug.Log($"Pool Max Size\n " +
            $"Lazgun: {LazgunPool.CountAll}\n " +
            $"Autocannon: {AutocannonPool.CountAll}\n " +
            $"Rocket Pod: {RocketPool.CountAll}");
    }
}
