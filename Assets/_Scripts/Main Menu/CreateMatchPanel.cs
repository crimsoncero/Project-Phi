using Photon.Pun;
using SeraphUtil;
using System;
using TMPro;
using UnityEngine;

public class CreateMatchPanel : MonoBehaviour
{
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }
    private MainMenuManager MainMenu { get { return MainMenuManager.Instance; } }

    [Header("Map Selection")]
    [SerializeField] private IntSelector _mapSelector;
    [SerializeField] private TMP_Text _mapName;
    private Maps CurrentMap
    {
        get { return _roomSettings.RoomProperties.Map; }
        set { _roomSettings.RoomProperties.Map = value; }
    }

    [Header("Nickname Selection")]
    [SerializeField] private TMP_InputField _nicknameInputField;
    private string CurrentNickname
    {
        get { return _roomSettings.RoomProperties.Nickname; }
        set { _roomSettings.RoomProperties.Nickname = value; }
    }

    [Header("Max Players Selection")]
    [SerializeField] private IntSelector _maxPlayersSelector;
    [SerializeField] private TMP_Text _maxPlayerValue;
    private int CurrentMaxPlayers
    {
        get { return _roomSettings.MaxPlayers; }
        set { _roomSettings.MaxPlayers = value; }
    }

    [Header("Timer Selection")]
    [SerializeField] private IntSelector _timerSelector;
    [SerializeField] private TMP_Text _timerValue;
    private int CurrentTimer
    {
        get { return _roomSettings.RoomProperties.Time; }
        set { _roomSettings.RoomProperties.Time = value; }
    }

    [Header("Timer Selection")]
    [SerializeField] private IntSelector _scoreSelector;
    [SerializeField] private TMP_Text _scoreValue;
    private int CurrentScore
    {
        get { return _roomSettings.RoomProperties.Score; }
        set { _roomSettings.RoomProperties.Score = value; }
    }

    [Header("Weapon Selection")]
    [SerializeField] private IntSelector _weaponSelector;
    [SerializeField] private TMP_Text _weaponValue;
    private WeaponSpawnPattern WeaponSpawn
    {
        get { return _roomSettings.RoomProperties.WeaponSpawnPattern; }
        set { _roomSettings.RoomProperties.WeaponSpawnPattern = value; }
    }

    private RoomSettings _roomSettings;
    

    private void OnEnable()
    {
        if(_roomSettings == null)
            _roomSettings = new RoomSettings();
        else
        {
            _roomSettings.GenerateRoomName();
        }



        // Init values
        _mapName.text = CurrentMap.ToString();
        _nicknameInputField.text = CurrentNickname;
        _maxPlayerValue.text = CurrentMaxPlayers.ToString();
        SetScoreValue();
        SetTimerValue();

        // Add events
        _mapSelector.ValueChanged += UpdateMap;
        _maxPlayersSelector.ValueChanged += UpdateMaxPlayers;
        _scoreSelector.ValueChanged += UpdateScore;
        _timerSelector.ValueChanged += UpdateTimer;
        _weaponSelector.ValueChanged += UpdateWeaponSpawn;
    }

    private void OnDisable()
    {
        // Remove events
        _mapSelector.ValueChanged -= UpdateMap;
        _maxPlayersSelector.ValueChanged -= UpdateMaxPlayers;
        _scoreSelector.ValueChanged -= UpdateScore;
        _timerSelector.ValueChanged -= UpdateTimer;
        _weaponSelector.ValueChanged -= UpdateWeaponSpawn;

    }


    #region Callbacks

    public void UpdateMap(int value, bool wasIncreased)
    {
        CurrentMap = (Maps)value;
        _mapName.text = CurrentMap.ToString();

    }
    public void UpdateMaxPlayers(int value, bool wasIncreased)
    {
        CurrentMaxPlayers = value;
        _maxPlayerValue.text = CurrentMaxPlayers.ToString();

    }
    public void UpdateNickname(string nickname)
    {
        if (nickname == null) // If player didnt write anything, use the default name.
            CurrentNickname = PhotonNetwork.NickName + "'s Game";
        else
        {
            CurrentNickname = nickname;
            _nicknameInputField.text = CurrentNickname;
        }
    }
    public void UpdateWeaponSpawn(int value, bool wasIncreased)
    {
        WeaponSpawn = (WeaponSpawnPattern)value;
        _weaponValue.text = WeaponSpawn.ToString();

    }
    public void UpdateScore(int value, bool wasIncreased)
    {
        CurrentScore = value;
        SetScoreValue();
    }
    public void UpdateTimer(int value, bool wasIncreased)
    {
        //if(value == -15)
        //{
        //    if(!wasIncreased)
        //        value = 0;
        //    else
        //        value = _timerSelector.Max;
        //}
        CurrentTimer = value;
        SetTimerValue();
    }

    public void ReturnToMainMenu()
    {
        MainMenu.ActivateMenuPanel();
    }

    public void CreateGame()
    {
        _roomSettings.CreateRoomUsingSettings(Con);
    }

    public void CopyRoomID()
    {
        GUIUtility.systemCopyBuffer = _roomSettings.RoomName;
    }
    #endregion


    private void SetTimerValue()
    {
        string str = "";
        if (CurrentTimer == 0)
            str = "N/A";
        else
            str = TimeSpan.FromSeconds(CurrentTimer).ToString(@"mm\:ss");

        _timerValue.text = str;
    }
    private void SetScoreValue()
    {
        string str = "";
        if (CurrentScore == 0)
            str = "N/A";
        else
            str = CurrentScore.ToString();

        _scoreValue.text = str;
    }

}

