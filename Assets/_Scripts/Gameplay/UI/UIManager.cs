using ExitGames.Client.Photon;
using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviourPunCallbacks
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    

    private GameManager GameManager { get { return GameManager.Instance; } }

    [SerializeField] private PauseHandler _pauseHandler;
    [SerializeField] private ScoreTag _scoreTagPrefab;
    [SerializeField] private RectTransform _gameUI;
    [SerializeField] private PlayerHUD _playerHUD;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private RectTransform _scoreboard;
    [SerializeField] private EndgameScreen _endGameScreen;
    [SerializeField] private TMP_Text _ping;

    private List<ScoreTag> _scoreboardTags;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }

        Synchronizer.OnMatchStarted += OnMatchStarted;
        Synchronizer.OnTimerUpdated += UpdateTimerText;
        Synchronizer.OnMatchFinished += OnMatchFinished;
        PlayerController.OnPause += OnPause;

    }

    

    private void OnDestroy()
    {
        Synchronizer.OnMatchStarted -= OnMatchStarted;
        Synchronizer.OnTimerUpdated -= UpdateTimerText;
        Synchronizer.OnMatchFinished -= OnMatchFinished;
        PlayerController.OnPause -= OnPause;

    }


    public void Init()
    {

        _scoreboardTags = new List<ScoreTag>();

        _playerHUD.Init();

        foreach(var spaceship in GameManager.Instance.SpaceshipList)
        {
            ScoreTag tag = Instantiate(_scoreTagPrefab, _scoreboard);
            tag.InitTag(spaceship);
            _scoreboardTags.Add(tag);
        }

        StartCoroutine(UpdatePing());
    }

    private void UpdateTimerText(int time)
    {
        string s = string.Format("{0:00}:{1:00}", (time / 60), time % 60);
        _timerText.text = s;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        foreach(var tag in _scoreboardTags)
        {
            tag.UpdateScore();
        }
    }


    private void OnMatchStarted()
    {
        Init();
    }

    private void OnMatchFinished(EndGamePlayerData[] data)
    {
        _gameUI.gameObject.SetActive(false);
        _endGameScreen.gameObject.SetActive(true);
        _endGameScreen.Init(data);
    }

    public void OnReturnToMenu()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom(false);
        }

        SceneManager.LoadSceneAsync("Main Menu");
    }

    private IEnumerator UpdatePing()
    {
        while (true)
        {
            _ping.text = PhotonNetwork.GetPing().ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnPause(bool isPaused)
    {
        _pauseHandler.gameObject.SetActive(isPaused);
    }

    public void OnResumeFromPause()
    {
        GameManager.Instance.ClientSpaceship.PlayerController.Unpause();
        OnPause(false);
    }
}
