using Photon.Pun;
using UnityEngine;

public class WaitingRoomPanel : MonoBehaviour
{
    [SerializeField] private GameObject _startButton;

    private void OnEnable()
    {
        if(PhotonNetwork.IsMasterClient)
            _startButton.SetActive(true);
        else
            _startButton.SetActive(false);
    }


    public void OnStartMatch()
    {
        
    }
}
