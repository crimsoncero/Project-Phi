using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    private GameManager GameManager { get { return GameManager.Instance; } }

    [SerializeField] private PlayerHUD _playerHUD;

    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
        {
            _instance = this;
        }
    }
    public void Init()
    {
        _playerHUD.Init();
    }


}
