using UnityEngine;
using UnityEngine.InputSystem;

public class SPTester : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = _player.GetComponent<PlayerController>();
        _playerController.enabled = true;
        _player.GetComponent<PlayerInput>().enabled = true;
    }
}
