using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIOrbiter : MonoBehaviourPunCallbacks
{
    [SerializeField] float _orbitRadius;
    [SerializeField] float _rotationSpeed;
    [SerializeField] List<PlayerTag> _tagList;

    private void Update()
    {
        float angle = transform.rotation.eulerAngles.z + (_rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void UpdateShips()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();


        for(int i = 0; i < _tagList.Count; i++)
        {
            if(i < players.Length)
            {
                float angle = (360f / PhotonNetwork.CurrentRoom.PlayerCount) * (i);
                _tagList[i].InitTag(players[i], angle, _orbitRadius, _rotationSpeed);
            }
            else
            {
                _tagList[i].HideTag();
            }
        
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (targetPlayer.GetShipConfigID() >= 0)
        {
            UpdateShips();
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        UpdateShips();
    }
}
