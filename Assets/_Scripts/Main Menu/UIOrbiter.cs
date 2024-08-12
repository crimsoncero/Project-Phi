using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIOrbiter : MonoBehaviourPunCallbacks
{
    [SerializeField] float _orbitRadius;
    [SerializeField] float _rotationSpeed;
    [SerializeField] List<Image> _shipList;

    private void Update()
    {
        float angle = transform.rotation.eulerAngles.z + (_rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void UpdateShips()
    {

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Player[] players = PhotonNetwork.CurrentRoom.Players.Values.ToArray();


        for(int i = 0; i < _shipList.Count; i++)
        {
            if(i < players.Length)
            {
                SetTransform(i);
                _shipList[i].gameObject.SetActive(true);
                //_shipList[i].material = MainMenuManager.Instance.ShipConfigList.GetPlayerConfig(players[i]).Material;

            }
            else
            {
                _shipList[i].gameObject.SetActive(false);
            }
        
        }
    }

    private void SetTransform(int index)
    {
        Vector2 position = new Vector2();
        
        float angle = (360f / PhotonNetwork.CurrentRoom.PlayerCount) * (index);
        position.x = Mathf.Cos(angle * Mathf.Deg2Rad) * _orbitRadius;
        position.y = Mathf.Sin(angle * Mathf.Deg2Rad) * _orbitRadius;

        _shipList[index].rectTransform.anchoredPosition = position;
        _shipList[index].rectTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        UpdateShips();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        UpdateShips();
    }
}
