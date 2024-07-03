using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTag : MonoBehaviour
{

    [SerializeField] private TMPro.TextMeshProUGUI _nickname;
    [SerializeField] private Image _masterSign;
    [SerializeField] private Image _tagBackground;
    [SerializeField] private Image _colorImage;


    [SerializeField] private Color _inActiveColor;

    private Player _playerInfo = null;
    public Player PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            Update();
        }
    }

    public bool IsEmpty { get { return _playerInfo == null; } }

    public void Update()
    {
        // Empty Tag Settings
        if(_playerInfo == null)
        {
            _nickname.text = string.Empty;
            _masterSign.enabled = false;
            _colorImage.enabled = false;
        }
        // Init for current player info
        else
        {
            _nickname.text = PlayerInfo.NickName;
            _masterSign.enabled = PlayerInfo.IsMasterClient;

            PlayerProperties prop = new PlayerProperties(PlayerInfo);
            if (prop.SpaceshipConfig == null) return;

            _colorImage.enabled = true;
            _colorImage.color = prop.SpaceshipConfig.Color;

        }
    }

    public void SetNotOpenSlot()
    {
        _playerInfo = null;
        _tagBackground.color = _inActiveColor;
    }

    
}
