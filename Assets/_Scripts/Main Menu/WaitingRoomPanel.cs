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
    [SerializeField] private ShipConfigList _shipConfigList;
    /// <summary>
    /// A key value pairs of Configs IDs and whether they are in use or not.
    /// </summary>
    private Dictionary<int, bool> _configsInUse = new Dictionary<int, bool>();
    private bool InRoom { get { return PhotonNetwork.InRoom; } }
    private Room CurrentRoom { get { return PhotonNetwork.CurrentRoom; } }
    private AsyncOperation _asyncLoad;

    private void Awake()
    {
        // Initialize configs in use dict
        foreach (var config in _shipConfigList.ConfigList)
            _configsInUse.Add(config.ID, false);
    }
    public override void OnEnable()
    {
        base.OnEnable();

        foreach(var config in _shipConfigList.ConfigList)
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

    }
    
    private IEnumerator LoadAsyncScene()
    {
        _asyncLoad = SceneManager.LoadSceneAsync("Gameplay Test Scene");

        while(!_asyncLoad.isDone)
        {
            yield return null;
        }

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
        {
            AssignConfig(newPlayer);
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        ReleaseConfig(otherPlayer);
    }


    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);



        // Handle becoming the master client, need to sync up the available configs.
        if (PhotonNetwork.IsMasterClient)
        {
            // Reset the cofig list (in case something weird happened)
            foreach (var config in _shipConfigList.ConfigList)
                _configsInUse[config.ID] = false;

            // Check what configs the current players got, and sync back the data.
            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                int configID = player.GetShipConfigID();

                if (configID < 0) continue; // Invalid config, player was not assigned a config yet

                _configsInUse[configID] = true;
            }
        }
    }

}
