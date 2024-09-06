using MoreMountains.Feedbacks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private MMF_Player _sfx;


    void Start()
    {
        _sfx.PlayFeedbacks();
    }

    private void Suicide()
    {
        Destroy(this.gameObject);
    }
    
}
