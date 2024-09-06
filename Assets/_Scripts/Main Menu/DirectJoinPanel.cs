using UnityEngine;

public class DirectJoinPanel : UIPanel
{
    private string _directJoinMatchID;

    private ConnectionManager Con { get { return ConnectionManager.Instance; } }
    private MainMenuManager MainMenu { get { return MainMenuManager.Instance; } }


    private void OnEnable()
    {
        _directJoinMatchID = string.Empty;
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

    public void ReturnToMatchList()
    {
        MainMenu.ActivateJoinMatchPanel();
    }
}
