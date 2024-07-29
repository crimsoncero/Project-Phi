using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine;
using SeraphUtil;

public class ShipFeedbacks : MonoBehaviour
{
    [field: SerializeField] public MMF_Player DamageText { get; private set; }

    [Header("SFX")]
    [SerializeField] private float _opponentVolume;
    
    [SerializeField] private MMF_Player _fireLazgunSFX;
    [SerializeField] private MMF_Player _fireAutocannonSFX;
    [SerializeField] private MMF_Player _fireRocketSFX;



    public void TriggerDamageText(int damage, Vector3 position)
    {
        var floatingText = DamageText.GetFeedbackOfType<MMF_FloatingText>();

        floatingText.Value = "-"+damage.ToString();

        DamageText.PlayFeedbacks(position);

    }
    public void FireWeaponSFX(Weapon weapon, bool isOwner)
    {
        MMF_Player player;

        switch (weapon)
        {
            case Lazgun: player = _fireLazgunSFX; break;
            case Autocannon: player = _fireAutocannonSFX; break;
            case RocketPod: player = _fireRocketSFX; break;
            default: player = _fireLazgunSFX; break;
        }
        
        if (isOwner)
        {
            
        }
        else
        {
            var soundFeedback = player.GetFeedbackOfType<MMF_MMSoundManagerSound>();
            soundFeedback.MaxVolume = _opponentVolume;
        }

        player.PlayFeedbacks();
    }

}
