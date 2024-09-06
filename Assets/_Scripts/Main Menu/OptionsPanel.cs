using MoreMountains.Tools;
using System;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Button _chiptuneButton;
    [SerializeField] private Button _metalButton;

    [SerializeField] private Sprite _activeButton;
    [SerializeField] private Sprite _inactiveButton;

    private MainMenuManager MainMenu { get { return MainMenuManager.Instance; } }

    public void OnChiptunePressed()
    {
        _chiptuneButton.image.sprite = _activeButton;
        _metalButton.image.sprite = _inactiveButton;

        SoundPlayer.Instance.SetMusicPack(MusicPackEnum.Chiptune);
    }
    public void OnMetalPressed()
    {
        _chiptuneButton.image.sprite = _inactiveButton;
        _metalButton.image.sprite = _activeButton;

        SoundPlayer.Instance.SetMusicPack(MusicPackEnum.Metal);
    }

    public void ReturnToMainMenu()
    {
        MainMenu.ActivateMenuPanel();
    }



}
