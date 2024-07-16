using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviourPun
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

    private static ChatManager _instance;
    public static ChatManager Instance { get => _instance; }


    private void OnValidate()
    {
        _chatPhotonView = GetComponent<PhotonView>();
        _chatInputField = GetComponentInChildren<TMP_InputField>();
        _chatDropDownColor = GetComponentInChildren<TMP_Dropdown>();
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }
    }

    public void EnableChatTyping()
    {
        _chatInputField.ActivateInputField();
    }

    public void StopTyping()
    {
        if (!_chatInputField.IsActive()) return;
        _chatInputField.text = null;
        _chatInputField.DeactivateInputField();
    }

    public const string RPC_SEND_MESSAGE = "RPC_SendPublicMessage";

    [PunRPC]
    private void RPC_SendPublicMessage(Player player, string playerName, string message, string color)
    {
        if (_messageList.Count >= _maxMessages)
        {
            Destroy(_messageList[0].TextObject.gameObject);
            _messageList.RemoveAt(0);
        }
        Message newMessage = new();
        newMessage.Text = message;
        TMP_Text newText = Instantiate(_chatText, _chatBox.transform);
        switch (color)
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
                newText.color = Color.white; 
                break;
        }
        newMessage.TextObject = newText;
        newMessage.TextObject.text = $"{playerName}: {newMessage.Text}";
        _messageList.Add(newMessage);
    }
    public void SendMessageToChat()
    {
        if (_chatInputField == null || string.IsNullOrEmpty(_chatInputField.text)) return; 
        if (_chatPhotonView == null)
            Debug.Log("photon view is null");
        string message = _chatInputField.text;
        string color = _chatDropDownColor.captionText.text; 
        _chatPhotonView.RPC("RPC_SendPublicMessage", RpcTarget.All, PhotonNetwork.LocalPlayer, PhotonNetwork.NickName, message, color);
        _chatInputField.text = "";
    }
}
