using MoreMountains.Feedbacks;
using UnityEngine;

public class SoundHandler : MonoBehaviour
{
    [SerializeField] private MMF_Player _buttonPressedSFX;



    private void Start()
    {
        UIEvents.OnButtonPressed += () => _buttonPressedSFX.PlayFeedbacks();       
    }

}
