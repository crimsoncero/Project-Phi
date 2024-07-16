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

    private static ChatManager _instance;
    public static ChatManager Instance { get => _instance; }
    public bool IsChatting { get; private set; } = false;

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
        if (_chatInputField.isFocused)
            SendMessageToChat();
        else
        {
            _chatInputField.ActivateInputField();
            IsChatting = true;
        }
    }

    public void StopTyping()
    {
        _chatInputField.text = null;
        _chatInputField.DeactivateInputField();
        IsChatting = false;
    }

    public const string RPC_SEND_MESSAGE = "RPC_SendPublicMessage";

    [PunRPC]
    private void RPC_SendPublicMessage(Player player, string playerName, string message, Vector3 colorVector)
    {
        if (_messageList.Count >= _maxMessages)
        {
            Destroy(_messageList[0].TextObject.gameObject);
            _messageList.RemoveAt(0);
        }
        Message newMessage = new();
        newMessage.Text = message;
        TMP_Text newText = Instantiate(_chatText, _chatBox.transform);
        Color color = new Color(colorVector.x, colorVector.y, colorVector.z); 
        newText.color = color; 
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
        Color color = _chatDropDownColor.options[_chatDropDownColor.value].color;
        Vector3 colorVector = new Vector3(color.r, color.g, color.b); 
        _chatPhotonView.RPC("RPC_SendPublicMessage", RpcTarget.All, PhotonNetwork.LocalPlayer, PhotonNetwork.NickName, message, colorVector);
        _chatInputField.text = "";

        StopTyping();
    }
}
