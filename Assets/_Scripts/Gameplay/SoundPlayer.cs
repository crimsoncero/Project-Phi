using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Photon.Pun;
using System;
using Unity.VisualScripting;
using UnityEngine;

public enum MusicPackEnum
{
    Chiptune,
    Metal,
}

public enum MusicType
{
    Menu,
    Combat
}

public enum ButtonSFX
{
    Hover,
    Positive,
    Negetive,
}
public enum AudioGroups
{
    Master,
    SFX,
    Music,
}

public class SoundPlayer : MonoBehaviour
{
    private static SoundPlayer _instance;
    public static SoundPlayer Instance { get { return _instance; } }


    [Serializable]
    public struct MusicPack
    {
        public MMSMPlaylist MenuMusic;
        public MMSMPlaylist CombatMusic;
    }

    [Header("Components")]
    [SerializeField] private MMSMPlaylistManager _playlistManager;

    [Header("Music Packs")]
    [SerializeField] private MusicPack _chiptuneMusicPack;
    [SerializeField] private MusicPack _metalMusicPack;
    [field: SerializeField] public MusicPackEnum _musicPackSelector { get; private set; }

    [Header("UI SFX")]
    [SerializeField] private MMF_Player _buttonHoverSFX;
    [SerializeField] private MMF_Player _buttonPositiveSFX;
    [SerializeField] private MMF_Player _buttonNegetiveSFX;

    


    public bool IsMusicPlayerReady { get { return _playlistManager.didStart; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }
    }

    public bool PlayMusic(MusicType musicType)
    {
        MusicPack pack;

        switch (_musicPackSelector)
        {
            case MusicPackEnum.Chiptune:
                pack = _chiptuneMusicPack;
                break;
            
            case MusicPackEnum.Metal:
                pack = _metalMusicPack;
                break;

            default:
                pack = _chiptuneMusicPack;
                break;
        }

        MMSMPlaylist list;

        switch(musicType)
        {
            case MusicType.Menu:
                list = pack.MenuMusic;
                break;

            case MusicType.Combat:
                list = pack.CombatMusic;
                break;

            default:
                list = pack.MenuMusic;
                break;
        }
        
        if (_playlistManager.didStart)
        {
            _playlistManager.ChangePlaylistAndPlay(list);
            return true;
        }

        return false;
    }

    public void SetMusicPack(MusicPackEnum musicPack)
    {
        if (musicPack == _musicPackSelector) return;
        _musicPackSelector = musicPack;
        if(PhotonNetwork.InRoom)
            PlayMusic(MusicType.Combat);
        else
            PlayMusic(MusicType.Menu);
    }

    public void PlayButtonSFX(ButtonSFX buttonSFX)
    {
        switch (buttonSFX)
        {
            case ButtonSFX.Hover:
                _buttonHoverSFX.PlayFeedbacks();
                break;
            case ButtonSFX.Positive:
                _buttonPositiveSFX.PlayFeedbacks();
                break;
            case ButtonSFX.Negetive:
                _buttonNegetiveSFX.PlayFeedbacks();
                break;
        }
    }

    
}
