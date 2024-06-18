using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerTag : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _nickname;

    private Player _playerInfo;
    public Player PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            _nickname.text = _playerInfo.NickName;
        }
    }

    
}
