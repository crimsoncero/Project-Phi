using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitingRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _playerListPanel;

    [SerializeField] private List<PlayerTag> _playerTags = new List<PlayerTag>(RoomSettings.MAXPLAYERS);

    private bool InRoom { get { return PhotonNetwork.InRoom; } }

    public override void OnEnable()
    {
        base.OnEnable();

        if(PhotonNetwork.IsMasterClient)
            _startButton.SetActive(true);
        else
            _startButton.SetActive(false);

        InitPlayerTags();
    }


    private void InitPlayerTags()
    {
        if (!InRoom) return;

        Player[] currentPlayers = PhotonNetwork.CurrentRoom.Players.Values.ToArray();

        for(int i = 0; i < _playerTags.Count ; i++)
        {
            if(i <  currentPlayers.Length)
                _playerTags[i].PlayerInfo = currentPlayers[i];
            else if(i >= PhotonNetwork.CurrentRoom.MaxPlayers)
                _playerTags[i].SetNotOpenSlot();

        }

    }

    private void AddPlayerTag(Player playerInfo)
    {
        if (!InRoom) return;

        // Go through the array of player tags, and insert on the first empty tag slot.
        foreach (PlayerTag tag in _playerTags)
        {
            if (tag.IsEmpty)
            {
                tag.PlayerInfo = playerInfo;
                break;
            }
        }
    }

    private void RemovePlayerTag(Player playerInfo)
    {
        if (!InRoom) return;

        // Find the index in the playerTag Array of the player that left.
        int i = _playerTags.FindIndex((p) => p.PlayerInfo == playerInfo);
        if (i != -1) // if -1 then the player wasn't in the list
            _playerTags[i].PlayerInfo = null;
    }

    public void OnStartMatch()
    {
        
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        AddPlayerTag(newPlayer);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RemovePlayerTag(otherPlayer);

    }

}
