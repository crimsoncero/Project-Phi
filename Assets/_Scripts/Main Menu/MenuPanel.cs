using Photon.Pun;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }

    [SerializeField] private GameObject _directJoinPanel;
    [SerializeField] private GameObject _menuPanel;
    private string _directJoinMatchID;

    public void OnEnable()
    {
        SwitchDirectJoinPanel(false);
    }
    public void OnQuickplay()
    {
        Con.QuickPlay();
    }

    public void OnCreateMatch()
    {

    }

    public void OnMatchList()
    {

    }

    public void OnDirectJoin()
    {
        SwitchDirectJoinPanel(true);
    }

    public void OnOptions()
    {

    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Direct Join
    public void SwitchDirectJoinPanel(bool active)
    {
        _directJoinPanel.SetActive(active);
        _menuPanel.SetActive(!active);
    }
    public void OnDirectJoinConfirm()
    {
        if (string.IsNullOrEmpty(_directJoinMatchID))
            return;
        Con.JoinRoom(_directJoinMatchID);
    }
    public void SetMatchIDString(string matchID)
    {
        _directJoinMatchID = matchID;
    }
    #endregion
}
