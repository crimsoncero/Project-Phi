using UnityEngine;

[CreateAssetMenu(fileName = "Spaceship Config", menuName = "Scriptable Objects/Ship/Spaceship Config")]
public class SpaceshipConfig : ScriptableObject
{
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public Material Material { get; private set; }

}
