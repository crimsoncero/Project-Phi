using MoreMountains.Feedbacks;
using UnityEngine;

public class ShipFeedbacks : MonoBehaviour
{
    [field: SerializeField] public MMF_Player DamageText { get; private set; }


    public void TriggerDamageText(int damage, Vector3 position)
    {
        var floatingText = DamageText.GetFeedbackOfType<MMF_FloatingText>();

        floatingText.Value = "-"+damage.ToString();

        DamageText.PlayFeedbacks(position);

    }


}
