using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;

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
    }

    [ContextMenu("Switch Master Client")]
    public void ChangeMasterClient()
    {
        Player candidateMC = PhotonNetwork.LocalPlayer.GetNext();

        bool success = PhotonNetwork.SetMasterClient(candidateMC);
        Debug.Log("set master client result " + success);
    }

    public const string RPC_UPDATE_MASTER_CLIENT_TEXT = "RPC_UpdateMCName";
    [PunRPC]
    public void RPC_UpdateMCName(Player newMasterClient)
    {
        _masterClientNameText.text = newMasterClient.ToString();
        ActivateMasterClientButton();
    }
}
