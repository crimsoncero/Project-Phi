using ExitGames.Client.Photon;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    [SerializeField] private OptionsPanel _optionsPanel;
    [SerializeField] private CreateMatchPanel _createMatchPanel;
    [SerializeField] private GameListPanel _joinMatchPanel;
    [SerializeField] private WaitingRoomPanel _waitingRoomPanel;
    [SerializeField] private DirectJoinPanel _directJoinPanel;

    [SerializeField] private EventSystem _eventSystem;
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
            _createMatchPanel.gameObject,
            _joinMatchPanel.gameObject,
            _optionsPanel.gameObject,
            _waitingRoomPanel.gameObject,
            _directJoinPanel.gameObject
            };
        }
        
    }

    private IEnumerator Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            ActivateMenuPanel();
        }
        else
        {
            ActivateLoginPanel();
        }

        yield return new WaitUntil(() => SoundPlayer.Instance.IsMusicPlayerReady);
        
        SoundPlayer.Instance.PlayMusic(MusicType.Menu);

    }

    #region Panel Activation
    public void ActivateMenuPanel()
    {
        DeactivatePanels();
        _menuPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _menuPanel.FirstSelect;
    }
    public void ActivateWaitingRoomPanel()
    {
        DeactivatePanels();
        _waitingRoomPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _waitingRoomPanel.FirstSelect;

    }
    public void ActivateOptionsPanel()
    {
        DeactivatePanels();
        _optionsPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _optionsPanel.FirstSelect;

    }
    
    public void ActivateCreateMatchPanel()
    {
        DeactivatePanels();
        _createMatchPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _createMatchPanel.FirstSelect;

    }
    public void ActivateJoinMatchPanel()
    {
        DeactivatePanels();
        _joinMatchPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _joinMatchPanel.FirstSelect;

    }
    public void ActivateLoginPanel()
    {
        DeactivatePanels();
        _loginPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _loginPanel.FirstSelect;

    }
    public void ActivateDirectJoinPanel()
    {
        DeactivatePanels();
        _directJoinPanel.gameObject.SetActive(true);
        _eventSystem.firstSelectedGameObject = _directJoinPanel.FirstSelect;

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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
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
