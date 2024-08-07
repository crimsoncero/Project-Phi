using System;
using UnityEngine;

public class UIEvents : MonoBehaviour
{
    public static event Action OnButtonPressed;

    public void ButtonPressed()
    {
        OnButtonPressed?.Invoke();
    }
}
