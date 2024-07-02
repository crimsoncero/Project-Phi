using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private MMProgressBar _healthBar;
    private Image _healthBarImage;

    [Header("Heat Bar")]
    [SerializeField] private MMProgressBar _heatBar;
    [GradientUsage(false)][SerializeField] private Gradient _heatBarGradient;
    private Image _heatBarImage;

    

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
        }
        else
        {
            _playerSpaceship.OnHeatChanged -= UpdateHeatBar;
            _playerSpaceship.OnHealthChanged -= UpdateHealthBar;
        }
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
