using UnityEngine;
using TMPro;
using System;
using System.Net.NetworkInformation;
using System.Linq;

public class RoomCreationMenu : MonoBehaviour
{
    [SerializeField] RoomSetup _roomSetup;
    [SerializeField] TMP_Text _roomName;
    [SerializeField] TMP_Text _playerCount;
    [SerializeField] TMP_Text _timerCondition;
    [SerializeField] TMP_Text _scoreCondition;

    private RoomProperties _roomProperties;
    private int _playerCountNumber;
    private int _timerNumber;
    private int _scoreNumber;

    private void Awake()
    {
        _roomProperties = _roomSetup.RoomProperties;
    }

    public void CreateRoomUsingProperties()
    {
        _roomProperties.Name = _roomName.text;
        int.TryParse(_playerCount.text, out _playerCountNumber);
        int.TryParse(_timerCondition.text, out _timerNumber);
        bool isScore = int.TryParse(_scoreCondition.text, out _scoreNumber);
        _roomProperties.PlayerCount = _playerCountNumber;
        _roomProperties.MatchTime = _timerNumber;
        if(isScore)
            _roomProperties.ScoreGoal = _scoreNumber;
        AssignRoomID();
    }

    public void CopyRoomIDToClipboard()
    {
        GUIUtility.systemCopyBuffer = _roomSetup.RoomID;
    }

    private void AssignRoomID()
    {
        string dateTimeString = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string macAddress = GetMacAddress();

        if (macAddress == null)
        {
            Debug.LogError("Failed to retrieve MAC address.");
            return;
        }

        string concatenatedString = dateTimeString + macAddress;
        //_roomSetup.RoomID = concatenatedString.GetHashCode();
    }

    string GetMacAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var networkInterface in networkInterfaces)
        {
            if (networkInterface.OperationalStatus == OperationalStatus.Up)
            {
                var addressBytes = networkInterface.GetPhysicalAddress().GetAddressBytes();
                if (addressBytes.Length == 6)
                {
                    return string.Join(":", addressBytes.Select(b => b.ToString("X2")));
                }
            }
        }
        return null;
    }
}
