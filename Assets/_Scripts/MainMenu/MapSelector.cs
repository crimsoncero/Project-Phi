using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MapSelector : MonoBehaviour
{
    [SerializeField] TMP_Text _mapText;
    [SerializeField] private Maps _maps;

    private void Start()
    {
        ChangeMapText();
    }

    public void ChangeMap(int value = 1)
    {
        int mapCount = Enum.GetValues(typeof(Maps)).Length;
        _maps = (Maps)(((int)_maps + value + mapCount) % mapCount);
        ChangeMapText();
    }

    private void ChangeMapText()
    {
        _mapText.text = _maps.ToString();
    }
}
