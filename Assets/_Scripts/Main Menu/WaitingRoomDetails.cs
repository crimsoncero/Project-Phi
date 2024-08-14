using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomDetails : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _scoreGoalText;
    [SerializeField] private TMP_Text _timeLimitText;
    [SerializeField] private TMP_Text _playerCountText;

    [SerializeField] private Button _startMatchButton;

    public void Init()
    {
        RoomProperties roomProps = new RoomProperties(PhotonNetwork.CurrentRoom.CustomProperties);

        if (roomProps.Score > 0)
            _scoreGoalText.text = roomProps.Score.ToString();
        else
            _scoreGoalText.text = "N/A";

        if (roomProps.Time > 0)
            _timeLimitText.text = roomProps.Time.ToString();
        else
            _timeLimitText.text = "N/A";

        string playerCount = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";

        _playerCountText.text = playerCount;

        if(PhotonNetwork.IsMasterClient)
        {
            _startMatchButton.gameObject.SetActive(true);
        }
        else
        {
            _startMatchButton.gameObject.SetActive(false);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            _startMatchButton.gameObject.SetActive(true);
        }
        else
        {
            _startMatchButton.gameObject.SetActive(false);
        }
    }

}
