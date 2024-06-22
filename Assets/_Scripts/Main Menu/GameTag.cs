using Photon.Realtime;
using TMPro;
using UnityEngine;

public class GameTag : MonoBehaviour
{
    private const string PLAYER_COUNT = "{0} \\ {1}";
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    public RoomInfo Info { get; private set; }
    public RoomProperties Properties { get; private set; }

    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _playerCount;
    
    
    public void Init(RoomInfo roomInfo)
    {
        Info = roomInfo;
        Properties = new RoomProperties(Info.CustomProperties);

    }

    public void UpdateTag()
    {
        _nickname.text = Properties.Nickname;
        _playerCount.text = string.Format(PLAYER_COUNT, Info.PlayerCount, Info.MaxPlayers);
    }

    public void JoinGame()
    {
        Con.JoinRoom(Info.Name);
    }
}
