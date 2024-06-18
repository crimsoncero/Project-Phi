using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class WaitingRoomPanel : MonoBehaviourPunCallbacks
{
    private const string PlayerTagPath = "Prefabs\\Main Menu\\Player Tag";
    
    
    
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _playerListPanel;
    [SerializeField] private PlayerTag _playerTag;

    private List<PlayerTag> _playerTags = new List<PlayerTag>(6);


    private void OnEnable()
    {
        if(PhotonNetwork.IsMasterClient)
            _startButton.SetActive(true);
        else
            _startButton.SetActive(false);

        CreatePlayerTag(PhotonNetwork.LocalPlayer);
    }


    private void CreatePlayerTag(Player newPlayer)
    {
        if(PhotonNetwork.InRoom)
        {
            PlayerTag player = PhotonNetwork.Instantiate(PlayerTagPath, Vector3.zero, Quaternion.identity).GetComponent<PlayerTag>();
            player.transform.SetParent(_playerListPanel.transform);
            player.PlayerInfo = newPlayer;
            _playerTags.Add(player);
        }
    }


    public void OnStartMatch()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

    }

}
