using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageSender : MonoBehaviourPun
{
    [SerializeField] private PhotonView _chatPhotonView;
    [SerializeField] private TMP_InputField _chatInputField;
    [SerializeField] private GameObject _chatBox;
    [SerializeField] private TMP_Text _chatText;
    [SerializeField] private TMP_Dropdown _chatDropDownColor;
    [SerializeField] private List<Message> _messageList = new();
    [SerializeField] private int _maxMessages = 10;

    private const string RED = "Red";
    private const string GREEN = "Green";
    private const string BLUE = "Blue";
    private const string YELLOW = "Yellow";


    private void OnValidate()
    {
        _chatPhotonView = GetComponent<PhotonView>();
        _chatInputField = GetComponentInChildren<TMP_InputField>();
        _chatDropDownColor = GetComponentInChildren<TMP_Dropdown>();
    }

    public const string RPC_SEND_MESSAGE = "RPC_SendPublicMessage";
    [PunRPC]
    private void RPC_SendPublicMessage(Player player)
    {
        if(_chatPhotonView.IsMine)
        if (_chatInputField == null || _chatInputField.text == "") return;
        if (_messageList.Count >= _maxMessages)
        {
            Destroy(_messageList[0].TextObject.gameObject);
            _messageList.RemoveAt(0);
        }
        Message newMessage = new();
        newMessage.Text = _chatInputField.text;
        TMP_Text newText = Instantiate(_chatText, _chatBox.transform);
        switch (_chatDropDownColor.captionText.text)
        {
            case RED:
                newText.color = Color.red;
                break;
            case GREEN:
                newText.color = Color.green;
                break;
            case BLUE:
                newText.color = Color.blue;
                break;
            case YELLOW:
                newText.color = Color.yellow;
                break;
            default:
                break;
        }
        newMessage.TextObject = newText;
        newMessage.TextObject.text = $"{PhotonNetwork.NickName}: {newMessage.Text}";
        _messageList.Add(newMessage);
    }
    public void SendMessageToChat()
    {
        if (_chatPhotonView == null)
            Debug.Log("photon view is null");
        _chatPhotonView.RPC("RPC_SendPublicMessage", RpcTarget.All, PhotonNetwork.LocalPlayer);
    }
}
