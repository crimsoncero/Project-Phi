using NUnit.Framework;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    private List<GameObject> _panelList;
    [SerializeField] private LoginPanel _loginPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _createMatchPanel;
    [SerializeField] private GameObject _joinMatchPanel;

    private void Awake()
    {
        _panelList = new List<GameObject>
        {
            _loginPanel.gameObject,
            _menuPanel.gameObject,
            _creditsPanel.gameObject,
            _createMatchPanel.gameObject,
            _joinMatchPanel.gameObject,
            _optionsPanel.gameObject
        };
    }

    private void Start()
    {
        ActivateLoginPanel();
    }



    #region Panel Activation
    private void ActivateMenuPanel()
    {
        DeactivatePanels();
        _menuPanel.gameObject.SetActive(true);
    }
    private void ActivateOptionsPanel()
    {
        DeactivatePanels();
        _optionsPanel.gameObject.SetActive(true);
    }
    private void ActivateCreditsPanel()
    {
        DeactivatePanels();
        _creditsPanel.gameObject.SetActive(true);
    }
    private void ActivateCreateMatchPanel()
    {
        DeactivatePanels();
        _createMatchPanel.gameObject.SetActive(true);
    }
    private void ActivateJoinMatchPanel()
    {
        DeactivatePanels();
        _joinMatchPanel.gameObject.SetActive(true);
    }
    private void ActivateLoginPanel()
    {
        DeactivatePanels();
        _loginPanel.gameObject.SetActive(true);
    }
    private void DeactivatePanels()
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

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ActivateLoginPanel();
    }

    #endregion
}
