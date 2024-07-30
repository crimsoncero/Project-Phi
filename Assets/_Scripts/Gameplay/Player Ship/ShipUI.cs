using UnityEngine;
using MoreMountains.Tools;

public class ShipUI : MonoBehaviour
{
    [SerializeField] private MMProgressBar _healthBar;

    [SerializeField] private Spaceship _spaceship;


    private Quaternion _lastParentRotation;

    private void Start()
    {
        _spaceship.OnHealthChanged += UpdateHealthBar;
        if(_spaceship == GameManager.Instance.ClientSpaceship)
        {
            _healthBar.gameObject.SetActive(false);
        }

        _lastParentRotation = transform.parent.localRotation;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Inverse(transform.parent.localRotation) * 
                                  _lastParentRotation * transform.localRotation;

        _lastParentRotation = transform.parent.localRotation;
    }

    private void OnDestroy()
    {
        _spaceship.OnHealthChanged -= UpdateHealthBar;
    }


    private void UpdateHealthBar()
    {
        if (_spaceship == GameManager.Instance.ClientSpaceship) return;
        _healthBar.UpdateBar(_spaceship.CurrentHealth, 0, _spaceship.MaxHealth);
    }
}
