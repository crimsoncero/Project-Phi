using ExitGames.Client.Photon;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviourPunCallbacks
{

    private static MainMenuManager _instance;
    public static MainMenuManager Instance { get { return _instance; } }

    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    private List<GameObject> _panelList;

    [field: SerializeField] public ShipConfigList ShipConfigList { get; set; }

    [Header("Panels")]
    [SerializeField] private LoginPanel _loginPanel;
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _createMatchPanel;
    [SerializeField] private GameObject _joinMatchPanel;
    [SerializeField] private WaitingRoomPanel _waitingRoomPanel;
    [SerializeField] private DirectJoinPanel _directJoinPanel;


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
            _panelList = new List<GameObject>
            {
            _loginPanel.gameObject,
            _menuPanel.gameObject,
            _creditsPanel.gameObject,
            _createMatchPanel.gameObject,
            _joinMatchPanel.gameObject,
            _optionsPanel.gameObject,
            _waitingRoomPanel.gameObject,
            _directJoinPanel.gameObject
            };
        }
        
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ActivateMenuPanel();
        }
        else
        {
            ActivateLoginPanel();
        }

        SoundPlayer.Instance.PlayMusic(MusicType.Menu);

    }

    #region Panel Activation
    public void ActivateMenuPanel()
    {
        DeactivatePanels();
        _menuPanel.gameObject.SetActive(true);
    }
    public void ActivateWaitingRoomPanel()
    {
        DeactivatePanels();
        _waitingRoomPanel.gameObject.SetActive(true);
    }
    public void ActivateOptionsPanel()
    {
        DeactivatePanels();
        _optionsPanel.gameObject.SetActive(true);
    }
    public void ActivateCreditsPanel()
    {
        DeactivatePanels();
        _creditsPanel.gameObject.SetActive(true);
    }
    public void ActivateCreateMatchPanel()
    {
        DeactivatePanels();
        _createMatchPanel.gameObject.SetActive(true);
    }
    public void ActivateJoinMatchPanel()
    {
        DeactivatePanels();
        _joinMatchPanel.gameObject.SetActive(true);
    }
    public void ActivateLoginPanel()
    {
        DeactivatePanels();
        _loginPanel.gameObject.SetActive(true);
    }
    public void ActivateDirectJoinPanel()
    {
        DeactivatePanels();
        _directJoinPanel.gameObject.SetActive(true);
    }
    public void DeactivatePanels()
    {
        foreach(GameObject panel in _panelList)
        {
            panel.SetActive(false);
        }
    }
    #endregion

    #region Pun Callbacks

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ActivateMenuPanel();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log("Score " + PhotonNetwork.LocalPlayer.GetScore());

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ActivateLoginPanel();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        ActivateWaitingRoomPanel();
    }
    #endregion

    
}
