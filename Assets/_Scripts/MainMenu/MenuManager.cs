using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectionManager _connectionManager;

    [SerializeField] private GameObject _connectPanel;
    [SerializeField] private GameObject _startMenuPanel;
    [SerializeField] private GameObject _createGamePanel;


    public void ShowCreateGame()
    {
       DisableAllPanels();
        _createGamePanel.SetActive(true);
    } 
    
    public void ShowReconnect()
    {
        DisableAllPanels();
        _connectPanel.SetActive(true);
    }

    public void ShowStartMenu()
    {
        DisableAllPanels();
        _startMenuPanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        ShowReconnect();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        ShowStartMenu();
    }


    public void JoinMatchUsingClipboard()
    {
        string roomID = GUIUtility.systemCopyBuffer;
        _connectionManager.DirectJoin(roomID);
    }

    private void DisableAllPanels()
    {
        _connectPanel.SetActive(false);
        _startMenuPanel.SetActive(false);
        _createGamePanel.SetActive(false);
    } 

}
