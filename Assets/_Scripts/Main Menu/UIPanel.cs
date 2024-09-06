using Photon.Pun;
using UnityEngine;

public abstract class UIPanel : MonoBehaviourPunCallbacks
{
    [field: SerializeField] public GameObject FirstSelect {get; private set;}
}
