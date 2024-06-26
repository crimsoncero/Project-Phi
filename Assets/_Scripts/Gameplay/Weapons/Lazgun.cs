using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Lazgun", menuName = "Scriptable Objects/Weapons/Lazgun")]
public class Lazgun : Weapon
{
    [SerializeField] private Vector3 _spawnPoint;
    
    [field: Space][field: Header("Overheat Mechanic")]
    [field: SerializeField] public AnimationCurve CoolingRampCurve { get; private set; }
    [field: SerializeField] public float SustainFireDuration { get; private set; }
    [field: SerializeField] public float OverheatPenalty { get; private set; }
    [field: SerializeField] public float TimeToCool { get; private set; }

    public float MaximumHeat { get { return Mathf.Ceil(SustainFireDuration * (1 / FireRate)); } }
    private float BaseCoolingRate { get { return 1 / FireRate; } }

    public override void Fire(PhotonView photonView, Vector3 shipPosition, Quaternion shipRotation, Vector2 shipVelocity, float lag, int currentAmmo)
    {
        Instantiate(ProjectilePrefab, AdjustPosition(_spawnPoint, shipPosition, shipRotation), Quaternion.identity)
             .Init(photonView.Owner, Damage, AdjustVelocity(shipVelocity, shipRotation), shipRotation, lag);
    }

    /// <summary>
    /// Get the amount that needs to be cooled at this tick, dependant on how long was cooling.
    /// </summary>
    /// <returns></returns>
    public float CalcCooling(float duration, float deltaTime)
    {
        if (duration < 0) return 0;

        // Reach max ramp after half of the sustain duration.
        duration = Mathf.Clamp(duration, 0, SustainFireDuration / 2);
        duration = Mathf.Lerp(0, 1, duration / (SustainFireDuration / 2));

        return BaseCoolingRate * CoolingRampCurve.Evaluate(duration) * deltaTime;

    }
    
}
