using Photon.Pun;
using SeraphUtil;
using System.Collections;
using TMPro;
using UnityEngine;

public class QuickplayFiltering : UIPanel
{
    [SerializeField] private TMP_Text _mapName;
    [SerializeField] private IntSelector _mapSelector;

    private Maps _currentMap = Maps.Random;
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    private void Awake()
    {
        _mapSelector.ValueChanged += UpdateMap;
    }
    public override void OnEnable()
    {
        base.OnEnable();
        _mapSelector.Value = 0;
        _currentMap = Maps.Random;
        _mapName.text = "Any Map";
    }

    private void OnDestroy()
    {
        _mapSelector.ValueChanged -= UpdateMap;
    }
    public void UpdateMap(int value, bool wasIncreased)
    {
        _currentMap = (Maps)value;
        if (_currentMap == Maps.Random)
            _mapName.text = "Any Map";
        else
            _mapName.text = _currentMap.ToString();
    }

    public void Quickplay()
    {
        ExitGames.Client.Photon.Hashtable expectedMap = new ExitGames.Client.Photon.Hashtable();
        if (_currentMap == Maps.Random)
        {
            expectedMap = null;
        }
        else
        {
            expectedMap[RoomProperties.MAP_PROP_KEY] = _currentMap;
        }

        Con.QuickPlay(expectedMap);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        RoomSettings roomSettings = new RoomSettings();
        roomSettings.RoomProperties.Map = _currentMap;

        roomSettings.CreateRoomUsingSettings(Con);

    }
}
