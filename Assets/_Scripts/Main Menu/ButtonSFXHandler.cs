using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSFXHandler : MonoBehaviour, IPointerEnterHandler
{
    private Button _button;
    [SerializeField] private bool _isPositive = true;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void OnHover()
    {
        SoundPlayer.Instance.PlayButtonSFX(ButtonSFX.Hover);
    }

    public void OnClick()
    {
        if (_isPositive)
        {
            SoundPlayer.Instance.PlayButtonSFX(ButtonSFX.Positive);
        }
        else
        {
            SoundPlayer.Instance.PlayButtonSFX(ButtonSFX.Negetive);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    
}
