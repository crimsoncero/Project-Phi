using UnityEngine;

public class AsteroidLogic : MonoBehaviour
{

    [SerializeField] public Transform A;
    [SerializeField] public Transform B;
    [SerializeField] Transform Asteroid;

    [SerializeField,Range(0,1)] public float StartingRatio;
    [SerializeField] float Speed;
    [SerializeField] float RotationSpeed;
    [SerializeField] public float StartingRotation;

    [SerializeField] public Synchronizer SyncRef;

    private Vector3 StartingPosition;
    private float RatioSpeed;
    private float CurrentRatio;
    private float CurrentRotation;
    private float CurrentTime;
    
    

    private void OnValidate()
    {
        StartingPosition = A.position * StartingRatio + (1 - StartingRatio) * B.position;
        Asteroid.position = StartingPosition;
        RatioSpeed = Speed*(A.position - B.position).magnitude;
        Asteroid.rotation = Quaternion.Euler(new Vector3(0, 0, StartingRotation));
    }

    private void Start()
    {
        StartingPosition = A.position*StartingRatio + (1-StartingRatio)*B.position;
        CurrentRotation = StartingRotation;
        Synchronizer.OnTimerUpdated += SyncPosition;
    }

    private void Update()
    {
        if (SyncRef.MatchTimerGoal == 0)
            CurrentTime += Time.deltaTime;
        else
            CurrentTime -= Time.deltaTime;

        CurrentRatio = (StartingRatio + CurrentTime*RatioSpeed)%1;

        CurrentRotation = CurrentRotation.Decay(StartingRotation + RotationSpeed * CurrentTime, 5, Time.deltaTime);
        Asteroid.rotation = Quaternion.Euler(new Vector3 (0,0,CurrentRotation));
        Vector3 newpos = A.position * CurrentRatio + (1 - CurrentRatio) * B.position;
        if ((newpos - Asteroid.position).magnitude > 5) Asteroid.position = newpos;
        else Asteroid.position = Asteroid.position.Decay(A.position * CurrentRatio + (1 - CurrentRatio) * B.position,5,Time.deltaTime);
    }

    private void SyncPosition(int time)
    {
        CurrentTime = time;
    }
}
