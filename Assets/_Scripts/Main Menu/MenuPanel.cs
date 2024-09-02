using Photon.Pun;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    private ConnectionManager Con { get { return ConnectionManager.Instance; } }
    private MainMenuManager MainMenu { get { return MainMenuManager.Instance; } }

    [SerializeField] private QuickplayFiltering _quickplayMenu;
    [SerializeField] private Transform _menuButtons;

    public void OnQuickplay()
    {
        _menuButtons.gameObject.SetActive(false);
        _quickplayMenu.gameObject.SetActive(true);
    }

    public void OnQuickplayReturn()
    {
        _quickplayMenu.gameObject.SetActive(false);
        _menuButtons.gameObject.SetActive(true);
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
