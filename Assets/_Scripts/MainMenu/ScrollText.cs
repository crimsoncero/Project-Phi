using UnityEngine;
using TMPro;
public class ScrollText : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] int _maxValue;
    [SerializeField] int _minValue;

    private int _valueNumber;
    private string _defaultText;

    private void OnValidate()
    {
        TryGetComponent(out _text);
    }

    private void Awake()
    {
        _defaultText = _text.text;
        _valueNumber = _minValue;
    }

    public void ChangeValue(int value)
    {
        _valueNumber += value;
        if (_valueNumber >= _maxValue)
        {
            _valueNumber = _maxValue;
        }
        if(_valueNumber <= _minValue)
        {
            _valueNumber = _minValue;
            _text.text = _defaultText;
            return;
        }
        Debug.Log(_valueNumber);
        Debug.Log(value);
        _text.text = _valueNumber.ToString();
    }
}
