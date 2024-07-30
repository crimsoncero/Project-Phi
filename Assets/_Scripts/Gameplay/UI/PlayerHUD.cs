using MoreMountains.Tools;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private MMProgressBar _healthBar;

    [Header("Heat Bar")]
    [SerializeField] private MMProgressBar _heatBar;
    [GradientUsage(false)][SerializeField] private Gradient _heatBarGradient;
    private Image _heatBarImage;

    [Header("Ammo Counter")]
    [SerializeField] private Image _ammoImage;
    [SerializeField] private TMP_Text _ammoCounterText;
    [SerializeField] private Sprite _autocannonAmmo;
    [SerializeField] private Sprite _rocketAmmo;


    private Spaceship _playerSpaceship;

    private void Awake()
    {
        _heatBarImage = _heatBar.ForegroundBar.GetComponent<Image>();
    }

    private void OnEnable()
    {
        if(_playerSpaceship != null)
        {
            EnableEvents(true);
        }
    }

    private void OnDisable()
    {
        if (_playerSpaceship != null)
        {
            EnableEvents(false);
        }
    }

    private void EnableEvents(bool enable)
    {
        if(enable)
        {
            _playerSpaceship.OnHeatChanged += UpdateHeatBar;
            _playerSpaceship.OnHealthChanged += UpdateHealthBar;
            _playerSpaceship.OnSpecialFired += UpdateAmmoCounter;
            _playerSpaceship.OnSpecialChanged += UpdateAmmoSprite;
        }
        else
        {
            _playerSpaceship.OnHeatChanged -= UpdateHeatBar;
            _playerSpaceship.OnHealthChanged -= UpdateHealthBar;
            _playerSpaceship.OnSpecialFired -= UpdateAmmoCounter;
            _playerSpaceship.OnSpecialChanged -= UpdateAmmoSprite;

        }
    }

    private void UpdateAmmoSprite()
    {
        Weapon weapon = _playerSpaceship.SpecialWeapon;

        if(weapon == null)
        {
            _ammoImage.gameObject.SetActive(false);
            _ammoCounterText.gameObject.SetActive(false);
        }
        else
        {
            _ammoImage.gameObject.SetActive(true);
            _ammoCounterText.gameObject.SetActive(true);

            switch (weapon)
            {
                case Autocannon:
                    _ammoImage.sprite = _autocannonAmmo;
                    break;
                case RocketPod:
                    _ammoImage.sprite = _rocketAmmo;
                    break;
            }

            UpdateAmmoCounter();
        }
    }

    private void UpdateAmmoCounter()
    {
        _ammoCounterText.text = $"{_playerSpaceship.SpecialAmmo}/{_playerSpaceship.SpecialWeapon.MaxAmmo}";
    }

    public void Init()
    {
        _playerSpaceship = GameManager.Instance.ClientSpaceship;
        EnableEvents(true);
    }

    public void UpdateHeatBar(float progress)
    {
        // Change colors of bar. 
        if (_playerSpaceship.IsOverHeating)
            _heatBarImage.color = _heatBarGradient.Evaluate(1f);
        else
            _heatBarImage.color = _heatBarGradient.Evaluate(progress);
        
        _heatBar.UpdateBar(_playerSpaceship.PrimaryHeat, 0, _playerSpaceship.PrimaryWeapon.MaxHeat);
    }

    public void UpdateHealthBar()
    {
        _healthBar.UpdateBar(_playerSpaceship.CurrentHealth, 0, _playerSpaceship.MaxHealth);
    }


}
