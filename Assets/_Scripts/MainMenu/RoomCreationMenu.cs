//using UnityEngine;
//using TMPro;
//using System;
//using System.Net.NetworkInformation;
//using System.Linq;

//public class RoomCreationMenu : MonoBehaviour
//{
//    [SerializeField] private ConnectionManager _conManager;

//    [SerializeField] TMP_Text _roomName;
//    [SerializeField] TMP_Text _playerCount;
//    [SerializeField] TMP_Text _timerCondition;
//    [SerializeField] TMP_Text _scoreCondition;
//    [SerializeField] TMP_Text _roomID;
//    [SerializeField] TMP_Text _mapString;
//    [SerializeField] TMP_Text _weaponPatternString;

//    private RoomSetup _roomSetup;
//    private RoomProperties _roomProperties;
//    private int _playerCountNumber;
//    private int _timerNumber;
//    private int _scoreNumber;
//    private Maps _mapSelection;
//    private WeaponSpawnPattern _weaponPattern;

//    private void OnEnable()
//    {
//        CreateRoomSetup();
//    }

//    private void CreateRoomSetup()
//    {
//        _roomSetup = RoomSetup.CreateRoomInstance(_conManager.AssignRoomID());
//        _roomID.text = _roomSetup.RoomID;
//    }

//    private void SetRoomProperties()
//    {
//        if (_roomSetup == null)
//            return;
//        _roomProperties = _roomSetup.RoomProperties;
//        _roomProperties.Name = _roomName.text;
//        if (int.TryParse(_playerCount.text, out _playerCountNumber))
//        {
//            _roomProperties.PlayerCount = _playerCountNumber;
//            _roomSetup.RoomOptions.MaxPlayers = _playerCountNumber;
//        }
//        if (int.TryParse(_timerCondition.text, out _timerNumber))
//            _roomProperties.MatchTime = _timerNumber;
//        if(int.TryParse(_scoreCondition.text, out _scoreNumber))
//            _roomProperties.ScoreGoal = _scoreNumber;
//        if (Enum.TryParse(_mapString.text, out _mapSelection))
//            _roomProperties.Map = _mapSelection;
//        if (Enum.TryParse(_weaponPatternString.text, out _weaponPattern))
//            _roomProperties.WeaponSpawnPattern = _weaponPattern;
//    }

//    public void CreateMatch()
//    {
//        SetRoomProperties();
//        _conManager.CreateRoom(_roomSetup);
//    }

//    public void CopyRoomIDToClipboard()
//    {
//        if (_roomSetup.RoomID == null || _roomSetup.RoomID == "")
//            return;
//        GUIUtility.systemCopyBuffer = _roomSetup.RoomID;
//    }


//}
