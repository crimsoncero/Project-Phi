using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System;
using System.Linq;

public class MasterClientHandler : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text _masterClientNameText;
    [SerializeField] private GameObject _switchMasterClientNameButton;

    private void Start()
    {
        ActivateMasterClientButton();
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(RPC_UPDATE_MASTER_CLIENT_TEXT, RpcTarget.All, PhotonNetwork.MasterClient);
        }
    }

    private void ActivateMasterClientButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _switchMasterClientNameButton.SetActive(true);
        }
        else
        {
            _switchMasterClientNameButton.SetActive(false);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        photonView.RPC(RPC_UPDATE_MASTER_CLIENT_TEXT, RpcTarget.All, newMasterClient);
        photonView.RPC(RPC_RESET_COOLDOWN_TIMERS, RpcTarget.All);
    }

    [ContextMenu("Switch Master Client")]
    public void ChangeMasterClient()
    {
        if (!CanSwitch())
        {
            Debug.Log("There isn't an active player to switch to.");
            return;
        }

        Player candidateMC = PhotonNetwork.LocalPlayer.GetNext();

        bool success = PhotonNetwork.SetMasterClient(candidateMC);
        Debug.Log("set master client result " + success);
    }

    private bool CanSwitch()
    {
        return GameManager.Instance.SpaceshipList.Where(s => s.photonView.IsOwnerActive).Count() > 1;
    }

    public const string RPC_UPDATE_MASTER_CLIENT_TEXT = "RPC_UpdateMCName";
    [PunRPC]
    public void RPC_UpdateMCName(Player newMasterClient)
    {
        _masterClientNameText.text = newMasterClient.ToString();
        ActivateMasterClientButton();
    }
    
    public const string RPC_RESET_COOLDOWN_TIMERS = "RPC_ResetCooldownTimers";
    [PunRPC]
    public void RPC_ResetCooldownTimers()
    {
        GameManager.Instance.IncreaseShipCooldown();
        GameManager.Instance.IncreaseWeaponCooldown();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeMasterClient();
        }
    }
}
