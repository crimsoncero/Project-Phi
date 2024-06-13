using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private ConnectionManager _connectionManager;

    [SerializeField] private GameObject _connectPanel;
    [SerializeField] private GameObject _startMenuPanel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ShowReconnect()
    {
        _startMenuPanel.SetActive(false);
        _connectPanel.SetActive(true);
    }

    private void ShowStartMenu()
    {
        _startMenuPanel.SetActive(true);
        _connectPanel.SetActive(false);
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

}
