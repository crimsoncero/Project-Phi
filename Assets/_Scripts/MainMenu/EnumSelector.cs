using System;
using UnityEngine;
using TMPro;

public class EnumSelector<TEnum> : MonoBehaviour where TEnum : Enum
{
    [SerializeField] TMP_Text _enumText;
    [SerializeField] private TEnum _currentEnum;

    private void OnValidate()
    {
        TryGetComponent(out _enumText);
    }

    private void Start()
    {
        ChangeEnumText();
    }

    public void ChangeEnum(int value = 1)
    {
        int enumCount = Enum.GetValues(typeof(TEnum)).Length;
        int newEnumValue = ((int)(object)_currentEnum + value + enumCount) % enumCount;
        _currentEnum = (TEnum)(object)newEnumValue;
        ChangeEnumText();
    }

    protected virtual void ChangeEnumText()
    {
        _enumText.text = _currentEnum.ToString();
    }
}
