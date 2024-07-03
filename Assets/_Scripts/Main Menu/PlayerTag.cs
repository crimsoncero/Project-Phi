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

    private SpaceshipConfig _config = null;

    private Player _playerInfo = null;
    public Player PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            UpdateTag();
        }
    }

    public bool IsEmpty { get { return _playerInfo == null; } }

    public void UpdateTag()
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
            UpdateColor();
        }
    }

    public void SetNotOpenSlot()
    {
        _playerInfo = null;
        _tagBackground.color = _inActiveColor;
    }

    private void UpdateColor()
    {
        if (_playerInfo.GetShipConfigID() < 0) return;
        _config = MainMenuManager.Instance.ShipConfigList.GetConfig(_playerInfo.GetShipConfigID());
        _colorImage.enabled = true;
        _colorImage.color = _config.Color;
    }
    
}
