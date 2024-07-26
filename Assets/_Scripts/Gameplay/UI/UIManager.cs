using ExitGames.Client.Photon;
using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviourPunCallbacks
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }
    
    private GameManager GameManager { get { return GameManager.Instance; } }


    [SerializeField] private ScoreTag _scoreTagPrefab;

    [SerializeField] private PlayerHUD _playerHUD;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private RectTransform _scoreboard;

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
    }

    private void OnDestroy()
    {
        Synchronizer.OnMatchStarted -= OnMatchStarted;
        Synchronizer.OnTimerUpdated -= UpdateTimerText;
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
    }

    private void UpdateTimerText(int time)
    {
        string s = string.Format("{0:00}:{1:00}", (time / 60), time % 60);
        _timerText.text = s;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
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

}
