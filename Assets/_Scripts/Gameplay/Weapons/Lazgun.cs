using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "Lazgun", menuName = "Scriptable Objects/Weapons/Lazgun")]
public class Lazgun : Weapon
{
    [SerializeField] private Vector3 _spawnPoint;

    [field: Space][field: Header("Overheat Mechanic")]
    [field: SerializeField] public int HeatPerShot { get; private set; }
    [field: SerializeField] public AnimationCurve CoolingRampCurve { get; private set; }
    [field: Tooltip("At what percentage of the duration should max ramp be reached")][field: Range(0, 1)]
    [field: SerializeField] public float PercentageToMaxRamp { get; private set; }
    [field: SerializeField] public float SustainFireDuration { get; private set; }
    [field: SerializeField] public float OverheatPenalty { get; private set; }
    [field: SerializeField] public float TimeToCool { get; private set; }

    public float MaxHeat { get { return Mathf.Ceil(SustainFireDuration * FireRate * HeatPerShot); } }

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

        // Reach max ramp after reaching the percentage to max ramp;
        duration = Mathf.Clamp(duration, 0, SustainFireDuration * PercentageToMaxRamp);
        duration = Mathf.Lerp(0, 1, duration / (SustainFireDuration  * PercentageToMaxRamp));

        Debug.Log(HeatPerShot * FireRate * CoolingRampCurve.Evaluate(duration) * deltaTime);
        // Fire Rate * Heat Per Shot = Heat Per Second 
        return HeatPerShot * FireRate * CoolingRampCurve.Evaluate(duration) * deltaTime;

    }
    
}
