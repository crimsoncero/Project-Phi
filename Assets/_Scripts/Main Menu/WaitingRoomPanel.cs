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

    [SerializeField] private List<SpaceshipConfig> _spaceshipConfigs;

    private Dictionary<int, bool> _configsInUse = new Dictionary<int, bool>();
    private bool InRoom { get { return PhotonNetwork.InRoom; } }
    private Room CurrentRoom { get { return PhotonNetwork.CurrentRoom; } }
    private AsyncOperation _asyncLoad;

    private void Awake()
    {
        // Init configs in use dict
        foreach (var config in _spaceshipConfigs)
            _configsInUse.Add(config.ID, false);
    }
    public override void OnEnable()
    {
        base.OnEnable();

        if(PhotonNetwork.IsMasterClient)
            _startButton.SetActive(true);
        else
            _startButton.SetActive(false);


        photonView.RPC(RPC_AssingPlayerProp, RpcTarget.MasterClient, PhotonNetwork.LocalPlayer);

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

    public void OnStartMatch()
    {
        CurrentRoom.PlayerTtl = -1;
        CurrentRoom.IsOpen = false;
        CurrentRoom.IsVisible = false;
        StartCoroutine(LoadAsyncScene());
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
        ReclaimPlayerProp(otherPlayer);

    }

    public static string RPC_UpdatePlayerTags = "UpdatePlayerTags";
    [PunRPC]
    private void UpdatePlayerTags()
    {
        foreach(var tag in _playerTags)
            tag.Update();
    }


    public static string RPC_AssingPlayerProp = "AssignPlayerProp";
    [PunRPC]
    private void AssignPlayerProp(Player player)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        var freeConfigs = _configsInUse.Where((p) => p.Value == false).ToArray();

        int index = Random.Range(0, freeConfigs.Length);

        int configID = freeConfigs[index].Key;
        SpaceshipConfig config = _spaceshipConfigs[index];

        _configsInUse[config.ID] = true;
        PlayerProperties prop = new PlayerProperties();
        prop.SpaceshipConfig = config;
        player.CustomProperties = prop.HashTable;

        photonView.RPC(RPC_UpdatePlayerTags, RpcTarget.All);
    }

    private void ReclaimPlayerProp(Player player)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        SpaceshipConfig config = new PlayerProperties(player).SpaceshipConfig;

        _configsInUse[config.ID] = false;
    }

}
