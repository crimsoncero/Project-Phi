using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanel : UIPanel
{


    private bool isInitialConnect = true;

    [SerializeField] private GameObject _menu;
    [SerializeField] private TMP_Text _connectingText;
    [SerializeField] private TMP_Text _buttonText;

    [SerializeField] private TMP_InputField _inputField;
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    private void Start()
    {
        string defaultName = string.Empty;
        if(_inputField != null)
            if (PlayerPrefsHandler.HasNameKey())
            {
                defaultName = PlayerPrefsHandler.GetName();
                _inputField.text = defaultName;
            }

        PhotonNetwork.NickName = defaultName;
    }
    private void OnEnable()
    {
        _menu.SetActive(true);
        _connectingText.gameObject.SetActive(false);
    }

    public void SetPlayerName(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.Log("Player Name is null of empty");
            return;
        }

        PhotonNetwork.NickName = value;
        PlayerPrefsHandler.SetName(value);
    }

    public void Connect()
    {
        Con.Connect();
        if (isInitialConnect)
        {
            _buttonText.text = "Reconnect";
            isInitialConnect = false;
        }
        else
        {
            _connectingText.text = "Reconnecting...";
        }
        _menu.SetActive(false);
        _connectingText.gameObject.SetActive(true);
    }

}
