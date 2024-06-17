//using UnityEngine;
//using TMPro;
//public class ScrollText : MonoBehaviour
//{
//    [SerializeField] TMP_Text _text;
//    [SerializeField, Min(0)] int _maxValue;
//    [SerializeField, Min(0)] int _minValue;

//    private int _valueNumber;
//    private string _defaultText;

//    private void OnValidate()
//    {
//        TryGetComponent(out _text);
//    }

//    private void Awake()
//    {
//        if (_minValue > _maxValue)
//            _maxValue = _minValue;
//        _defaultText = _text.text;
//        _valueNumber = _minValue;
//    }

//    public void ChangeValue(int value)
//    {
//        _valueNumber += value;
//        if (_valueNumber >= _maxValue)
//        {
//            _valueNumber = _maxValue;
//        }
//        else if(_valueNumber <= _minValue)
//        {
//            _valueNumber = _minValue;
//            _text.text = _defaultText;
//            return;
//        }
//        _text.text = _valueNumber.ToString();
//    }
//}
