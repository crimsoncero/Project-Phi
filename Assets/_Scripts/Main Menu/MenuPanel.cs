using Photon.Pun;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }
    private MainMenuManager MainMenu { get { return MainMenuManager.Instance; } }


    public void OnQuickplay()
    {
        Con.QuickPlay();
    }

    public void OnCreateMatch()
    {
        MainMenu.ActivateCreateMatchPanel();
    }

    public void OnMatchList()
    {
        MainMenu.ActivateJoinMatchPanel();
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

}
