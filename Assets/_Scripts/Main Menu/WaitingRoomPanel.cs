    using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitingRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _startButton;

    [SerializeField] private GameObject _playerListPanel;

    [SerializeField] private List<PlayerTag> _playerTags = new List<PlayerTag>(RoomSettings.MAXPLAYERS);


    private Dictionary<int, bool> _configsInUse = new Dictionary<int, bool>();
    private bool InRoom { get { return PhotonNetwork.InRoom; } }
    private Room CurrentRoom { get { return PhotonNetwork.CurrentRoom; } }
    private AsyncOperation _asyncLoad;

    private void Awake()
    {
        // Initialize configs in use dict
        foreach (var config in MainMenuManager.Instance.ShipConfigList.ConfigList)
            _configsInUse.Add(config.ID, false);
    }
    public override void OnEnable()
    {
        base.OnEnable();

        foreach(var config in MainMenuManager.Instance.ShipConfigList.ConfigList)
        {
            _configsInUse[config.ID] = false;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            _startButton.SetActive(true);
            AssignConfig(PhotonNetwork.LocalPlayer);
        }
        else
        {
            _startButton.SetActive(false);
        }


        InitPlayerTags();


    }
    
    private IEnumerator LoadAsyncScene()
    {
        _asyncLoad = SceneManager.LoadSceneAsync("Gameplay Test Scene");

        while(!_asyncLoad.isDone)
        {
            yield return null;
        }

    }
    private void InitPlayerTags()
    {
        if (!InRoom) return;

        Player[] currentPlayers = CurrentRoom.Players.Values.ToArray();

        for(int i = 0; i < _playerTags.Count ; i++)
        {
            if(i <  currentPlayers.Length)
                _playerTags[i].PlayerInfo = currentPlayers[i];
            else if(i >= CurrentRoom.MaxPlayers)
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

    private void UpdatePlayerTags()
    {
        foreach (PlayerTag tag in _playerTags)
            tag.UpdateTag();
    }

    public void OnStartMatch()
    {
        CurrentRoom.PlayerTtl = -1;
        CurrentRoom.IsOpen = false;
        CurrentRoom.IsVisible = false;
        StartCoroutine(LoadAsyncScene());
    }

    private void AssignConfig(Player player)
    {
        int[] freeConfigs = _configsInUse.Where((c) => c.Value == false).Select((k) => k.Key).ToArray();

        if (freeConfigs.Length <= 0) return;

        int index = Random.Range(0, freeConfigs.Length);

        int configID = freeConfigs[index];
        _configsInUse[configID] = true;

        player.SetShipConfigID(configID);
        player.SetPlayerKills(0);
    }

    private void ReleaseConfig(Player player)
    {
        int configID = player.GetShipConfigID();

        _configsInUse[configID] = false;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        if (PhotonNetwork.IsMasterClient)
            AssignConfig(newPlayer);


        AddPlayerTag(newPlayer);

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        RemovePlayerTag(otherPlayer);

        ReleaseConfig(otherPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        UpdatePlayerTags();
    }



}
