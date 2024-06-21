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
    private Maps _selectedMap = Maps.Random;





    private RoomSettings _roomSettings;
    

    private void OnEnable()
    {
        if(_roomSettings == null)
            _roomSettings = new RoomSettings();

        // Init values
        _mapName.text = _selectedMap.ToString();



        // Add events
        _mapSelector.ValueChanged += UpdateSelectedMap;
    }

    private void OnDisable()
    {

        // Remove events
        _mapSelector.ValueChanged -= UpdateSelectedMap;
    }


    #region Selectors Callbacks

    public void UpdateSelectedMap(int value, bool wasIncreased)
    {
        _selectedMap = (Maps)value;
        _mapName.text = _selectedMap.ToString();

    } 
    
    #endregion


}

